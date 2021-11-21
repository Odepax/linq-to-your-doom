using System.Text;

namespace LinqToYourDoom.SourceGenerators.Utilities {
	static class ValueTupleExtensions {
		public static void Generate(StringBuilder code) {
			code.AppendLine(@"using System;");
			code.AppendLine(@"using System.Collections;");
			code.AppendLine(@"using System.Collections.Generic;");
			code.AppendLine(@"using System.Runtime.CompilerServices;");

			code.AppendLine(@"namespace LinqToYourDoom {"); {
				code.AppendLine(@"public static partial class ValueTupleExtensions {"); {
					GenerateTo_(code, 8);

					code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					code.AppendLine($@"static InvalidOperationException NotEnoughItems() => new(""The sequence doesn't contain enough items."");");

					code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					code.AppendLine($@"static InvalidOperationException TooManyItems() => new(""The sequence contains too many items."");");

					code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					code.AppendLine($@"static T CheckForMoreItems<T>(this T @return, IEnumerator enumerator, ArgumentValidation argumentValidation) {{"); {
						code.AppendLine($@"if (argumentValidation == ArgumentValidation.Strict && enumerator.MoveNext())");
						code.AppendLine($@"throw TooManyItems();");
						code.AppendLine($@"else return @return;");
					}
					code.AppendLine($@"}}");

					code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					code.AppendLine($@"static T NewFrom<T>(Action<T> action) where T : new() {{"); {
						code.AppendLine($@"var @new = new T();");
						code.AppendLine($@"action.Invoke(@new);");
						code.AppendLine($@"return @new;");
					}
					code.AppendLine($@"}}");

					code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)]");
					code.AppendLine($@"static T NewFrom<T>(Action<T, int> action, int i) where T : new() {{"); {
						code.AppendLine($@"var @new = new T();");
						code.AppendLine($@"action.Invoke(@new, i);");
						code.AppendLine($@"return @new;");
					}
					code.AppendLine($@"}}");

					GenerateToTuple(code, 8);
					GenerateSelect(code, 8);
				}
				code.AppendLine(@"}");
			}
			code.AppendLine(@"}");
		}

		static void GenerateTo_(StringBuilder code, int N) {
			var TYPES = "T";
			var ITEMS = "@this.Item1";
			var INDEXED_ITEMS = "[startIndex] = @this.Item1";
			var KEYED_ITEMS = "[keySelector.Invoke(@this.Item1)] = @this.Item1";
			var KEYED_I_ITEMS = "[keySelector.Invoke(@this.Item1, 0)] = @this.Item1";

			for (var i = 2; i <= N; ++i) {
				TYPES += ", T";
				ITEMS += ", @this.Item" + i;
				INDEXED_ITEMS += ", [startIndex + " + (i - 1) + "] = @this.Item" + i;
				KEYED_ITEMS += ", [keySelector.Invoke(@this.Item" + i + ")] = @this.Item" + i;
				KEYED_I_ITEMS += ", [keySelector.Invoke(@this.Item" + i + ", " + (i - 1) + ")] = @this.Item" + i;

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static T[] ToArray<T>(this ({ TYPES }) @this) => new[] {{ { ITEMS } }};");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static List<T> ToList<T>(this ({ TYPES }) @this) => new() {{ { ITEMS } }};");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static Dictionary<int, T> ToDictionary<T>(this ({ TYPES }) @this, int startIndex = 0) => new() {{ { INDEXED_ITEMS } }};");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static Dictionary<TKey, T> ToDictionary<TKey, T>(this ({ TYPES }) @this, Func<T, TKey> keySelector) => new() {{ { KEYED_ITEMS } }};");
				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static Dictionary<TKey, T> ToDictionary<TKey, T>(this ({ TYPES }) @this, Func<T, int, TKey> keySelector) => new() {{ { KEYED_I_ITEMS } }};");
			}
		}

		static void GenerateToTuple(StringBuilder code, int N) {
			var TYPES = "T";
			var ITEMS_A = "enumerator.MoveNext() ? enumerator.Current : throw NotEnoughItems()";
			var ITEMS_B = "enumerator.MoveNext() ? enumerator.Current : defaultValue";
			var ITEMS_C = "enumerator.MoveNext() ? enumerator.Current : defaultFactory.Invoke()";
			var ITEMS_D = "enumerator.MoveNext() ? enumerator.Current : defaultFactory.Invoke(0)";
			var ITEMS_E = "enumerator.MoveNext() ? enumerator.Current : NewFrom(action)";
			var ITEMS_F = "enumerator.MoveNext() ? enumerator.Current : NewFrom(action, 0)";

			for (var i = 2; i <= N; ++i) {
				TYPES += ", T";
				ITEMS_A += ", enumerator.MoveNext() ? enumerator.Current : throw NotEnoughItems()";
				ITEMS_B += ", enumerator.MoveNext() ? enumerator.Current : defaultValue";
				ITEMS_C += ", enumerator.MoveNext() ? enumerator.Current : defaultFactory.Invoke()";
				ITEMS_D += ", enumerator.MoveNext() ? enumerator.Current : defaultFactory.Invoke(" + (i - 1) + ")";
				ITEMS_E += ", enumerator.MoveNext() ? enumerator.Current : NewFrom(action)";
				ITEMS_F += ", enumerator.MoveNext() ? enumerator.Current : NewFrom(action, " + (i - 1) + ")";

				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""this""/> contains more items than necessary to feed the tuple");
				code.AppendLine($@"/// and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// the extra items are ignored, otherwise, an <see cref=""InvalidOperationException""/> is thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"///");
				code.AppendLine($@"/// <exception cref=""InvalidOperationException"">");
				code.AppendLine($@"/// When <paramref name=""this""/> doesn't contain enough items to feed the tuple.");
				code.AppendLine($@"/// </exception>");
				code.AppendLine($@"public static ({ TYPES }) ToTuple{ i }<T>(this IEnumerable<T> @this, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"using var enumerator = @this.GetEnumerator();");
					code.AppendLine($@"return ({ ITEMS_A }).CheckForMoreItems(enumerator, argumentValidation);");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""this""/> contains more items than necessary to feed the tuple");
				code.AppendLine($@"/// and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// the extra items are ignored, otherwise, an <see cref=""InvalidOperationException""/> is thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static ({ TYPES }) ToTuple{ i }<T>(this IEnumerable<T> @this, T defaultValue, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"using var enumerator = @this.GetEnumerator();");
					code.AppendLine($@"return ({ ITEMS_B }).CheckForMoreItems(enumerator, argumentValidation);");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""this""/> contains more items than necessary to feed the tuple");
				code.AppendLine($@"/// and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// the extra items are ignored, otherwise, an <see cref=""InvalidOperationException""/> is thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static ({ TYPES }) ToTuple{ i }<T>(this IEnumerable<T> @this, Func<T> defaultFactory, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"using var enumerator = @this.GetEnumerator();");
					code.AppendLine($@"return ({ ITEMS_C }).CheckForMoreItems(enumerator, argumentValidation);");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <param name=""defaultFactory"">");
				code.AppendLine($@"/// The factory is given the 0-based index of the tuple item.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"///");
				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""this""/> contains more items than necessary to feed the tuple");
				code.AppendLine($@"/// and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// the extra items are ignored, otherwise, an <see cref=""InvalidOperationException""/> is thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static ({ TYPES }) ToTuple{ i }<T>(this IEnumerable<T> @this, Func<int, T> defaultFactory, ArgumentValidation argumentValidation = default) {{"); {
					code.AppendLine($@"using var enumerator = @this.GetEnumerator();");
					code.AppendLine($@"return ({ ITEMS_D }).CheckForMoreItems(enumerator, argumentValidation);");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""this""/> contains more items than necessary to feed the tuple");
				code.AppendLine($@"/// and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// the extra items are ignored, otherwise, an <see cref=""InvalidOperationException""/> is thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static ({ TYPES }) ToTuple{ i }<T>(this IEnumerable<T> @this, Action<T> action, ArgumentValidation argumentValidation = default) where T : new() {{"); {
					code.AppendLine($@"using var enumerator = @this.GetEnumerator();");
					code.AppendLine($@"return ({ ITEMS_E }).CheckForMoreItems(enumerator, argumentValidation);");
				}
				code.AppendLine($@"}}");

				code.AppendLine($@"/// <param name=""action"">");
				code.AppendLine($@"/// The action is given the 0-based index of the tuple item.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"///");
				code.AppendLine($@"/// <param name=""argumentValidation"">");
				code.AppendLine($@"/// When <paramref name=""this""/> contains more items than necessary to feed the tuple");
				code.AppendLine($@"/// and <paramref name=""argumentValidation""/> is <see cref=""ArgumentValidation.Lenient""/>,");
				code.AppendLine($@"/// the extra items are ignored, otherwise, an <see cref=""InvalidOperationException""/> is thrown.");
				code.AppendLine($@"/// </param>");
				code.AppendLine($@"public static ({ TYPES }) ToTuple{ i }<T>(this IEnumerable<T> @this, Action<T, int> action, ArgumentValidation argumentValidation = default) where T : new() {{"); {
					code.AppendLine($@"using var enumerator = @this.GetEnumerator();");
					code.AppendLine($@"return ({ ITEMS_F }).CheckForMoreItems(enumerator, argumentValidation);");
				}
				code.AppendLine($@"}}");
			}
		}

		static void GenerateSelect(StringBuilder code, int N) {
			var IN_TYPES = "TIn";
			var OUT_TYPES = "TOut";
			var ITEMS = "selector.Invoke(@this.Item1)";

			for (var i = 2; i <= N; ++i) {
				IN_TYPES += ", TIn";
				OUT_TYPES += ", TOut";
				ITEMS += ", selector.Invoke(@this.Item" + i + ')';

				code.AppendLine($@"[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ({ OUT_TYPES }) Select<TIn, TOut>(this ({ IN_TYPES }) @this, Func<TIn, TOut> selector) => ({ ITEMS });");
			}
		}

	}
}
