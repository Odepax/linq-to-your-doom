using System.Runtime.CompilerServices;
using System.Text;

namespace LinqToYourDoom {
	public static class StringBuilderExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static StringBuilder ToBuilder(this string? @this) => new(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IndentedStringBuilder ToIndentedBuilder(this string? @this, string indentString = "\t") => new(@this, indentString);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IndentedStringBuilder ToIndentedBuilder(this StringBuilder? @this, string indentString = "\t") => new(@this, indentString);
	}
}
