using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LinqToYourDoom {
	public static class DictionaryExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<KeyValuePair<TKey, TValue>> WhereValueNotNull<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue?>> @this) =>
			@this.Where(it => it.Value is not null)!;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<KeyValuePair<TKey, string>> WhereValueNotNullNorEmpty<TKey>(this IEnumerable<KeyValuePair<TKey, string?>> @this) =>
			@this.Where(it => !string.IsNullOrEmpty(it.Value))!;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<KeyValuePair<TKey, string>> WhereValueNotNullNorWhiteSpace<TKey>(this IEnumerable<KeyValuePair<TKey, string?>> @this) =>
			@this.Where(it => !string.IsNullOrWhiteSpace(it.Value))!;

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static IOrderedEnumerable<KeyValuePair<TKey, TValue>> OrderByKey<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, IComparer<TKey>? comparer = default) => @this.OrderBy(kvp => kvp.Key, comparer);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static IOrderedEnumerable<KeyValuePair<TKey, TValue>> OrderByValue<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, IComparer<TValue>? comparer = default) => @this.OrderBy(kvp => kvp.Value, comparer);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static IOrderedEnumerable<KeyValuePair<TKey, TValue>> OrderByKey<TKey, TValue, TSelector>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TKey, TSelector> selector, IComparer<TSelector>? comparer = default) => @this.OrderBy(kvp => selector(kvp.Key), comparer);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static IOrderedEnumerable<KeyValuePair<TKey, TValue>> OrderByValue<TKey, TValue, TSelector>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TValue, TSelector> selector, IComparer<TSelector>? comparer = default) => @this.OrderBy(kvp => selector(kvp.Value), comparer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static IOrderedEnumerable<KeyValuePair<TKey, TValue>> OrderByKeyDescending<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, IComparer<TKey>? comparer = default) => @this.OrderByDescending(kvp => kvp.Key, comparer);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static IOrderedEnumerable<KeyValuePair<TKey, TValue>> OrderByValueDescending<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, IComparer<TValue>? comparer = default) => @this.OrderByDescending(kvp => kvp.Value, comparer);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static IOrderedEnumerable<KeyValuePair<TKey, TValue>> OrderByKeyDescending<TKey, TValue, TSelector>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TKey, TSelector> selector, IComparer<TSelector>? comparer = default) => @this.OrderByDescending(kvp => selector(kvp.Key), comparer);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static IOrderedEnumerable<KeyValuePair<TKey, TValue>> OrderByValueDescending<TKey, TValue, TSelector>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TValue, TSelector> selector, IComparer<TSelector>? comparer = default) => @this.OrderByDescending(kvp => selector(kvp.Value), comparer);

		public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, TKey key, Func<TValue> defaultValueFactory) {
			if (@this.TryGetValue(key, out var existingValue))
				return existingValue;

			else return defaultValueFactory.Invoke();
		}

		public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, TKey key, Func<TKey, TValue> defaultValueFactory) {
			if (@this.TryGetValue(key, out var existingValue))
				return existingValue;

			else return defaultValueFactory.Invoke(key);
		}

		public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, TKey key, Action<TValue>? defaultValueInit = null) where TValue : new() {
			if (@this.TryGetValue(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new TValue();

				defaultValueInit?.Invoke(defaultValue);

				return defaultValue;
			}
		}

		public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, TKey key, Action<TKey, TValue> defaultValueInit) where TValue : new() {
			if (@this.TryGetValue(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new TValue();

				defaultValueInit.Invoke(key, defaultValue);

				return defaultValue;
			}
		}

		public static TValue GetValueOrSet<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue defaultValue) {
			if (@this.TryGetValue(key, out var existingValue))
				return existingValue;

			else return @this[key] = defaultValue;
		}

		public static TValue GetValueOrSet<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TValue> defaultValueFactory) {
			if (@this.TryGetValue(key, out var existingValue))
				return existingValue;

			else return @this[key] = defaultValueFactory.Invoke();
		}

		public static TValue GetValueOrSet<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TKey, TValue> defaultValueFactory) {
			if (@this.TryGetValue(key, out var existingValue))
				return existingValue;

			else return @this[key] = defaultValueFactory.Invoke(key);
		}

		public static TValue GetValueOrSet<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Action<TValue>? defaultValueInit = null) where TValue : new() {
			if (@this.TryGetValue(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new TValue();

				defaultValueInit?.Invoke(defaultValue);

				return @this[key] = defaultValue;
			}
		}

		public static TValue GetValueOrSet<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Action<TKey, TValue> defaultValueInit) where TValue : new() {
			if (@this.TryGetValue(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new TValue();

				defaultValueInit.Invoke(key, defaultValue);

				return @this[key] = defaultValue;
			}
		}
	}
}
