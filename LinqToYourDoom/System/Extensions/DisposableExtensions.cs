using System;
using System.Collections.Generic;

namespace LinqToYourDoom;

public static class DisposableExtensions {
	// IEnumerable<T> where T : IDisposable
	// ----

	/// <summary>
	/// Disposes all items in <paramref name="this"/> sequence.
	/// </summary>
	///
	/// <remarks>
	/// <see cref="DisposeAll{T}(IEnumerable{T})"/> is <b>eager</b>.
	/// For a <b>lazy</b> equivalent, use <c>@this.<see cref="EnumerableExtensions.Each{T}(IEnumerable{T}, Action{T})">Each</see>(it => it.Dispose())</c>.
	/// </remarks>
	public static void DisposeAll<T>(this IEnumerable<T> @this) where T : IDisposable {
		foreach (var disposable in @this)
			disposable.Dispose();
	}

	/// <summary>
	/// Disposes all items in <paramref name="this"/> sequence that satisfy the <paramref name="predicate"/>.
	/// </summary>
	///
	/// <inheritdoc cref="DisposeAll{T}(IEnumerable{T})"/>
	public static void DisposeAll<T>(this IEnumerable<T> @this, Predicate<T> predicate) where T : IDisposable {
		foreach (var disposable in @this)
			if (predicate.Invoke(disposable))
				disposable.Dispose();
	}

	// IReadOnlyDictionary<TKey, TValue> where TValue : IDisposable
	// ----

