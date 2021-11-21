using System.Text;

namespace LinqToYourDoom.SourceGenerators.Utilities {
	static class NumberExtensions {
		public static void Generate(StringBuilder code) {
			code.AppendLine(@"using System;");
			code.AppendLine(@"using System.Runtime.CompilerServices;");

			code.AppendLine(@"namespace LinqToYourDoom {"); {
				code.AppendLine(@"public static partial class NumberExtensions {"); {
					GenerateDivRem(code);
					GenerateAbs(code);
					GenerateSign(code);
					Generate_(code);
				}
				code.AppendLine(@"}");
			}
			code.AppendLine(@"}");
		}

		static void GenerateDivRem(StringBuilder code) {
			var Data = new[] {
				("sbyte", "(sbyte) "),
				("byte", "(byte) "),
				("short", "(short) "),
				("ushort", "(ushort) "),
				("int", ""),
				("uint", ""),
				("long", ""),
				("ulong", "")
			};

			foreach (var (TYPE, CAST) in Data) {
				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""divisor""/> is <c>0</c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// <paramref name=""divisor""/> will be silently considered as <c>1</c>,");
				code.AppendLine($@"/// otherwise, a <see cref=""DivideByZeroException""/> will be thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static ({ TYPE } Quotient, { TYPE } Remainder) DivRem(this { TYPE } @this, { TYPE } divisor, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (divisor == 0 && argumentValidation == ArgumentValidation.Lenient)");
					code.AppendLine($@"return (@this, 0);");

					code.AppendLine($@"var q = @this / divisor;");
					code.AppendLine($@"var r = @this - q * divisor;");

					code.AppendLine($@"return ({ CAST }q, { CAST }r);");
				}
				code.AppendLine($@"}}");
			}
		}

		static void GenerateAbs(StringBuilder code) {
			var Data = new[] {
				"sbyte",
				"short",
				"int",
				"long",
				"decimal",
				"float",
				"double"
			};

			foreach (var TYPE in Data) {
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Abs(this { TYPE } @this) => Math.Abs(@this);");
			}
		}

