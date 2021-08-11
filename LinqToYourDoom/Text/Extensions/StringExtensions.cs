using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace LinqToYourDoom {
	public static class StringExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool DoesNotEqual(this string? @this, string? other, StringComparison comparisonType) => !string.Equals(@this, other, comparisonType);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNullOrEmpty([NotNullWhen(false)] this string? @this) => string.IsNullOrEmpty(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? @this) => string.IsNullOrWhiteSpace(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNotNullNorEmpty([NotNullWhen(true)] this string? @this) => !string.IsNullOrEmpty(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNotNullNorWhiteSpace([NotNullWhen(true)] this string? @this) => !string.IsNullOrWhiteSpace(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string DefaultIfNullOrEmpty(this string? @this, string defaultValue) => string.IsNullOrEmpty(@this) ? defaultValue : @this;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string DefaultIfNullOrWhiteSpace(this string? @this, string defaultValue) => string.IsNullOrWhiteSpace(@this) ? defaultValue : @this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string EmptyIfNullOrEmpty(this string? @this) => string.IsNullOrEmpty(@this) ? string.Empty : @this;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string EmptyIfNullOrWhiteSpace(this string? @this) => string.IsNullOrWhiteSpace(@this) ? string.Empty : @this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string? NullIfNullOrEmpty(this string? @this) => string.IsNullOrEmpty(@this) ? null : @this;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string? NullIfNullOrWhiteSpace(this string? @this) => string.IsNullOrWhiteSpace(@this) ? null : @this;

		const string Obsolete_StringComparison = @"Always prefer overloads that include a 'StringComparison' parameter.";

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool DoesNotStartWith(this string @this, char value) => !@this.StartsWith(value);

		/// <param name="comparisonType">
		/// See <see href="https://docs.microsoft.com/en-us/dotnet/standard/base-types/best-practices-strings#choosing-a-stringcomparison-member-for-your-method-call">
		/// Choosing a StringComparison member for your method call</see>.
		///
		/// <list type="bullet">
		/// <item> <description> <see cref="StringComparison.Ordinal"/> is a bare-metal performance-friendly default; </description> </item>
		/// <item> <description> <see cref="StringComparison.OrdinalIgnoreCase"/> is good for protocol schemes, e.g.HTTP or FILE, and non-linguistic data, e.g.file paths or URL slugs; </description> </item>
		/// <item> <description> <see cref="StringComparison.CurrentCulture"/> is good for comparing and sorting display texts; </description> </item>
		/// <item> <description> The invariant culture is a locale-agnostic English-alike culture. </description> </item>
		/// </list>
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool DoesNotStartWith(this string @this, string value, StringComparison comparisonType) => !@this.StartsWith(value, comparisonType);
		[Obsolete(Obsolete_StringComparison)] public static bool DoesNotStartWith(this string @this, string value) => !@this.StartsWith(value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool DoesNotEndWith(this string @this, char value) => !@this.EndsWith(value);

		/// <inheritdoc cref="DoesNotStartWith(string, string, StringComparison)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool DoesNotEndWith(this string @this, string value, StringComparison comparisonType) => !@this.EndsWith(value, comparisonType);
		[Obsolete(Obsolete_StringComparison)] public static bool DoesNotEndWith(this string @this, string value) => !@this.EndsWith(value);

		/// <inheritdoc cref="DoesNotStartWith(string, string, StringComparison)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool DoesNotContain(this string @this, char value, StringComparison comparisonType) => !@this.Contains(value, comparisonType);
		[Obsolete(Obsolete_StringComparison)] public static bool DoesNotContain(this string @this, char value) => !@this.Contains(value);

		/// <inheritdoc cref="DoesNotStartWith(string, string, StringComparison)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool DoesNotContain(this string @this, string value, StringComparison comparisonType) => !@this.Contains(value, comparisonType);
		[Obsolete(Obsolete_StringComparison)] public static bool DoesNotContain(this string @this, string value) => !@this.Contains(value);

		public static bool TryGetCharAt(this string @this, int index, out char @char) {
			if (index < 0 || @this.Length <= index) {
				@char = default;
				return false;
			}

			else {
				@char = @this[index];
				return true;
			}
		}

		/// <param name="argumentValidation">
		/// When <paramref name="count"/> is negative and <paramref name="argumentValidation"/> is <see cref="ArgumentValidation.Lenient"/>,
		/// <paramref name="count"/> will be silently considered <c>0</c>
		/// otherwise, an <see cref="ArgumentOutOfRangeException"/> is thrown.
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Repeat(this string @this, int count, ArgumentValidation argumentValidation = default) =>
			@this.RepeatToBuilder(count, new StringBuilder(@this.Length * count.CoerceAtLeast(0)), argumentValidation).ToString();

		/// <inheritdoc cref="Repeat(string, int, ArgumentValidation)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static StringBuilder RepeatToBuilder(this string @this, int count, ArgumentValidation argumentValidation = default) =>
			@this.RepeatToBuilder(count, new StringBuilder(@this.Length * count.CoerceAtLeast(0)), argumentValidation);

		/// <inheritdoc cref="Repeat(string, int, ArgumentValidation)"/>
		public static StringBuilder RepeatToBuilder(this string @this, int count, StringBuilder output, ArgumentValidation argumentValidation = default) {
			if (count < 0 && argumentValidation == ArgumentValidation.Lenient)
				count = 0;

			return output.Insert(0, @this, count);
		}

		/// <inheritdoc cref="Repeat(string, int, ArgumentValidation)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Repeat(this string @this, int count, ArgumentValidation argumentValidation, string separator = "", string prefix = "", string suffix = "", string fallback = "") =>
			@this.RepeatToBuilder(count, new StringBuilder(@this.Length * count.CoerceAtLeast(0)), separator, prefix, suffix, fallback, argumentValidation).ToString();

		/// <inheritdoc cref="Repeat(string, int, ArgumentValidation)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static StringBuilder RepeatToBuilder(this string @this, int count, ArgumentValidation argumentValidation, string separator = "", string prefix = "", string suffix = "", string fallback = "") =>
			@this.RepeatToBuilder(count, new StringBuilder(@this.Length * count.CoerceAtLeast(0)), separator, prefix, suffix, fallback, argumentValidation);
		
		/// <inheritdoc cref="Repeat(string, int, ArgumentValidation)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static StringBuilder RepeatToBuilder(this string @this, int count, StringBuilder output, ArgumentValidation argumentValidation, string separator = "", string prefix = "", string suffix = "", string fallback = "") =>
			@this.RepeatToBuilder(count, output, separator, prefix, suffix, fallback, argumentValidation);

		/// <inheritdoc cref="Repeat(string, int, ArgumentValidation)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Repeat(this string @this, int count, string separator, string prefix = "", string suffix = "", string fallback = "", ArgumentValidation argumentValidation = default) =>
			@this.RepeatToBuilder(count, new StringBuilder(@this.Length * count.CoerceAtLeast(0)), separator, prefix, suffix, fallback, argumentValidation).ToString();

		/// <inheritdoc cref="Repeat(string, int, ArgumentValidation)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static StringBuilder RepeatToBuilder(this string @this, int count, string separator, string prefix = "", string suffix = "", string fallback = "", ArgumentValidation argumentValidation = default) =>
			@this.RepeatToBuilder(count, new StringBuilder(@this.Length * count.CoerceAtLeast(0)), separator, prefix, suffix, fallback, argumentValidation);
		
		/// <inheritdoc cref="Repeat(string, int, ArgumentValidation)"/>
		public static StringBuilder RepeatToBuilder(this string @this, int count, StringBuilder output, string separator, string prefix = "", string suffix = "", string fallback = "", ArgumentValidation argumentValidation = default) {
			if (count < 0) {
				if (argumentValidation == ArgumentValidation.Lenient)
					count = 0;

				else throw new ArgumentOutOfRangeException(nameof(count), "Cannot repeat value a negative number of times.");
			}

			if (@this.Length * count == 0)
				output.Append(fallback);

			else {
				output.Append(prefix).Append(@this);

				for (var i = 1; i < count; ++i)
					output.Append(separator).Append(@this);

				output.Append(suffix);
			}

			return output;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Remove(this string @this, string oldValue, StringComparison comparisonType = StringComparison.Ordinal) =>
			@this.Replace(oldValue, null, comparisonType);

		public static string Append(this string? @this, char suffix) => (@this ?? string.Empty) + suffix;
		public static string Append(this string? @this, string suffix) => (@this ?? string.Empty) + suffix;

		public static string Prepend(this string? @this, char prefix) => prefix + (@this ?? string.Empty);
		public static string Prepend(this string? @this, string prefix) => prefix + (@this ?? string.Empty);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte ToSByte(this string @this) => sbyte.Parse(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte ToByte(this string @this) => byte.Parse(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int ToInt(this string @this) => int.Parse(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint ToUInt(this string @this) => uint.Parse(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short ToShort(this string @this) => short.Parse(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort ToUShort(this string @this) => ushort.Parse(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long ToLong(this string @this) => long.Parse(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong ToULong(this string @this) => ulong.Parse(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float ToFloat(this string @this) => float.Parse(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double ToDouble(this string @this) => double.Parse(@this);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static decimal ToDecimal(this string @this) => decimal.Parse(@this);

		/// <summary>
		/// A version of <see cref="string.Substring(int)"/> that doesn't throw <see cref="ArgumentOutOfRangeException"/>s.
		/// </summary>
		///
		/// <param name="startIndex"> If negative, it starts from the end of the string. </param>
		public static string LenientSubstring(this string @this, int startIndex) {
			if (@this.Length == 0 || @this.Length < startIndex)
				return string.Empty;

			if (startIndex < 0) startIndex = @this.Length + startIndex; // x + (-y) == x - y)
			if (startIndex < 0) return @this;

			return @this.Substring(startIndex);
		}

		/// <summary>
		/// A version of <see cref="string.Substring(int, int)"/> that doesn't throw <see cref="ArgumentOutOfRangeException"/>s.
		/// </summary>
		///
		/// <param name="startIndex"> If negative, it starts from the end of the string. </param>
		/// <param name="length"> If negative, it corresponds to the number of characters to take out from the end of the string. </param>
		public static string LenientSubstring(this string @this, int startIndex, int length) {
			if (@this.Length == 0 || @this.Length < startIndex)
				return string.Empty;

			if (startIndex < 0) startIndex = @this.Length + startIndex; // x + (-y) == x - y)
			if (startIndex < 0) startIndex = 0;

			var tailLength = @this.Length - startIndex;

			if (length < 0) length = tailLength + length; // x + (-y) == x - y)
			if (length < 0) return string.Empty;

			if (tailLength < length) length = tailLength;

			return @this.Substring(startIndex, length);
		}
	}
}
