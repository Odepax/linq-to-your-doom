using System.Text;

namespace LinqToYourDoom.SourceGenerators.Utilities {
	static class RandomD {
		public static void Generate(StringBuilder code) {
			code.AppendLine(@"using System;");
			code.AppendLine(@"using System.Runtime.CompilerServices;");

			code.AppendLine(@"namespace LinqToYourDoom;");

			code.AppendLine(@"public sealed partial class RandomD {"); {
				GenerateBetween(code);
				GenerateUncheckedBetween(code);
				GenerateSign_Bool(code);
			}
			code.AppendLine(@"}");
		}

		static void GenerateBetween(StringBuilder code) {
			var Data = new[] {
				"sbyte",
				"byte",
				"short",
				"ushort",
				"int",
				"uint",
				"long",
				"ulong",
				"decimal",
				"float",
				"double"
			};

			foreach (var TYPE in Data) {
				code.AppendLine($@"public { TYPE } Between({ TYPE } min, { TYPE } max, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (max < min) {{"); {
						code.AppendLine($@"if (argumentValidation == ArgumentValidation.Lenient)");
						code.AppendLine($@"(min, max) = (max, min);");

						code.AppendLine($@"else throw new ArgumentException("".Between(min, max) must be called with parameters that respect min <= max."", nameof(max));");
					}
					code.AppendLine($@"}}");

					code.AppendLine($@"return UncheckedBetween(min, max);");
				}
				code.AppendLine($@"}}");
			}
		}

		static void GenerateUncheckedBetween(StringBuilder code) {
			foreach (var (TYPE, CAST) in new[] {
				("sbyte", "(sbyte) "),
				("byte", "(byte) "),
				("short", "(short) "),
				("ushort", "(ushort) "),
				("int", "")
			}) {
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public { TYPE } UncheckedBetween({ TYPE } min, { TYPE } max) => { CAST }ThreadLocalRandom.Value!.Next(min, max);");
			}

			foreach (var (TYPE, CAST_START, CAST_END) in new[] {
				("uint", "(uint) (", ")"),
				("long", "(long) (", ")"),
				("ulong", "(ulong) (", ")"),
			}) {
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public { TYPE } UncheckedBetween({ TYPE } min, { TYPE } max) => { CAST_START }ThreadLocalRandom.Value!.NextDouble() * (max - min) + min{ CAST_END };");
			}

			foreach (var (TYPE, CAST_START, CAST_END) in new[] {
				("decimal", "((decimal) ", ")"),
				("float", "((float) ", ")"),
				("double", "", "")
			}) {
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public { TYPE } UncheckedBetween({ TYPE } min, { TYPE } max) => { CAST_START }ThreadLocalRandom.Value!.NextDouble(){ CAST_END } * (max - min) + min;");
			}
		}

		static void GenerateSign_Bool(StringBuilder code) {
			var Data = new[] {
				("float", "f"),
				("double", "d")
			};

			foreach (var (TYPE, LITERAL_SUFFIX) in Data) {
				code.AppendLine($@"public bool Bool({ TYPE } chance, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (chance < 0{ LITERAL_SUFFIX } || 1{ LITERAL_SUFFIX } < chance) {{"); {
						code.AppendLine($@"if (argumentValidation == ArgumentValidation.Lenient)");
						code.AppendLine($@"chance = chance.UncheckedCoerceIn(0{ LITERAL_SUFFIX }, 1{ LITERAL_SUFFIX });");

						code.AppendLine($@"else throw new ArgumentOutOfRangeException(nameof(chance), "".Bool(chance) must be in [ 0, 1 ]."");");
					}
					code.AppendLine($@"}}");

					code.AppendLine($@"return UncheckedBool(chance);");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public bool UncheckedBool({ TYPE } chance) => ThreadLocalRandom.Value!.NextDouble() < chance;");

				code.AppendLine($@"public int Sign({ TYPE } chance, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"if (chance < 0{ LITERAL_SUFFIX } || 1{ LITERAL_SUFFIX } < chance) {{"); {
						code.AppendLine($@"if (argumentValidation == ArgumentValidation.Lenient)");
						code.AppendLine($@"chance = chance.UncheckedCoerceIn(0{ LITERAL_SUFFIX }, 1{ LITERAL_SUFFIX });");

						code.AppendLine($@"else throw new ArgumentOutOfRangeException(nameof(chance), "".Sign(chance) must be in [ 0, 1 ]."");");
					}
					code.AppendLine($@"}}");

					code.AppendLine($@"return UncheckedSign(chance);");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public int UncheckedSign({ TYPE } chance) => UncheckedBool(chance) ? +1 : -1;");
			}
		}
	}
}
