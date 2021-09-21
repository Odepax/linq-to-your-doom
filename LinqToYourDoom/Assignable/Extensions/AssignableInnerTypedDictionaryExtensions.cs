using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LinqToYourDoom {
	public static class AssignableInnerTypedDictionaryExtensions {
		/// <summary>
		/// Assigns values from an<paramref name="other"/> dictionary.
		///
		/// For each key-value-pair, <see cref="Assign{TKey, TValue}(InnerTypedDictionary{TKey, TValue}, IEnumerable{KeyValuePair{TKey, TValue}}, ConflictHandling)"/>
		/// follows <see cref="IAssignable{TIn, TOut}.Assign(TIn, ConflictHandling)"/>'s rules:
		///
		/// <list type="bullet">
		/// <item>
		/// If either the current value or <paramref name="other"/>'s value is <see langword="default"/>,
		/// the result is whatever value is not <see langword="default"/>.
		/// </item>
		///
		/// <item>
		/// If both values are <see langword="default"/>, the result is also <see langword="default"/>.
		/// </item>
		///
		/// <item>
		/// Otherwise the result is assigned by reflection; <see cref="LinqToYourDoom"/> detects
		/// <see cref="IAssignable{TIn, TOut}"/> implementations, <see cref="string"/>s,
		/// simple dictionaries, and will assign other types based on their values.
		///
		/// Assigning this way might throw an <see cref="InvalidOperationException"/>
		/// if <see cref="LinqToYourDoom"/> isn't able to reflect on the assigned entry.
		/// </item>
		/// </list>
		/// </summary>
		///
		/// <returns> <see langword="this"/>. </returns>
		public static InnerTypedDictionary<TKey, TValue> Assign<TKey, TValue>(this InnerTypedDictionary<TKey, TValue> @this, IEnumerable<KeyValuePair<TKey, TValue>> other, ConflictHandling conflictHandling = default) where TKey : notnull =>
			@this.Assign(other, conflictHandling, (_key, thisValue, otherValue, conflictHandling) =>
				AssignFromReflection<TKey, TValue>(thisValue, otherValue, conflictHandling)
			);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static TValue AssignFromReflection<TKey, TValue>(TValue a, TValue b, ConflictHandling conflictHandling) where TKey : notnull {
			if (a is string a_string && b is string b_string)
				return AssignString<TValue>(a_string, b_string, conflictHandling);

			var a_type = a!.GetType();
			var b_type = b!.GetType();

			if (IsAssignable(a_type, b_type))
				return AssignAssignable<TKey, TValue>(a_type, b_type, a, b, conflictHandling);

			if (IsDictionary(a_type, b_type, out var key_type, out var a_value, out var b_value)) {
				if (a_value == typeof(string) && b_value == typeof(string))
					return AssignStringDictionary<TKey, TValue>(key_type, a, b, conflictHandling);

				if (IsAssignable(a_value, b_value))
					return AssignAssignableDictionary<TKey, TValue>(key_type, a_value, a, b, conflictHandling);

				if (b_value.Inherits(a_value))
					return AssignDictionary<TKey, TValue>(key_type, a_value, a, b, conflictHandling);
			}

			return a.Assign(b, conflictHandling, (_, _) => throw new InvalidOperationException($"Value { a } is not mergeable from value { b }."))!;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool IsAssignable(Type a, Type b) =>
			a.Implements(typeof(IAssignable<,>).MakeGenericType(b, a));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool IsDictionary(Type a, Type b,
			[NotNullWhen(true)] out Type? key_type,
			[NotNullWhen(true)] out Type? a_value,
			[NotNullWhen(true)] out Type? b_value
		) {
			if (
				   a.ImplementsGeneric(typeof(IDictionary<,>), out var iDictionary) // A is IDictionary
				&& b.ImplementsGeneric(typeof(IEnumerable<>), out var iEnumerable) // B is IEnumerable
				&& iEnumerable.GenericTypeArguments[0].GetGenericTypeDefinition() == typeof(KeyValuePair<,>) // B<0> is KeyValuePair
				&& iEnumerable.GenericTypeArguments[0].GenericTypeArguments.ToVariable(out var kvp_generics)[0].Inherits(iDictionary.GenericTypeArguments[0]) // B<0><K> is A<K>
			) {
				key_type = iDictionary.GenericTypeArguments[0];
				a_value = iDictionary.GenericTypeArguments[1];
				b_value = kvp_generics[1];

				return true;
			}

			key_type = default;
			a_value = default;
			b_value = default;

			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static TValue AssignString<TValue>(string a, string b, ConflictHandling conflictHandling) =>
			(TValue) (object) a.Assign(b, StringComparison.InvariantCulture, conflictHandling, string.Concat);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static TValue AssignAssignable<TKey, TValue>(Type a_type, Type b_type, TValue a, TValue b, ConflictHandling conflictHandling) where TKey : notnull =>
			(TValue) typeof(AssignableInnerTypedDictionaryExtensions)
				.GetMethod(nameof(AssignAssignable_method), BindingFlags.Static | BindingFlags.NonPublic)!
				.MakeGenericMethod(typeof(TValue), b_type, a_type)
				.Invoke(null, BindingFlags.DoNotWrapExceptions, null, new object?[] { a, b, conflictHandling }, null)!;

		static TOut AssignAssignable_method<TValue, TIn, TOut>(TOut a, TIn b, ConflictHandling conflictHandling)
		where TIn : TValue
		where TOut : TValue, IAssignable<TIn, TOut> =>
			a.Assign(b, conflictHandling);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static TValue AssignAssignableDictionary<TKey, TValue>(Type k_type, Type v_type, TValue a, TValue b, ConflictHandling conflictHandling) where TKey : notnull =>
			(TValue) typeof(AssignableInnerTypedDictionaryExtensions)
				.GetMethod(nameof(AssignAssignableDictionary_method), BindingFlags.Static | BindingFlags.NonPublic)!
				.MakeGenericMethod(k_type, v_type)
				.Invoke(null, BindingFlags.DoNotWrapExceptions, null, new object?[] { a, b, conflictHandling }, null)!;

		static IDictionary<TK, TV> AssignAssignableDictionary_method<TK, TV>(IDictionary<TK, TV> a, IEnumerable<KeyValuePair<TK, TV>> b, ConflictHandling conflictHandling)
		where TV : IAssignable<TV, TV> =>
			a.Assign(b, conflictHandling);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static TValue AssignStringDictionary<TKey, TValue>(Type k_type, TValue a, TValue b, ConflictHandling conflictHandling) where TKey : notnull =>
			(TValue) typeof(AssignableInnerTypedDictionaryExtensions)
				.GetMethod(nameof(AssignStringDictionary_method), BindingFlags.Static | BindingFlags.NonPublic)!
				.MakeGenericMethod(k_type)
				.Invoke(null, BindingFlags.DoNotWrapExceptions, null, new object?[] { a, b, conflictHandling }, null)!;

		static IDictionary<TK, string> AssignStringDictionary_method<TK>(IDictionary<TK, string> a, IEnumerable<KeyValuePair<TK, string>> b, ConflictHandling conflictHandling) =>
			a.Assign(b, StringComparison.InvariantCulture, conflictHandling, string.Concat);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static TValue AssignDictionary<TKey, TValue>(Type k_type, Type v_type, TValue a, TValue b, ConflictHandling conflictHandling) where TKey : notnull =>
			(TValue) typeof(AssignableInnerTypedDictionaryExtensions)
				.GetMethod(nameof(AssignDictionary_method), BindingFlags.Static | BindingFlags.NonPublic)!
				.MakeGenericMethod(k_type, v_type)
				.Invoke(null, BindingFlags.DoNotWrapExceptions, null, new object?[] { a, b, conflictHandling }, null)!;

		static IDictionary<TK, TV> AssignDictionary_method<TK, TV>(IDictionary<TK, TV> a, IEnumerable<KeyValuePair<TK, TV>> b, ConflictHandling conflictHandling) =>
			a.Assign(b, conflictHandling, (_, _) => throw new InvalidOperationException($"Dictionary value { a } is not mergeable from value { b }."));
	}
}
