using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LinqToYourDoom;

public sealed partial class RandomD : IRandomD, IDisposable {
	// From https://docs.microsoft.com/en-us/dotnet/api/system.random?view=net-6.0#instantiating-the-random-number-generator:
	//
	// > On the .NET Framework only, because the clock has finite resolution,
	// > using the parameterless constructor to create different Random objects in close succession
	// > creates random number generators that produce identical sequences of random numbers.
	// >
	// > Note that the Random class in .NET Core does not have this limitation.
	readonly ThreadLocal<Random> ThreadLocalRandom;

	public RandomD() => ThreadLocalRandom = new(() => new());
	public RandomD(int seed) => ThreadLocalRandom = new(() => new(seed));

	public void Dispose() => ThreadLocalRandom.Dispose();

	// ----

	public T In<T>(IReadOnlyList<T> values) {
		if (values.Count == 0)
			throw new ArgumentException("The collection was empty.", nameof(values));

		return values[ThreadLocalRandom.Value!.Next(values.Count)];
	}

	public bool TryIn<T>(IReadOnlyList<T> values, [MaybeNullWhen(false)] out T? @out) {
		if (values.Count == 0) {
			@out = default;
			return false;
		}

		else {
			@out = values[ThreadLocalRandom.Value!.Next(values.Count)];
			return true;
		}
	}

	public T Pop<T>(IList<T> values) {
		if (values.Count == 0)
			throw new ArgumentException("The collection was empty.", nameof(values));

		var index = ThreadLocalRandom.Value!.Next(values.Count);
		var item = values[index];

		values.RemoveAt(index);

		return item;
	}

	public bool TryPop<T>(IList<T> values, [MaybeNullWhen(false)] out T? @out) {
		if (values.Count == 0) {
			@out = default;
			return false;
		}

		else {
			var index = ThreadLocalRandom.Value!.Next(values.Count);
			@out = values[index];

			values.RemoveAt(index);

			return true;
		}
	}

	// ----

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Bool() => ThreadLocalRandom.Value!.Next(2) == 1;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Sign() => ThreadLocalRandom.Value!.Next(2) * 2 - 1;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float TauAngleF() => ((float) ThreadLocalRandom.Value!.NextDouble()) * MathF.Tau;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float PiAngleF() => UncheckedBetween(-MathF.PI, +MathF.PI);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double TauAngle() => ThreadLocalRandom.Value!.NextDouble() * Math.Tau;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double PiAngle() => UncheckedBetween(-Math.PI, +Math.PI);
}
