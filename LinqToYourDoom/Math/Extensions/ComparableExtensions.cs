using System;
using System.Runtime.CompilerServices;

namespace LinqToYourDoom {
	public static class ComparableExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T CoerceAtLeast<T>(this T @this, T min) where T : IComparable<T> =>
			@this.CompareTo(min) < 0 ? min : @this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T CoerceAtMost<T>(this T @this, T max) where T : IComparable<T> =>
			max.CompareTo(@this) < 0 ? max : @this;

		/// <param name="argumentValidation">
		/// When <c><paramref name="max"/> &lt; <paramref name="min"/></c>
		/// and <paramref name="argumentValidation"/> is <see cref="ArgumentValidation.Lenient"/>,
		/// <paramref name="min"/> and <paramref name="max"/> will be silently swapped;
		/// otherwise, an <see cref="ArgumentException"/> is thrown.
		/// </param>
		public static T CoerceIn<T>(this T @this, T min, T max, ArgumentValidation argumentValidation = default) where T : IComparable<T> {
			if (max.CompareTo(min) < 0) {
				if (argumentValidation == ArgumentValidation.Lenient)
					(min, max) = (max, min);

				else throw new ArgumentException(".CoerceIn(min, max) must be called with parameters that respect min <= max.", nameof(max));
			}

			return @this.UncheckedCoerceIn(min, max);
		}

		/// <summary>
		/// <see cref="CoerceIn{T}(T, T, T, ArgumentValidation)"/>, but without bounds validation.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T UncheckedCoerceIn<T>(this T @this, T min, T max) where T : IComparable<T> =>
			  @this.CompareTo(min) < 0 ? min
			: max.CompareTo(@this) < 0 ? max
			: @this;
	}
}
