using System.Text;

namespace LinqToYourDoom.SourceGenerators.Utilities {
	static class TryFunc {
		public static void Generate(StringBuilder code) {
			code.AppendLine(@"using System.Diagnostics.CodeAnalysis;");

			code.AppendLine(@"namespace LinqToYourDoom {"); {
				GenerateTryFunc(code, 16);
			}
			code.AppendLine(@"}");
		}

		static void GenerateTryFunc(StringBuilder code, int N) {
			var TYPES = "";
			var PARAMS = "";

			for (var i = 1; i <= N; ++i) {
				TYPES += "in T" + i + ", ";
				PARAMS += "T" + i + " in" + i + ", ";

				code.AppendLine($@"/// <inheritdoc cref=""TryFunc{{TResult}}""/>");
				code.AppendLine($@"public delegate bool TryFunc<{ TYPES }TResult>({ PARAMS }[MaybeNullWhen(false)] out TResult @out);");
			}
		}
	}
}