	/// <summary>
	/// Disposes all values in <paramref name="this"/> dictionary.
	/// </summary>
	///
	/// <inheritdoc cref="DisposeAll{T}(IEnumerable{T})"/>
	public static void DisposeAll<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this) where TValue : IDisposable {
		foreach (var (_, disposable) in @this)
			disposable.Dispose();
	}

	/// <summary>
	/// Disposes all values in <paramref name="this"/> dictionary that satisfy the <paramref name="predicate"/>.
	/// </summary>
	///
	/// <inheritdoc cref="DisposeAll{TKey, TValue}(IReadOnlyDictionary{TKey, TValue})"/>
	public static void DisposeAll<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, Predicate<TValue> predicate) where TValue : IDisposable {
		foreach (var (_, disposable) in @this)
			if (predicate.Invoke(disposable))
				disposable.Dispose();
	}
	
	/// <inheritdoc cref="DisposeAll{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, Predicate{TValue})"/>
	public static void DisposeAll<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, Func<TKey, TValue, bool> predicate) where TValue : IDisposable {
		foreach (var (key, disposable) in @this)
			if (predicate.Invoke(key, disposable))
				disposable.Dispose();
	}

	// IEnumerable<T>
	// ----

	/// <summary>
	/// Disposes all items that implement <see cref="IDisposable"/> in <paramref name="this"/> sequence.
	/// </summary>
	///
	/// <inheritdoc cref="DisposeAll{T}(IEnumerable{T})"/>
	public static void TryDisposeAll<T>(this IEnumerable<T> @this) {
		foreach (var item in @this)
			if (item is IDisposable disposable)
				disposable.Dispose();
	}

	/// <summary>
	/// Disposes all disposables returned by the <paramref name="selector"/> from the items in <paramref name="this"/> sequence.
	/// </summary>
	///
	/// <inheritdoc cref="DisposeAll{T}(IEnumerable{T})"/>
	public static void DisposeAll<T>(this IEnumerable<T> @this, Func<T, IDisposable> selector) {
		foreach (var item in @this)
			selector.Invoke(item).Dispose();
	}

	/// <summary>
	/// Disposes all disposables returned by the <paramref name="trySelector"/> from the items in <paramref name="this"/> sequence.
	/// </summary>
	///
	/// <inheritdoc cref="DisposeAll{T}(IEnumerable{T}, Func{T, IDisposable})"/>
	public static void DisposeAll<T>(this IEnumerable<T> @this, TryFunc<T, IDisposable> trySelector) {
		foreach (var item in @this)
			if (trySelector.Invoke(item, out var disposable))
				disposable.Dispose();
	}

	/// <inheritdoc cref="DisposeAll{T}(IEnumerable{T}, TryFunc{T, IDisposable})"/>
	public static void DisposeAll<T>(this IEnumerable<T> @this, Func<T, (bool, IDisposable)> trySelector) {
		foreach (var item in @this) {
			var (isDisposable, disposable) = trySelector.Invoke(item);

			if (isDisposable) disposable.Dispose();
		}
	}


	// IReadOnlyDictionary<TKey, TValue>
	// ----

	/// <summary>
	/// Disposes all values that implement <see cref="IDisposable"/> in <paramref name="this"/> dictionary.
	/// </summary>
	///
	/// <inheritdoc cref="DisposeAll{T}(IEnumerable{T})"/>
	public static void TryDisposeAll<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this) {
		foreach (var (_, value) in @this)
			if (value is IDisposable disposable)
				disposable.Dispose();
	}

	/// <summary>
	/// Disposes all disposables returned by the <paramref name="selector"/> from the values in <paramref name="this"/> dictionary.
	/// </summary>
	///
	/// <inheritdoc cref="DisposeAll{T}(IEnumerable{T})"/>
	public static void DisposeAll<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, Func<TKey, TValue, IDisposable> selector) {
		foreach (var (key, value) in @this)
			selector.Invoke(key, value).Dispose();
	}

	/// <summary>
	/// Disposes all disposables returned by the <paramref name="trySelector"/> from the values in <paramref name="this"/> dictionary.
	/// </summary>
	///
	/// <inheritdoc cref="DisposeAll{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, Func{TKey, TValue, IDisposable})"/>
	public static void DisposeAll<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, TryFunc<TKey, TValue, IDisposable> trySelector) {
		foreach (var (key, value) in @this)
			if (trySelector.Invoke(key, value, out var disposable))
				disposable.Dispose();
	}

	/// <inheritdoc cref="DisposeAll{TKey, TValue}(IReadOnlyDictionary{TKey, TValue}, TryFunc{TKey, TValue, IDisposable})"/>
	public static void DisposeAll<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, Func<TKey, TValue, (bool, IDisposable)> trySelector) {
		foreach (var (key, value) in @this) {
			var (isDisposable, disposable) = trySelector.Invoke(key, value);

			if (isDisposable) disposable.Dispose();
		}
	}

	// ICollection<T> where T : IDisposable
	// ----

	/// <summary>
	/// Removes an item from this collection, also disposing the item.
	/// </summary>
	public static void RemoveAndDispose<T>(this ICollection<T> @this, T item) where T : IDisposable {
		@this.Remove(item);
		item.Dispose();
	}

	// IList<T> where T : IDisposable
	// ----

	/// <summary>
	/// Removes the item at the specified <paramref name="index"/> from this list,
	/// also disposing the item.
	/// </summary>
	public static void RemoveAtAndDispose<T>(this IList<T> @this, int index) where T : IDisposable {
		var disposable = @this[index];

		@this.RemoveAt(index);
		disposable.Dispose();
	}

	/// <summary>
	/// Overrides the value at the specifed <paramref name="index"/>,
	/// also disposing the existing value.
	/// </summary>
	public static void SetAndDispose<T>(this IList<T> @this, int index, T value) where T : IDisposable {
		@this[index].Tee(out var disposable);
		@this[index] = value;

		disposable.Dispose();
	}

	// IDictionary<TKey, TValue> where TValue : IDisposable
	// ----

	/// <summary>
	/// Removes the value for the specified <paramref name="key"/> from this dictionary,
	/// also disposing the value.
	/// </summary>
	public static void RemoveAndDispose<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key) where TValue : IDisposable {
		if (@this.Remove(key, out var disposable))
			disposable.Dispose();
	}

	/// <summary>
	/// Overrides the value associated with a <paramref name="key"/>,
	/// also disposing any existing value.
	/// </summary>
	public static void SetAndDispose<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue value) where TValue : IDisposable {
		if (@this.Remove(key, out var disposable))
			disposable.Dispose();

		@this[key] = value;
	}

	// ICollection<T>
	// ----

	/// <summary>
	/// Removes an item from this collection,
	/// also disposing it if it implements <see cref="IDisposable"/>.
	/// </summary>
	public static void RemoveAndTryDispose<T>(this ICollection<T> @this, T item) {
		@this.Remove(item);
		
		if (item is IDisposable disposable)
			disposable.Dispose();
	}

	// IList<T>
	// ----

	/// <summary>
	/// Removes the item at the specified <paramref name="index"/> from this list,
	/// also disposing it if it implements <see cref="IDisposable"/>.
	/// </summary>
	public static void RemoveAtAndTryDispose<T>(this IList<T> @this, int index) {
		var item = @this[index];

		@this.RemoveAt(index);

		if (item is IDisposable disposable)
			disposable.Dispose();
	}

	/// <summary>
	/// Overrides the value at the specifed <paramref name="index"/>,
	/// also disposing the existing value if it implements <see cref="IDisposable"/>.
	/// </summary>
	public static void SetAndTryDispose<T>(this IList<T> @this, int index, T value) {
		@this[index].Tee(out var item);
		@this[index] = value;

		if (item is IDisposable disposable)
			disposable.Dispose();
	}

	// IDictionary<TKey, TValue>
	// ----

	/// <summary>
	/// Removes the value for the specified <paramref name="key"/> from this dictionary,
	/// also disposing it if it implements <see cref="IDisposable"/>.
	/// </summary>
	public static void RemoveAndTryDispose<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key) {
		if (@this.Remove(key, out var item) && item is IDisposable disposable)
			disposable.Dispose();
	}

	/// <summary>
	/// Overrides the value associated with a <paramref name="key"/>,
	/// also disposing any existing value if it implements <see cref="IDisposable"/>.
	/// </summary>
	public static void SetAndTryDispose<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue value) {
		if (@this.Remove(key, out var item) && item is IDisposable disposable)
			disposable.Dispose();

		@this[key] = value;
	}
}
