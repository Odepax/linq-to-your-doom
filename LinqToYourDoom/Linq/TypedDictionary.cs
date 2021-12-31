using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LinqToYourDoom;

/// <summary>
/// <see cref="TypedDictionary{TKey, TValue}"/> is a proxy to a standard dictionary,
/// allowing to retrieve values while also specifying a narrower return type.
/// </summary>
public readonly ref struct TypedDictionary<TKey, TValue> {
	readonly IDictionary<TKey, TValue> Storage;
	public TypedDictionary(IDictionary<TKey, TValue> storage) => Storage = storage;

	public bool TryGet<T>(TKey key, [NotNullWhen(true)] out T? value) where T : TValue {
		if (Storage.TryGetValue(key, out var untypedValue)) {
			value = (T) untypedValue!;
			return true;
		}

		value = default;
		return false;
	}

	public bool TryRemove<T>(TKey key, [NotNullWhen(true)] out T? removedValue) where T : TValue {
		if (Storage.Remove(key, out var untypedValue)) {
			removedValue = (T) untypedValue!;
			return true;
		}

		removedValue = default;
		return false;
	}

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
			Storage[key] = defaultValue;

			return defaultValue;
		}
	}

	public T GetOrSet<T>(TKey key, Func<T> defaultValueFactory) where T : TValue {
		if (TryGet<T>(key, out var existingValue))
			return existingValue;

		else {
			Storage[key] = defaultValueFactory.Invoke().Tee(out var defaultValue);

			return defaultValue;
		}
	}

	public T GetOrSet<T>(TKey key, Func<TKey, T> defaultValueFactory) where T : TValue {
		if (TryGet<T>(key, out var existingValue))
			return existingValue;

		else {
			Storage[key] = defaultValueFactory.Invoke(key).Tee(out var defaultValue);

			return defaultValue;
		}
	}

	public T GetOrSet<T>(TKey key, Action<T>? defaultValueInit = null) where T : TValue, new() {
		if (TryGet<T>(key, out var existingValue))
			return existingValue;

		else {
			var defaultValue = new T();

			defaultValueInit?.Invoke(defaultValue);

			Storage[key] = defaultValue;

			return defaultValue;
		}
	}

	public T GetOrSet<T>(TKey key, Action<TKey, T> defaultValueInit) where T : TValue, new() {
		if (TryGet<T>(key, out var existingValue))
			return existingValue;

		else {
			var defaultValue = new T();

			defaultValueInit.Invoke(key, defaultValue);

			Storage[key] = defaultValue;

			return defaultValue;
		}
	}
}