		static void GenerateSign(StringBuilder code) {
			var Data = new[] {
				("double", "", true, false, "", ""),
				("float", "", true, false, "", ""),
				("decimal", "", false, false, "", ""),
				("long", "", false, true, "ulong", "63"),
				("int", "", false, true, "uint", "31"),
				("short", "(short) ", false, false, "", ""),
				("sbyte", "(sbyte) ", false, false, "", "")
			};

			foreach (var (TYPE, CAST, IS_FLOAT, SPECIAL_RETURN, UTYPE, SHIFT) in Data) {
				code.AppendLine($@"/// <param name=""zeroValue""> The value to return if <paramref name=""this""/> is <c>0</c>. </param>");
				code.AppendLine($@"///");
				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""zeroValue""/> is outside <c>{{ -1 ; 0 ; +1 }}</c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// <paramref name=""zeroValue""/> will be silently converted to its own sign,");
				code.AppendLine($@"/// otherwise an <see cref=""ArgumentOutOfRangeException""/> will be thrown.");

				if (IS_FLOAT) {
					code.AppendLine($@"///");
					code.AppendLine($@"/// When <paramref name=""this""/> is <see cref=""{ TYPE }.NaN""/> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
					code.AppendLine($@"/// <paramref name=""this""/> will be considered as <c>0</c>,");
					code.AppendLine($@"/// otherwise an <see cref=""ArithmeticException""/> will be thrown.");
				}

				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static { TYPE } Sign(this { TYPE } @this, { TYPE } zeroValue = 0, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (zeroValue != -1 && zeroValue != 0 && zeroValue != +1) {{"); {
						code.AppendLine($@"if (argumentValidation == ArgumentValidation.Lenient)");
						code.AppendLine($@"zeroValue = zeroValue < 0 ? { CAST }-1 : { CAST }+1;");
						code.AppendLine($@"else throw new ArgumentOutOfRangeException(nameof(zeroValue), "".Sign(zeroValue) must be one of {{ -1 ; 0 ; +1 }}."");");
					}
					code.AppendLine($@"}}");

					if (IS_FLOAT) {
						code.AppendLine($@"if ({ TYPE }.IsNaN(@this)) {{"); {
							code.AppendLine($@"if (argumentValidation == ArgumentValidation.Lenient)");
							code.AppendLine($@"return zeroValue;");
							code.AppendLine($@"else throw new ArithmeticException(""Cannot get the sign of NaN."");");
						}
						code.AppendLine($@"}}");
					}

					if (SPECIAL_RETURN) {
						code.AppendLine($@"return @this == 0 ? zeroValue : unchecked(@this >> { SHIFT } | ({ TYPE }) (({ UTYPE }) -@this >> { SHIFT }));");
					}

					else {
						code.AppendLine($@"return @this switch {{"); {
							code.AppendLine($@"< 0 => -1,");
							code.AppendLine($@"> 0 => +1,");
							code.AppendLine($@"_ => zeroValue");
						}
						code.AppendLine($@"}};");
					}
				}
				code.AppendLine($@"}}");
			}
		}

		static void Generate_(StringBuilder code) {
			code.AppendLine($@"static readonly double[] PowersOf10 = new[] {{ 1E0, 1E1, 1E2, 1E3, 1E4, 1E5, 1E6, 1E7, 1E8, 1E9, 1E10, 1E11, 1E12, 1E13, 1E14, 1E15 }};");
			code.AppendLine($@"static readonly float[] PowersOf10F = new[] {{ 1E0f, 1E1f, 1E2f, 1E3f, 1E4f, 1E5f, 1E6f }};");

			var Data = new[] {
				("double", "d", "", "15"),
				("float", "f", "F", "6"),
			};

			foreach (var (TYPE, LITERAL_SUFFIX, SUFFIX, MAX_POWERS) in Data) {
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNaN(this { TYPE } @this) => { TYPE }.IsNaN(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNotNaN(this { TYPE } @this) => !{ TYPE }.IsNaN(@this);");

				code.AppendLine($@"/// <param name=""fractionalDigits""> In <c>[ 0 ; { MAX_POWERS } ]</c>. </param>");
				code.AppendLine($@"///");
				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""fractionalDigits""/> is outside <c>[ 0 ; { MAX_POWERS } ]</c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// <paramref name=""fractionalDigits""/> will be silently coerced in <c>[ 0 ; { MAX_POWERS } ]</c>,");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentOutOfRangeException""/> will be thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static { TYPE } Ceiling(this { TYPE } @this, int fractionalDigits = 0, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (fractionalDigits < 0 || { MAX_POWERS } < fractionalDigits) {{"); {
						code.AppendLine($@"if (argumentValidation == ArgumentValidation.Lenient)");
						code.AppendLine($@"fractionalDigits = fractionalDigits.CoerceIn(0, { MAX_POWERS });");

						code.AppendLine($@"else throw new ArgumentOutOfRangeException(nameof(fractionalDigits), ""Cannot ceil outside of 0 to { MAX_POWERS } fractional digits."");");
					}
					code.AppendLine($@"}}");

					code.AppendLine($@"return Math{ SUFFIX }.Ceiling(@this * PowersOf10{ SUFFIX }[fractionalDigits]) / PowersOf10{ SUFFIX }[fractionalDigits];");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <param name=""fractionalDigits""> In <c>[ 0 ; { MAX_POWERS } ]</c>. </param>");
				code.AppendLine($@"///");
				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""fractionalDigits""/> is outside <c>[ 0 ; { MAX_POWERS } ]</c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// <paramref name=""fractionalDigits""/> will be silently coerced in <c>[ 0 ; { MAX_POWERS } ]</c>,");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentOutOfRangeException""/> will be thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static { TYPE } Floor(this { TYPE } @this, int fractionalDigits = 0, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (fractionalDigits < 0 || { MAX_POWERS } < fractionalDigits) {{"); {
						code.AppendLine($@"if (argumentValidation == ArgumentValidation.Lenient)");
						code.AppendLine($@"fractionalDigits = fractionalDigits.CoerceIn(0, { MAX_POWERS });");

						code.AppendLine($@"else throw new ArgumentOutOfRangeException(nameof(fractionalDigits), ""Cannot ceil outside of 0 to { MAX_POWERS } fractional digits."");");
					}
					code.AppendLine($@"}}");

					code.AppendLine($@"return Math{ SUFFIX }.Floor(@this * PowersOf10{ SUFFIX }[fractionalDigits]) / PowersOf10{ SUFFIX }[fractionalDigits];");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <param name=""fractionalDigits""> In <c>[ 0 ; { MAX_POWERS } ]</c>. </param>");
				code.AppendLine($@"///");
				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""fractionalDigits""/> is outside <c>[ 0 ; { MAX_POWERS } ]</c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// <paramref name=""fractionalDigits""/> will be silently coerced in <c>[ 0 ; { MAX_POWERS } ]</c>,");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentOutOfRangeException""/> will be thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static { TYPE } Round(this { TYPE } @this, int fractionalDigits = 0, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if ((fractionalDigits < 0 || { MAX_POWERS } < fractionalDigits) && argumentValidation == ArgumentValidation.Lenient)");
					code.AppendLine($@"fractionalDigits = fractionalDigits.CoerceIn(0, { MAX_POWERS });");

					code.AppendLine($@"return Math{ SUFFIX }.Round(@this, fractionalDigits, MidpointRounding.AwayFromZero);");

				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <summary> Linear interpolation. </summary>");
				code.AppendLine($@"///");
				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <c><paramref name=""upperIn""/> &lt; <paramref name=""lowerIn""/></c> or <c><paramref name=""upperOut""/> &lt; <paramref name=""lowerOut""/></c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// the unordered bounds will be silently swapped;");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentException""/> will be thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static { TYPE } Lerp(this { TYPE } @this, { TYPE } lowerIn, { TYPE } upperIn, { TYPE } lowerOut, { TYPE } upperOut, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (upperIn < lowerIn) {{"); {
						code.AppendLine($@"if (argumentValidation == ArgumentValidation.Lenient)");
						code.AppendLine($@"(lowerIn, upperIn) = (upperIn, lowerIn);");

						code.AppendLine($@"else throw new ArgumentException("".Lerp(lowerIn, upperIn) must be called with parameters that respect lowerIn <= upperIn."", nameof(upperIn));");
					}
					code.AppendLine($@"}}");

					code.AppendLine($@"if (upperOut < lowerOut) {{"); {
						code.AppendLine($@"if (argumentValidation == ArgumentValidation.Lenient)");
						code.AppendLine($@"(lowerOut, upperOut) = (upperOut, lowerOut);");

						code.AppendLine($@"else throw new ArgumentException("".Lerp(lowerOut, upperOut) must be called with parameters that respect lowerOut <= upperOut."", nameof(upperOut));");
					}
					code.AppendLine($@"}}");

					code.AppendLine($@"return UncheckedLerp(@this, lowerIn, upperIn, lowerOut, upperOut);");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <summary> <see cref=""Lerp({ TYPE }, { TYPE }, { TYPE }, { TYPE }, { TYPE }, ArgumentValidation)""/> without bounds validation. </summary>");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static { TYPE } UncheckedLerp(this { TYPE } @this, { TYPE } lowerIn, { TYPE } upperIn, { TYPE } lowerOut, { TYPE } upperOut) {{"); {
					code.AppendLine($@"// Stolen from https://www.alanzucconi.com/2021/01/24/linear-interpolation/");
					code.AppendLine($@"var difference = upperIn - lowerIn;");

					code.AppendLine($@"return difference == 0{ LITERAL_SUFFIX }");
					code.AppendLine($@"? (lowerOut + upperOut) / 2{ LITERAL_SUFFIX }");
					code.AppendLine($@": lowerOut + (@this - lowerIn) * (upperOut - lowerOut) / difference;");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Pow(this { TYPE } @this, { TYPE } power) => Math{ SUFFIX }.Pow(@this, power);");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Sqrt(this { TYPE } @this) => Math{ SUFFIX }.Sqrt(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Cbrt(this { TYPE } @this) => Math{ SUFFIX }.Cbrt(@this);");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Exp(this { TYPE } @this) => Math{ SUFFIX }.Exp(@this);");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Log(this { TYPE } @this) => Math{ SUFFIX }.Log(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Log2(this { TYPE } @this) => Math{ SUFFIX }.Log2(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Log10(this { TYPE } @this) => Math{ SUFFIX }.Log10(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Log(this { TYPE } @this, { TYPE } newBase) => Math{ SUFFIX }.Log(@this, newBase);");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Cos(this { TYPE } @this) => Math{ SUFFIX }.Cos(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Sin(this { TYPE } @this) => Math{ SUFFIX }.Sin(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Tan(this { TYPE } @this) => Math{ SUFFIX }.Tan(@this);");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Cosh(this { TYPE } @this) => Math{ SUFFIX }.Cosh(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Sinh(this { TYPE } @this) => Math{ SUFFIX }.Sinh(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Tanh(this { TYPE } @this) => Math{ SUFFIX }.Tanh(@this);");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Acos(this { TYPE } @this) => Math{ SUFFIX }.Acos(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Asin(this { TYPE } @this) => Math{ SUFFIX }.Asin(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Atan(this { TYPE } @this) => Math{ SUFFIX }.Atan(@this);");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Acosh(this { TYPE } @this) => Math{ SUFFIX }.Acosh(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Asinh(this { TYPE } @this) => Math{ SUFFIX }.Asinh(@this);");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Atanh(this { TYPE } @this) => Math{ SUFFIX }.Atanh(@this);");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static { TYPE } ProgressRatio(this { TYPE } @this, { TYPE } upper) => @this / upper;");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static { TYPE } DeclineRatio(this { TYPE } @this, { TYPE } upper) => (upper - @this) / upper;");

				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <c><paramref name=""upper""/> &lt; <paramref name=""lower""/></c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// the unordered bounds will be silently swapped;");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentException""/> will be thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static { TYPE } ProgressRatio(this { TYPE } @this, { TYPE } lower, { TYPE } upper, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (upper < lower) {{"); {
						code.AppendLine($@"if (argumentValidation == ArgumentValidation.Lenient)");
						code.AppendLine($@"(lower, upper) = (upper, lower);");

						code.AppendLine($@"else throw new ArgumentException("".ProgressRatio(lower, upper) must be called with parameters that respect lower <= upper."", nameof(upper));");
					}
					code.AppendLine($@"}}");

