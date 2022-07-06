using System;
using System.Runtime.CompilerServices;

namespace LinqToYourDoom;

/// <summary> <i>D</i> stands for <i>Doom</i>, of course... </summary>
public static partial class MathD {
	public const double SQRT2 = 1.4142135623730951;

	/// <summary>
	/// Cosine or sine of 45deg.
	/// </summary>
	public const double SQRT1_2 = 0.7071067811865476;

	public const float SQRT2F = 1.4142135623f;

	/// <inheritdoc cref="SQRT1_2"/>
	public const float SQRT1_2F = 0.70710678f;

	public static readonly RandomD Random = new();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Min<T>(T a, T b) where T : IComparable<T> =>
		a.CompareTo(b) < 0 ? a : b;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Max<T>(T a, T b) where T : IComparable<T> =>
		a.CompareTo(b) < 0 ? b : a;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (T Min, T Max) MinMax<T>(T a, T b) where T : IComparable<T> =>
		a.CompareTo(b) < 0 ? (a, b) : (b, a);
}
