using System.Text;

namespace LinqToYourDoom.SourceGenerators.Utilities {
	static class MathD {
		public static void Generate(StringBuilder code) {
			code.AppendLine(@"using System;");
			code.AppendLine(@"using System.Runtime.CompilerServices;");

			code.AppendLine(@"namespace LinqToYourDoom {"); {
				code.AppendLine(@"public static partial class MathD {"); {
					GenerateAvg(code);
					GenerateAbsMin(code);
					GenerateAbsMax(code);
				}
				code.AppendLine(@"}");
			}
			code.AppendLine(@"}");
		}

		static void GenerateAvg(StringBuilder code) {
			var Data = new[] {
				("sbyte", "(sbyte) (", ")"),
				("byte", "(byte) (", ")"),
				("short", "(short) (", ")"),
				("ushort", "(ushort) (", ")"),
				("int", "", ""),
				("uint", "", "U"),
				("long", "", "L"),
				("ulong", "", "UL"),
				("decimal", "", "m"),
				("float", "", "f"),
				("double", "", "d"),
			};

			foreach (var (TYPE, CAST_START, CAST_END) in Data)
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } Avg({ TYPE } a, { TYPE } b) => { CAST_START }(a + b) / 2{ CAST_END };");
		}

		static readonly string[] Data = new[] {
			"sbyte",
			"short",
			"int",
			"long",
			"decimal",
			"float",
			"double"
		};

		static void GenerateAbsMin(StringBuilder code) {
			foreach (var TYPE in Data) {
				code.AppendLine($@"/// <summary> Compares the absolutes of two values, and returns the original value corresponding to the maximum absolute. </summary>");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static { TYPE } AbsMax({ TYPE } a, { TYPE } b) => Math.Abs(a) < Math.Abs(b) ? b : a;");
			}
		}

		static void GenerateAbsMax(StringBuilder code) {
			foreach (var TYPE in Data) {
				code.AppendLine($@"/// <summary> Compares the absolutes of two values, and returns the original value corresponding to the minimum absolute. </summary>");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static { TYPE } AbsMin({ TYPE } a, { TYPE } b) => Math.Abs(a) < Math.Abs(b) ? a : b;");
			}
		}
	}
}
