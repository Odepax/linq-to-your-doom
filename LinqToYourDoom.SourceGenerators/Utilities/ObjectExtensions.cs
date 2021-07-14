using System.Text;

namespace LinqToYourDoom.SourceGenerators.Utilities {
	static class ObjectExtensions {
		public static void Generate(StringBuilder code) {
			code.AppendLine(@"using System;");
			code.AppendLine(@"using System.Collections;");
			code.AppendLine(@"using System.Collections.Generic;");
			code.AppendLine(@"using System.Linq;");
			code.AppendLine(@"using System.Runtime.CompilerServices;");

			code.AppendLine(@"namespace LinqToYourDoom.System.Extensions {"); {
				code.AppendLine(@"public static partial class ObjectExtensions {"); {
					GenerateToVariable(code, 8);
					GenerateTo(code, 8);
					GenerateDo(code, 8);
				}
				code.AppendLine(@"}");
			}
			code.AppendLine(@"}");
		}

		static void GenerateToVariable(StringBuilder code, int N) {
			var TYPES = "T1";
			var PARAMS = "out T1 out1";
			var ASSIGNS = "out1";

			for (var i = 2; i <= N; ++i) {
				TYPES += ", T" + i;
				PARAMS += ", out T" + i + " out" + i;
				ASSIGNS += ", out" + i;

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ({ TYPES }) ToVariable<{ TYPES }>(this ({ TYPES }) @this, { PARAMS }) => ({ ASSIGNS }) = @this;");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static TIn ToVariable<TIn, { TYPES }>(this TIn @this, { PARAMS }, Func<TIn, ({ TYPES })> selector) {{ ({ ASSIGNS }) = selector.Invoke(@this); return @this; }}");
			}
		}

		static void GenerateTo(StringBuilder code, int N) {
			var TYPES = "T1";
			var ITEMS = "@this.Item1";

			for (var i = 2; i <= N; ++i) {
				TYPES += ", T" + i;
				ITEMS += ", @this.Item" + i;

				code.AppendLine($@"/// <summary>");
				code.AppendLine($@"/// Equivalent of <see cref=""Enumerable.Select{{TSource, TResult}}(IEnumerable{{TSource}}, Func{{TSource, TResult}})""/> for a value tuple.");
				code.AppendLine($@"///");
				code.AppendLine($@"/// Note that <see cref=""Enumerable.Select{{TSource, TResult}}(IEnumerable{{TSource}}, Func{{TSource, TResult}})""/> is <b>lazy</b>,");
				code.AppendLine($@"/// whereas <see cref=""To{{{ TYPES }, TOut}}(({ TYPES }), Func{{{ TYPES }, TOut}})""/> is <b>eager</b>.");
				code.AppendLine($@"/// </summary>");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static TOut To<{ TYPES }, TOut>(this ({ TYPES }) @this, Func<{ TYPES }, TOut> selector) => selector.Invoke({ ITEMS });");
			}
		}

		static void GenerateDo(StringBuilder code, int N) {
			var TYPES = "T1";
			var ITEMS = "@this.Item1";

			for (var i = 2; i <= N; ++i) {
				TYPES += ", T" + i;
				ITEMS += ", @this.Item" + i;

				code.AppendLine($@"/// <summary>");
				code.AppendLine($@"/// Equivalent of <see cref=""List{{T}}.ForEach(Action{{T}})""/> for a value tuple.");
				code.AppendLine($@"///");
				code.AppendLine($@"/// Note that both <see cref=""List{{T}}.ForEach(Action{{T}})""/>");
				code.AppendLine($@"/// and <see cref=""Do{{{ TYPES }}}(({ TYPES }), Action{{{ TYPES }}})""/> are <b>eager</b>.");
				code.AppendLine($@"/// </summary>");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static ({ TYPES }) Do<{ TYPES }>(this ({ TYPES }) @this, Action<{ TYPES }> action) {{"); {
					code.AppendLine($@"action.Invoke({ ITEMS });");
					code.AppendLine($@"return @this;");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <summary>");
				code.AppendLine($@"/// Equivalent of <see cref=""Do{{{ TYPES }}}(({ TYPES }), Action{{{ TYPES }}})""/>,");
				code.AppendLine($@"/// but taking a <see cref=""Func{{{ TYPES }, TResult}}""/> and discarding the returned value.");
				code.AppendLine($@"///");
				code.AppendLine($@"/// This override discards the returned value instead of blocking the compilation");
				code.AppendLine($@"/// when <paramref name=""action""/> is a method that does not return <see cref=""void""/>.");
				code.AppendLine($@"/// </summary>");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static ({ TYPES }) Do<{ TYPES }, T_>(this ({ TYPES }) @this, Func<{ TYPES }, T_> action) {{"); {
					code.AppendLine($@"_ = action.Invoke({ ITEMS });");
					code.AppendLine($@"return @this;");
				}
				code.AppendLine($@"}}");
			}
		}
	}
}