					code.AppendLine($@"return UncheckedProgressRatio(@this, lower, upper);");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <summary> <see cref=""ProgressRatio({ TYPE }, { TYPE }, { TYPE }, ArgumentValidation)""/> without bounds validation. </summary>");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static { TYPE } UncheckedProgressRatio(this { TYPE } @this, { TYPE } lower, { TYPE } upper) => (@this - lower) / (upper - lower);");

				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <c><paramref name=""upper""/> &lt; <paramref name=""lower""/></c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// the unordered bounds will be silently swapped;");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentException""/> will be thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static { TYPE } DeclineRatio(this { TYPE } @this, { TYPE } lower, { TYPE } upper, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (upper < lower) {{"); {
						code.AppendLine($@"if (argumentValidation == ArgumentValidation.Lenient)");
						code.AppendLine($@"(lower, upper) = (upper, lower);");

						code.AppendLine($@"else throw new ArgumentException("".DeclineRatio(lower, upper) must be called with parameters that respect lower <= upper."", nameof(upper));");
					}
					code.AppendLine($@"}}");

					code.AppendLine($@"return UncheckedDeclineRatio(@this, lower, upper);");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <summary> <see cref=""DeclineRatio({ TYPE }, { TYPE }, { TYPE }, ArgumentValidation)""/> without bounds validation. </summary>");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static { TYPE } UncheckedDeclineRatio(this { TYPE } @this, { TYPE } lower, { TYPE } upper) => (upper - @this) / (upper - lower);");

				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""multiple""/> is <c>0</c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// <see cref=""{ TYPE }.NaN""/> will be returned;");
				code.AppendLine($@"/// when <paramref name=""multiple""/> is negative, the result is effectively the same as <see cref=""FloorToMultiple({ TYPE }, { TYPE }, ArgumentValidation)""/>;");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentOutOfRangeException""/> will be thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static { TYPE } CeilingToMultiple(this { TYPE } @this, { TYPE } multiple, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (multiple <= 0 && argumentValidation == ArgumentValidation.Strict)");
					code.AppendLine($@"throw new ArgumentOutOfRangeException(nameof(multiple), ""Cannot ceil to the nearest multiple of zero or negative."");");

					code.AppendLine($@"return Math{ SUFFIX }.Ceiling(@this / multiple) * multiple;");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""multiple""/> is <c>0</c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// <see cref=""{ TYPE }.NaN""/> will be returned;");
				code.AppendLine($@"/// when <paramref name=""multiple""/> is negative, the result is effectively the same as <see cref=""CeilingToMultiple({ TYPE }, { TYPE }, ArgumentValidation)""/>;");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentOutOfRangeException""/> will be thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static { TYPE } FloorToMultiple(this { TYPE } @this, { TYPE } multiple, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (multiple <= 0 && argumentValidation == ArgumentValidation.Strict)");
					code.AppendLine($@"throw new ArgumentOutOfRangeException(nameof(multiple), ""Cannot floor to the nearest multiple of zero or negative."");");

					code.AppendLine($@"return Math{ SUFFIX }.Floor(@this / multiple) * multiple;");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""multiple""/> is <c>0</c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// <see cref=""{ TYPE }.NaN""/> will be returned;");
				code.AppendLine($@"/// when <paramref name=""multiple""/> is negative, the result is effectively the same as when it's positive;");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentOutOfRangeException""/> will be thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static { TYPE } RoundToMultiple(this { TYPE } @this, { TYPE } multiple, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (multiple <= 0 && argumentValidation == ArgumentValidation.Strict)");
					code.AppendLine($@"throw new ArgumentOutOfRangeException(nameof(multiple), ""Cannot round to the nearest multiple of zero or negative."");");

					code.AppendLine($@"return Math{ SUFFIX }.Round(@this / multiple, MidpointRounding.AwayFromZero) * multiple;");
				}
				code.AppendLine($@"}}");
			}
		}
	}
}
