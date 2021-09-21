using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LinqToYourDoom {
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

		// Core
		// ----

		public int Count => Storage.Count;
		public IEnumerable<TKey> Keys => Storage.Keys;
		public IEnumerable<TValue> Values => Storage.Values;

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

		// Extra
		// ----

		/// <summary>
		/// Retrieves the value associated with the given <paramref name="key"/>,
		/// or throws a <see cref="KeyNotFoundException"/> if the <paramref name="key"/> isn't registered.
		/// </summary>
		public T Get<T>(TKey key) where T : TValue {
			if (TryGet<T>(key, out var value))
				return value;

			else throw new KeyNotFoundException($"The key { key } was not present in the dictionary.");
		}

		public T GetOrDefault<T>(TKey key, T defaultValue) where T : TValue {
			if (TryGet<T>(key, out var existingValue))
				return existingValue;

			else return defaultValue;
		}

		public T GetOrDefault<T>(TKey key, Func<T> defaultValueFactory) where T : TValue {
			if (TryGet<T>(key, out var existingValue))
				return existingValue;

			else return defaultValueFactory.Invoke();
		}

		public T GetOrDefault<T>(TKey key, Func<TKey, T> defaultValueFactory) where T : TValue {
			if (TryGet<T>(key, out var existingValue))
				return existingValue;

			else return defaultValueFactory.Invoke(key);
		}

		public T GetOrDefault<T>(TKey key, Action<T>? defaultValueInit = null) where T : TValue, new() {
			if (TryGet<T>(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new T();

				defaultValueInit?.Invoke(defaultValue);

				return defaultValue;
			}
		}

		public T GetOrDefault<T>(TKey key, Action<TKey, T> defaultValueInit) where T : TValue, new() {
			if (TryGet<T>(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new T();

				defaultValueInit.Invoke(key, defaultValue);

				return defaultValue;
			}
		}

		public T GetOrSet<T>(TKey key, T defaultValue) where T : TValue {
			if (TryGet<T>(key, out var existingValue))
				return existingValue;

			else {
				Set(key, defaultValue);

				return defaultValue;
			}
		}

		public T GetOrSet<T>(TKey key, Func<T> defaultValueFactory) where T : TValue {
			if (TryGet<T>(key, out var existingValue))
				return existingValue;

			else {
				Set(key, defaultValueFactory.Invoke().ToVariable(out var defaultValue));

				return defaultValue;
			}
		}

		public T GetOrSet<T>(TKey key, Func<TKey, T> defaultValueFactory) where T : TValue {
			if (TryGet<T>(key, out var existingValue))
				return existingValue;

			else {
				Set(key, defaultValueFactory.Invoke(key).ToVariable(out var defaultValue));

				return defaultValue;
			}
		}

		public T GetOrSet<T>(TKey key, Action<T>? defaultValueInit = null) where T : TValue, new() {
			if (TryGet<T>(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new T();

				defaultValueInit?.Invoke(defaultValue);

				Set(key, defaultValue);

				return defaultValue;
			}
		}

		public T GetOrSet<T>(TKey key, Action<TKey, T> defaultValueInit) where T : TValue, new() {
			if (TryGet<T>(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new T();

				defaultValueInit.Invoke(key, defaultValue);

				Set(key, defaultValue);

				return defaultValue;
			}
		}

		/// <summary>
		/// Adds a <paramref name="value"/> under the specified <paramref name="key"/> to this <see cref="InnerTypedDictionary{TKey, TValue}"/>,
		/// <b>except if</b> the <paramref name="key"/> is already registered.
		/// </summary>
		public bool TrySet<T>(TKey key, T value) where T : TValue {
			if (TryGet<T>(key, out _))
				return false;

			Set(key, value);
			return true;
		}

		/// <summary>
		/// Adds a <paramref name="value"/> under the specified <paramref name="key"/> to this <see cref="InnerTypedDictionary{TKey, TValue}"/>,
		/// or throws an <see cref="ArgumentException"/> if the <paramref name="key"/> is already registered.
		/// </summary>
		public void Add<T>(TKey key, T value) where T : TValue {
			if (TryGet<T>(key, out _))
				throw new ArgumentException($"An item with the same key { key } has already been added.", nameof(key));

			else Set(key, value);
		}

		/// <summary>
		/// Overrides the value associated with a <paramref name="key"/>,
		/// also disposing any existing value if it implements <see cref="IDisposable"/>.
		/// </summary>
		public void SetAndDispose<T>(TKey key, T value) where T : TValue {
			RemoveAndDispose(key);

			Storage[key] = value;
		}

		public void Remove(TKey key) => Storage.Remove(key);

		/// <summary>
		/// Removes an item from this dictionary,
		/// also disposing it if it implements <see cref="IDisposable"/>.
		/// </summary>
		public void RemoveAndDispose(TKey key) {
			if (Storage.Remove(key, out var removedValue) && removedValue is IDisposable disposableValue)
				disposableValue.Dispose();
		}

		/// <summary>
		/// Removes all elements in this dictionary,
		/// also disposing all items that implement <see cref="IDisposable"/>.
		/// </summary>
		public void ClearAndDispose() {
			foreach (var (_, item) in Storage)
				if (item is IDisposable disposableItem)
					disposableItem.Dispose();

			Storage.Clear();
		}

		/// <summary>
		/// Removes all elements in this dictionary,
		/// also disposing all items that implement <see cref="IDisposable"/>
		/// and that satisfy the <paramref name="predicate"/>.
		/// </summary>
		public void ClearAndDispose(Predicate<TValue> predicate) {
			foreach (var (_, item) in Storage)
				if (item is IDisposable disposableItem && predicate.Invoke(item))
					disposableItem.Dispose();

			Storage.Clear();
		}

		/// <summary>
		/// Removes all elements in this dictionary,
		/// also disposing all items that implement <see cref="IDisposable"/>
		/// and that satisfy the <paramref name="predicate"/>.
		/// </summary>
		public void ClearAndDispose(Predicate<KeyValuePair<TKey, TValue>> predicate) {
			foreach (var pair in Storage)
				if (pair.Value is IDisposable disposableItem && predicate.Invoke(pair))
					disposableItem.Dispose();

			Storage.Clear();
		}

		/// <summary>
		/// Removes all elements in this dictionary,
		/// also disposing all items that implement <see cref="IDisposable"/>
		/// and that satisfy the <paramref name="predicate"/>.
		/// </summary>
		public void ClearAndDispose(Func<TKey, TValue, bool> predicate) {
			foreach (var (key, item) in Storage)
				if (item is IDisposable disposableItem && predicate.Invoke(key, item))
					disposableItem.Dispose();

			Storage.Clear();
		}

		// Enumerable
		// ----

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Storage.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Storage.GetEnumerator();

		// Cloneable
		// ----

		public InnerTypedDictionary<TKey, TValue> ShallowClone() => new(new(Storage));

		// Assignable
		// ----

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
