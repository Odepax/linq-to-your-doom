using System.Text;

namespace LinqToYourDoom.SourceGenerators.Utilities {
	static class BooleanExtensions {
		public static void Generate(StringBuilder code) {
			code.AppendLine(@"using System.Runtime.CompilerServices;");

			code.AppendLine(@"namespace LinqToYourDoom;");

			code.AppendLine(@"public static partial class BooleanExtensions {"); {
				GenerateTo_(code);
				GenerateToBool(code);
			}
			code.AppendLine(@"}");
		}

		static readonly (string, string, string, string, string, string)[] Data = new[] {
			("sbyte", "", "", "SByte", "(sbyte)1", "(sbyte)0"),
			("byte", "", "", "Byte", "(byte)1", "(byte)0"),
			("short", "", "", "Short", "(short)1", "(short)0"),
			("ushort", "", "", "UShort", "(ushort)1", "(ushort)0"),
			("int", "", "", "Int", "1", "0"),
			("uint", "", "u", "UInt", "1u", "0u"),
			("long", "", "L", "Long", "1L", "0L"),
			("ulong", "", "ul", "ULong", "1ul", "0ul"),
			("decimal", "", "m", "Decimal", "1m", "0m"),
			("float", @" or <see cref=""float.NaN""/>", "f && !float.IsNaN(@this)", "Float", "1f", "0f"),
			("double", @" or <see cref=""double.NaN""/>", "d && !double.IsNaN(@this)", "Double", "1d", "0d")
		};

		static void GenerateTo_(StringBuilder code) {
			foreach (var (TYPE, _, _, PASCAL_TYPE, ONE, ZERO) in Data) {
				code.AppendLine($@"/// <summary> Converts <see langword=""true""/> to <c>1</c> and <see langword=""false""/> to <c>0</c>. </summary>");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static { TYPE } To{ PASCAL_TYPE }(this bool @this) => @this ? { ONE } : { ZERO };");
			}
		}

		static void GenerateToBool(StringBuilder code) {
			foreach (var (TYPE, EXTRA_FALSE, END, _, _, _) in Data) {
				code.AppendLine($@"/// <summary> Returns <see langword=""false""/> is <paramref name=""this""/> is <c>0</c>{ EXTRA_FALSE }, <see langword=""true""/> otherwise. </summary>");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ToBool(this { TYPE } @this) => @this != 0{ END };");
			}
		}
	}
}
