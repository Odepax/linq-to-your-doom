using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
		IShallowCloneable<InnerTypedDictionary<TKey, TValue>>
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
				Set(key, defaultValueFactory.Invoke().Tee(out var defaultValue));

				return defaultValue;
			}
		}

		public T GetOrSet<T>(TKey key, Func<TKey, T> defaultValueFactory) where T : TValue {
			if (TryGet<T>(key, out var existingValue))
				return existingValue;

			else {
				Set(key, defaultValueFactory.Invoke(key).Tee(out var defaultValue));

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

		/// <summary>
		/// Assigns values from an<paramref name="other"/> dictionary.
		///
		/// For each key-value-pair, <see cref="Assign(IEnumerable{KeyValuePair{TKey, TValue}}, ConflictHandling, Func{TKey, TValue, TValue, ConflictHandling, TValue})"/>
		/// follows <see cref="IAssignable{TIn, TOut}.Assign(TIn, ConflictHandling)"/>'s first rules
		/// by privileging non-<see langword="default"/> values,
		/// but otherwise relying on the <paramref name="assign"/> delegate
		/// to route values and perform the assignment heavy work.
		///
		/// If an <see cref="AssignConflictException"/> is thrown,
		/// <see cref="Assign(IEnumerable{KeyValuePair{TKey, TValue}}, ConflictHandling, Func{TKey, TValue, TValue, ConflictHandling, TValue})"/>
		/// will prepend the key to the <see cref="AssignConflictException.Path"/> automatically.
		/// </summary>
		///
		/// <returns> <see langword="this"/>. </returns>
		public InnerTypedDictionary<TKey, TValue> Assign(IEnumerable<KeyValuePair<TKey, TValue>> other, ConflictHandling conflictHandling, Func<TKey, TValue, TValue, ConflictHandling, TValue> assign) {
			foreach (var (key, otherValue) in other)
				try {
					Storage[key] = (
						  !Storage.TryGetValue(key, out var thisValue) || Equals(thisValue, default) ? otherValue
						: Equals(otherValue, default) ? thisValue
						: assign.Invoke(key, thisValue, otherValue, conflictHandling)
					);
				}

				catch (AssignConflictException conflict) {
					conflict.PrependPropertyAndIndexer(null, key.ToString());

					throw;
				}

			return this;
		}
	}
}
