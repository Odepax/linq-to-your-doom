using System.Text;

namespace LinqToYourDoom.SourceGenerators.Utilities {
	static class ObjectExtensions {
		public static void Generate(StringBuilder code) {
			code.AppendLine(@"using System;");
			code.AppendLine(@"using System.Collections;");
			code.AppendLine(@"using System.Collections.Generic;");
			code.AppendLine(@"using System.Linq;");
			code.AppendLine(@"using System.Runtime.CompilerServices;");

			code.AppendLine(@"namespace LinqToYourDoom {"); {
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

				// 6452162F-ACE2-4D6B-8D91-BC98B9115762
				//
				// The compiler doesn't seem to understand "ValueTuple{T1, ..., T8}" with 8 generics,
				// and generates a CS1574 warning, complaining that the XML comment has a "cref" attribute
				// that could not be resolved.
				//
				// I think once you understand the 6 other overloads, grasping the 7th should be ok,
				// even without the documentation...
				if (i < 8) {
					code.AppendLine($@"/// <summary>");
					code.AppendLine($@"/// Equivalent of <see cref=""Enumerable.Select{{TSource, TResult}}(IEnumerable{{TSource}}, Func{{TSource, TResult}})""/> for a value tuple.");
					code.AppendLine($@"///");
					code.AppendLine($@"/// Note that <see cref=""Enumerable.Select{{TSource, TResult}}(IEnumerable{{TSource}}, Func{{TSource, TResult}})""/> is <b>lazy</b>,");
					code.AppendLine($@"/// whereas <see cref=""To{{{ TYPES }, TOut}}(ValueTuple{{{ TYPES }}}, Func{{{ TYPES }, TOut}})""/> is <b>eager</b>.");
					code.AppendLine($@"/// </summary>");
				}
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

				// The compiler doesn't understand ValueTuple with 8 generics in the docstring,
				// and generates a CS1574 warning. @see 6452162F-ACE2-4D6B-8D91-BC98B9115762
				if (i < 8) {
					code.AppendLine($@"/// <summary>");
					code.AppendLine($@"/// Equivalent of <see cref=""List{{T}}.ForEach(Action{{T}})""/> for a value tuple.");
					code.AppendLine($@"///");
					code.AppendLine($@"/// Note that both <see cref=""List{{T}}.ForEach(Action{{T}})""/>");
					code.AppendLine($@"/// and <see cref=""Into{{{ TYPES }}}(ValueTuple{{{ TYPES }}}, Action{{{ TYPES }}})""/> are <b>eager</b>.");
					code.AppendLine($@"/// </summary>");
				}
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static ({ TYPES }) Into<{ TYPES }>(this ({ TYPES }) @this, Action<{ TYPES }> action) {{"); {
					code.AppendLine($@"action.Invoke({ ITEMS });");
					code.AppendLine($@"return @this;");
				}
				code.AppendLine($@"}}");

				// <see cref="ValueTuple{...}"/> with 8 generics generates a CS1574 warning.
				// @see 6452162F-ACE2-4D6B-8D91-BC98B9115762
				if (i < 8) {
					code.AppendLine($@"/// <summary>");
					code.AppendLine($@"/// Equivalent of <see cref=""Into{{{ TYPES }}}(ValueTuple{{{ TYPES }}}, Action{{{ TYPES }}})""/>,");
					code.AppendLine($@"/// but taking a <see cref=""Func{{{ TYPES }, TResult}}""/> and discarding the returned value.");
					code.AppendLine($@"///");
					code.AppendLine($@"/// This override discards the returned value instead of blocking the compilation");
					code.AppendLine($@"/// when <paramref name=""action""/> is a method that does not return <see cref=""void""/>.");
					code.AppendLine($@"/// </summary>");
				}
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static ({ TYPES }) Into<{ TYPES }, T_>(this ({ TYPES }) @this, Func<{ TYPES }, T_> action) {{"); {
					code.AppendLine($@"_ = action.Invoke({ ITEMS });");
					code.AppendLine($@"return @this;");
				}
				code.AppendLine($@"}}");
			}
		}
	}
}
