using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using LinqToYourDoom.Assignable;
using LinqToYourDoom.Assignable.Extensions;
using LinqToYourDoom.System;
using LinqToYourDoom.System.Extensions;

namespace LinqToYourDoom.Symbols {
	/// <summary>
	/// <see cref="InnerTypedDictionary{TKey, TValue}"/> is a storage helper,
	/// refactored from experimental implementations of <see cref="SymbolDictionary"/>.
	/// </summary>
	///
	/// <remarks>
	/// <para>
	/// <b>Always</b> use the <see langword="static"/> <see cref="New"/> factory methods
	/// to instantiate this type, as using <c><see langword="default"/>(<see cref="InnerTypedDictionary{TKey, TValue}"/>)</c>
	/// would not initialize the internal state correctly.
	/// </para>
	///
	/// <para>
	/// Despite being a <see langword="readonly struct"/>,
	/// <see cref="InnerTypedDictionary{TKey, TValue}"/>'s internals are actually mutable.
	/// </para>
	/// </remarks>
	public readonly struct InnerTypedDictionary<TKey, TValue> :
		IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
		IShallowCloneable<InnerTypedDictionary<TKey, TValue>>,
		IAssignable<IEnumerable<KeyValuePair<TKey, TValue>>, InnerTypedDictionary<TKey, TValue>>
	where TKey : notnull {
		public static InnerTypedDictionary<TKey, TValue> New() => new(new());
		public static InnerTypedDictionary<TKey, TValue> New(int capacity) => new(new(capacity));

		readonly Dictionary<TKey, TValue> Storage;
		InnerTypedDictionary(Dictionary<TKey, TValue> storage) => Storage = storage;

		public int Count => Storage.Count;
		public IEnumerable<TKey> Keys => Storage.Keys;

		public bool TryGet<T>(TKey key, [NotNullWhen(true)] out T? value) where T : TValue {
			if (Storage.TryGetValue(key, out var untypedValue)) {
				value = (T) untypedValue!;
				return true;
			}

			value = default;
			return false;
		}

		public void Set<T>(TKey key, T value) where T : TValue => Storage[key] = value;

		public bool TryRemove<T>(TKey key, [NotNullWhen(true)] out T? removedValue) where T : TValue {
			if (Storage.Remove(key, out var untypedValue)) {
				removedValue = (T) untypedValue!;
				return true;
			}

			removedValue = default;
			return false;
		}

		public void Clear() => Storage.Clear();

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Storage.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Storage.GetEnumerator();

		public InnerTypedDictionary<TKey, TValue> ShallowClone() => new(new(Storage));

		public InnerTypedDictionary<TKey, TValue> Assign(IEnumerable<KeyValuePair<TKey, TValue>> other, ConflictHandling conflictHandling = default) {
			foreach (var (key, otherValue) in other)
				try {
					Storage[key] = (
						  !Storage.TryGetValue(key, out var thisValue) || Equals(thisValue, default) ? otherValue
						: Equals(otherValue, default) ? thisValue
						: AssignFromReflection(thisValue, otherValue, conflictHandling)
					);
				}

				catch (AssignConflictException conflict) {
					conflict.PrependPropertyAndIndexer(null, key?.ToString());

					throw;
				}

			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static TValue AssignFromReflection(TValue a, TValue b, ConflictHandling conflictHandling) {
			if (a is string a_string && b is string b_string)
				return AssignString(a_string, b_string, conflictHandling);

			var a_type = a!.GetType();
			var b_type = b!.GetType();

			if (IsAssignable(a_type, b_type))
				return AssignAssignable(a_type, b_type, a, b, conflictHandling);

			if (IsDictionary(a_type, b_type, out var key_type, out var a_value, out var b_value)) {
				if (a_value == typeof(string) && b_value == typeof(string))
					return AssignStringDictionary(key_type, a, b, conflictHandling);

				if (IsAssignable(a_value, b_value))
					return AssignAssignableDictionary(key_type, a_value, a, b, conflictHandling);

				if (b_value.Inherits(a_value))
					return AssignDictionary(key_type, a_value, a, b, conflictHandling);
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
		static TValue AssignString(string a, string b, ConflictHandling conflictHandling) =>
			(TValue) (object) a.Assign(b, StringComparison.InvariantCulture, conflictHandling, string.Concat);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static TValue AssignAssignable(Type a_type, Type b_type, TValue a, TValue b, ConflictHandling conflictHandling) =>
			(TValue) typeof(InnerTypedDictionary<TKey, TValue>)
				.GetMethod(nameof(AssignAssignable_method), BindingFlags.Static | BindingFlags.NonPublic)!
				.MakeGenericMethod(b_type, a_type)
				.Invoke(null, BindingFlags.DoNotWrapExceptions, null, new object?[] { a, b, conflictHandling }, null)!;

		static TOut AssignAssignable_method<TIn, TOut>(TOut a, TIn b, ConflictHandling conflictHandling)
		where TIn : TValue
		where TOut : TValue, IAssignable<TIn, TOut> =>
			a.Assign(b, conflictHandling);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static TValue AssignAssignableDictionary(Type k_type, Type v_type, TValue a, TValue b, ConflictHandling conflictHandling) =>
			(TValue) typeof(InnerTypedDictionary<TKey, TValue>)
				.GetMethod(nameof(AssignAssignableDictionary_method), BindingFlags.Static | BindingFlags.NonPublic)!
				.MakeGenericMethod(k_type, v_type)
				.Invoke(null, BindingFlags.DoNotWrapExceptions, null, new object?[] { a, b, conflictHandling }, null)!;

		static IDictionary<TK, TV> AssignAssignableDictionary_method<TK, TV>(IDictionary<TK, TV> a, IEnumerable<KeyValuePair<TK, TV>> b, ConflictHandling conflictHandling)
		where TV : IAssignable<TV, TV> =>
			a.Assign(b, conflictHandling);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static TValue AssignStringDictionary(Type k_type, TValue a, TValue b, ConflictHandling conflictHandling) =>
			(TValue) typeof(InnerTypedDictionary<TKey, TValue>)
				.GetMethod(nameof(AssignStringDictionary_method), BindingFlags.Static | BindingFlags.NonPublic)!
				.MakeGenericMethod(k_type)
				.Invoke(null, BindingFlags.DoNotWrapExceptions, null, new object?[] { a, b, conflictHandling }, null)!;

		static IDictionary<TK, string> AssignStringDictionary_method<TK>(IDictionary<TK, string> a, IEnumerable<KeyValuePair<TK, string>> b, ConflictHandling conflictHandling) =>
			a.Assign(b, StringComparison.InvariantCulture, conflictHandling, string.Concat);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static TValue AssignDictionary(Type k_type, Type v_type, TValue a, TValue b, ConflictHandling conflictHandling) =>
			(TValue) typeof(InnerTypedDictionary<TKey, TValue>)
				.GetMethod(nameof(AssignDictionary_method), BindingFlags.Static | BindingFlags.NonPublic)!
				.MakeGenericMethod(k_type, v_type)
				.Invoke(null, BindingFlags.DoNotWrapExceptions, null, new object?[] { a, b, conflictHandling }, null)!;

		static IDictionary<TK, TV> AssignDictionary_method<TK, TV>(IDictionary<TK, TV> a, IEnumerable<KeyValuePair<TK, TV>> b, ConflictHandling conflictHandling) =>
			a.Assign(b, conflictHandling, (_, _) => throw new InvalidOperationException($"Dictionary value { a } is not mergeable from value { b }."));
	}
}
