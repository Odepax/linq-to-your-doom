using System.Text;

namespace LinqToYourDoom.SourceGenerators.Utilities {
	static class IRandomD {
		public static void Generate(StringBuilder code) {
			code.AppendLine(@"using System;");

			code.AppendLine(@"namespace LinqToYourDoom {"); {
				code.AppendLine(@"public partial interface IRandomD {"); {
					GenerateBetween(code);
					GenerateSign_Bool_Angle(code);
				}
				code.AppendLine(@"}");
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
				code.AppendLine($@"/// <summary>");
				code.AppendLine($@"/// Generates a random number in <c>[ <paramref name=""min""/>, <paramref name=""max""/> [</c>.");
				code.AppendLine($@"/// </summary>");
				code.AppendLine($@"/// <param name=""min""> Inclusive. </param>");
				code.AppendLine($@"/// <param name=""max""> Exclusive. </param>");
				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <c><paramref name=""max""/> &lt; <paramref name=""min""/></c>");
				code.AppendLine($@"/// and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// <paramref name=""min""/> and <paramref name=""max""/> will be silently swapped;");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentException""/> is thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public { TYPE } Between({ TYPE } min, { TYPE } max, ArgumentValidation argumentValidation = default);");

				code.AppendLine($@"/// <summary> <see cref=""Between({ TYPE }, { TYPE }, ArgumentValidation)""/> without bounds validation. </summary>");
				code.AppendLine($@"public { TYPE } UncheckedBetween({ TYPE } min, { TYPE } max);");
			}
		}

		static void GenerateSign_Bool_Angle(StringBuilder code) {
			var Data = new[] {
				("double", "d", ""),
				("float", "f", "F")
			};

			foreach (var (TYPE, LITERAL_SUFFIX, SUFFIX) in Data) {
				code.AppendLine($@"/// <param name=""chance"">");
				code.AppendLine($@"/// The probability to return <c>+1</c>. A value of <c>0.5{ LITERAL_SUFFIX }</c> means a 50% chance.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""chance""/> is outside <c>[ 0{ LITERAL_SUFFIX }, 1{ LITERAL_SUFFIX } ]</c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// <paramref name=""chance""/> will be silently clamped;");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentOutOfRangeException""/> is thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public int Sign({ TYPE } chance, ArgumentValidation argumentValidation = default);");

				code.AppendLine($@"/// <summary> <see cref=""Sign({ TYPE }, ArgumentValidation)""/> without bounds validation. </summary>");
				code.AppendLine($@"public int UncheckedSign({ TYPE } chance);");

				code.AppendLine($@"/// <param name=""chance"">");
				code.AppendLine($@"/// The probability to return <see langword=""true""/>. A value of <c>0.5{ LITERAL_SUFFIX }</c> means a 50% chance.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""chance""/> is outside <c>[ 0{ LITERAL_SUFFIX }, 1{ LITERAL_SUFFIX } ]</c> and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// <paramref name=""chance""/> will be silently clamped;");
				code.AppendLine($@"/// otherwise, an <see cref=""ArgumentOutOfRangeException""/> is thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public bool Bool({ TYPE } chance, ArgumentValidation argumentValidation = default);");

				code.AppendLine($@"/// <summary> <see cref=""Sign({ TYPE }, ArgumentValidation)""/> without bounds validation. </summary>");
				code.AppendLine($@"public bool UncheckedBool({ TYPE } chance);");

				code.AppendLine($@"/// <returns>");
				code.AppendLine($@"/// A random angle number in <c>[ 0, <see cref=""Math{ SUFFIX }.Tau""/> [</c>.");
				code.AppendLine($@"/// </returns>");
				code.AppendLine($@"public { TYPE } TauAngle{ SUFFIX }();");

				code.AppendLine($@"/// <returns>");
				code.AppendLine($@"/// A random angle number in <c>[ -<see cref=""Math{ SUFFIX }.PI""/>, +<see cref=""Math{ SUFFIX }.PI""/> [</c>.");
				code.AppendLine($@"/// </returns>");
				code.AppendLine($@"public { TYPE } PiAngle{ SUFFIX }();");
			}
		}
	}
}
