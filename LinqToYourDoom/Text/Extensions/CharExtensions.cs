using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using LinqToYourDoom.System;

namespace LinqToYourDoom.Text.Extensions {
	public static class CharExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsControl(this char @this) => char.IsControl(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsDigit(this char @this) => char.IsDigit(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsLetter(this char @this) => char.IsLetter(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsLetterOrDigit(this char @this) => char.IsLetterOrDigit(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsLower(this char @this) => char.IsLower(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsUpper(this char @this) => char.IsUpper(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsWhiteSpace(this char @this) => char.IsWhiteSpace(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static char ToLower(this char @this) => char.ToLower(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static char ToLower(this char @this, CultureInfo culture) => char.ToLower(@this, culture);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static char ToLowerInvariant(this char @this) => char.ToLowerInvariant(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static char ToUpper(this char @this) => char.ToUpper(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static char ToUpper(this char @this, CultureInfo culture) => char.ToUpper(@this, culture);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static char ToUpperInvariant(this char @this) => char.ToUpperInvariant(@this);

		/// <param name="argumentValidation">
		/// When <paramref name="count"/> is negative and <paramref name="argumentValidation"/> is <see cref="ArgumentValidation.Lenient"/>,
		/// <paramref name="count"/> will be silently considered <c>0</c>
		/// otherwise, an <see cref="ArgumentOutOfRangeException"/> is thrown.
		/// </param>
		public static string Repeat(this char @this, int count, ArgumentValidation argumentValidation = default) {
			if (count < 0 && argumentValidation == ArgumentValidation.Lenient)
				count = 0;

			return new(@this, count);
		}
	}
}
