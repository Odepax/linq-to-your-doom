using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace LinqToYourDoom;

public static class RegexExtensions {
	/// <summary>
	/// To get syntax coloration of the pattern, use the special <c>language=</c> comment:
	/// <c>/* lang=regex */ @"...".ToRegex()</c>.
	/// 
	/// For literals, it might actually be better to use <c>new Regex(@"...")</c> directly...
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Regex ToRegex(this string @this) => new(@this);

	/// <inheritdoc cref="ToRegex(string)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Regex ToRegex(this string @this, RegexOptions options) => new(@this, options);

	/// <inheritdoc cref="ToRegex(string)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Regex ToRegex(this string @this, RegexOptions options, TimeSpan matchTimeout) => new(@this, options, matchTimeout);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string Remove(this string @this, Regex regex) => regex.Replace(@this, string.Empty);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string Replace(this string @this, Regex regex, string replacement) => regex.Replace(@this, replacement);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string Replace(this string @this, Regex regex, string replacement, int count) => regex.Replace(@this, replacement, count);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string Replace(this string @this, Regex regex, string replacement, int count, int startIndex) => regex.Replace(@this, replacement, count, startIndex);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string Replace(this string @this, Regex regex, MatchEvaluator evaluator) => regex.Replace(@this, evaluator);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string Replace(this string @this, Regex regex, MatchEvaluator evaluator, int count) => regex.Replace(@this, evaluator, count);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string Replace(this string @this, Regex regex, MatchEvaluator evaluator, int count, int startIndex) => regex.Replace(@this, evaluator, count, startIndex);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string[] Split(this string @this, Regex regex) => regex.Split(@this);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string[] Split(this string @this, Regex regex, int count) => regex.Split(@this, count);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string[] Split(this string @this, Regex regex, int count, int startIndex) => regex.Split(@this, count, startIndex);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool Contains(this string @this, Regex regex) => regex.IsMatch(@this);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool Contains(this string @this, Regex regex, int startIndex) => regex.IsMatch(@this, startIndex);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static Match Match(this string @this, Regex regex) => regex.Match(@this);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static Match Match(this string @this, Regex regex, int startIndex) => regex.Match(@this, startIndex);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static Match Match(this string @this, Regex regex, int startIndex, int length) => regex.Match(@this, startIndex, length);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static MatchCollection Matches(this string @this, Regex regex) => regex.Matches(@this);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static MatchCollection Matches(this string @this, Regex regex, int startIndex) => regex.Matches(@this, startIndex);
}
