using System.Text;

namespace LinqToYourDoom.SourceGenerators.Utilities {
	static class EnumerableExtensions {
		public static void Generate(StringBuilder code) {

			code.AppendLine(@"using System;");
			code.AppendLine(@"using System.Collections.Generic;");
			code.AppendLine(@"using System.Runtime.CompilerServices;");

			code.AppendLine(@"namespace LinqToYourDoom {"); {
				code.AppendLine(@"public static partial class EnumerableExtensions {"); {
					GenerateAbsMin_AbsMax(code);
					Generate_(code, 8);
				}
				code.AppendLine(@"}");
			}
			code.AppendLine(@"}");
		}

		static void GenerateAbsMin_AbsMax(StringBuilder code) {
			var Operations = new[] {
				("Min", "minimum"),
				("Max", "maximum")
			};

			var Data = new[] {
				"sbyte",
				"short",
				"int",
				"long",
				"decimal",
				"float",
				"double",
			};

			foreach (var (OP, DESC) in Operations)
			foreach (var TYPE in Data) {
				code.AppendLine($@"/// <summary> Compares the absolutes of a sequence's values, and returns the original value corresponding to the { DESC } absolute. </summary>");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
				code.AppendLine($@"public static { TYPE } Abs{ OP }(this IEnumerable<{ TYPE }> @this, { TYPE } defaultValue = default) => @this.{ OP }By(Math.Abs, defaultValue);");
			}
		}

		static void Generate_(StringBuilder code, int N) {
			var TYPES = "T1";
			var ITEMS = "item.Item1";

			for (var i = 2; i <= N; ++i) {
				TYPES += ", T" + i;
				ITEMS += ", item.Item" + i;

				code.AppendLine($@"public static IEnumerable<({ TYPES })> Each<{ TYPES }>(this IEnumerable<({ TYPES })> @this, Action<{ TYPES }> action) {{"); {
					code.AppendLine($@"foreach (var item in @this) {{"); {
						code.AppendLine($@"action.Invoke({ ITEMS });");
						code.AppendLine($@"yield return item;");
					}
					code.AppendLine($@"}}");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"public static IEnumerable<({ TYPES })> Each<{ TYPES }>(this IEnumerable<({ TYPES })> @this, Action<{ TYPES }, int> action) {{"); {
					code.AppendLine($@"var i = -1;");
					code.AppendLine($@"foreach (var item in @this) {{"); {
						code.AppendLine($@"action.Invoke({ ITEMS }, ++i);");
						code.AppendLine($@"yield return item;");
					}
					code.AppendLine($@"}}");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"public static IEnumerable<TOut> Select<{ TYPES }, TOut>(this IEnumerable<({ TYPES })> @this, Func<{ TYPES }, TOut> selector) {{"); {
					code.AppendLine($@"foreach (var item in @this)");
					code.AppendLine($@"yield return selector.Invoke({ ITEMS });");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"public static IEnumerable<TOut> Select<{ TYPES }, TOut>(this IEnumerable<({ TYPES })> @this, Func<{ TYPES }, int, TOut> selector) {{"); {
					code.AppendLine($@"var i = -1;");
					code.AppendLine($@"foreach (var item in @this)");
					code.AppendLine($@"yield return selector.Invoke({ ITEMS }, ++i);");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"public static IEnumerable<({ TYPES })> Where<{ TYPES }>(this IEnumerable<({ TYPES })> @this, Func<{ TYPES }, bool> predicate) {{"); {
					code.AppendLine($@"foreach (var item in @this)");
					code.AppendLine($@"if (predicate.Invoke({ ITEMS }))");
					code.AppendLine($@"yield return item;");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"public static IEnumerable<({ TYPES })> Where<{ TYPES }>(this IEnumerable<({ TYPES })> @this, Func<{ TYPES }, int, bool> predicate) {{"); {
					code.AppendLine($@"var i = -1;");
					code.AppendLine($@"foreach (var item in @this)");
					code.AppendLine($@"if (predicate.Invoke({ ITEMS }, ++i))");
					code.AppendLine($@"yield return item;");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <inheritdoc cref=""TrySelect{{TIn, TOut}}(IEnumerable{{TIn}}, TryFunc{{TIn, TOut}})""/>");
				code.AppendLine($@"public static IEnumerable<TOut> TrySelect<{ TYPES }, TOut>(this IEnumerable<({ TYPES })> @this, TryFunc<{ TYPES }, TOut> trySelector) {{"); {
					code.AppendLine($@"foreach (var item in @this)");
					code.AppendLine($@"if (trySelector.Invoke({ ITEMS }, out var @out))");
					code.AppendLine($@"yield return @out;");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <inheritdoc cref=""TrySelect{{TIn, TOut}}(IEnumerable{{TIn}}, TryFunc{{TIn, TOut}})""/>");
				code.AppendLine($@"public static IEnumerable<TOut> TrySelect<{ TYPES }, TOut>(this IEnumerable<({ TYPES })> @this, TryFunc<{ TYPES }, int, TOut> trySelector) {{"); {
					code.AppendLine($@"int i = -1;");
					code.AppendLine($@"foreach (var item in @this)");
					code.AppendLine($@"if (trySelector.Invoke({ ITEMS }, ++i, out var @out))");
					code.AppendLine($@"yield return @out;");
				}
				code.AppendLine($@"}}");
			}
		}
	}
}
