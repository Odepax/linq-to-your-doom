using System;
using System.Collections.Generic;
using LinqToYourDoom.System.Extensions;

namespace LinqToYourDoom.Symbols.Extensions {
	public static class SymbolDictionaryExtensions {
		/// <summary>
		/// Retrieves the value associated with the given <paramref name="key"/>,
		/// or throws a <see cref="KeyNotFoundException"> if the <paramref name="key"/> isn't registered.
		/// </summary>
		public static T Get<T>(this IReadOnlySymbolDictionary @this, Symbol<T> key) {
			if (@this.TryGet(key, out var value))
				return value;

			else throw new KeyNotFoundException($"The key { key } was not present in the symbol dictionary.");
		}

		/// <summary>
		/// Adds a <paramref name="value"/> under the specified <paramref name="key"/> to <paramref name="this"/> <see cref="ISymbolDictionary"/>,
		/// <b>except if</b> the <paramref name="key"/> is already registered.
		/// </summary>
		public static void SetDefault<T>(this ISymbolDictionary @this, Symbol<T> key, T value) {
			if (!@this.TryGet(key, out _))
				@this.Set(key, value);
		}

		/// <summary>
		/// Adds a <paramref name="value"/> under the specified <paramref name="key"/> to <paramref name="this"/> <see cref="ISymbolDictionary"/>,
		/// or throws an <see cref="ArgumentException"> if the <paramref name="key"/> is already registered.
		/// </summary>
		public static void Add<T>(this ISymbolDictionary @this, Symbol<T> key, T value) {
			if (@this.TryGet(key, out _))
				throw new ArgumentException($"An item with the same key { key } has already been added.", nameof(key));

			else @this.Set(key, value);
		}

		public static T GetOrDefault<T>(this IReadOnlySymbolDictionary @this, Symbol<T> key, T defaultValue) {
			if (@this.TryGet(key, out var existingValue))
				return existingValue;

			else return defaultValue;
		}

		public static T GetOrDefault<T>(this IReadOnlySymbolDictionary @this, Symbol<T> key, Func<T> defaultValueFactory) {
			if (@this.TryGet(key, out var existingValue))
				return existingValue;

			else return defaultValueFactory.Invoke();
		}

		public static T GetOrDefault<T>(this IReadOnlySymbolDictionary @this, Symbol<T> key, Func<Symbol<T>, T> defaultValueFactory) {
			if (@this.TryGet(key, out var existingValue))
				return existingValue;

			else return defaultValueFactory.Invoke(key);
		}

		public static T GetOrDefault<T>(this IReadOnlySymbolDictionary @this, Symbol<T> key, Action<T>? defaultValueInit = null) where T : new() {
			if (@this.TryGet(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new T();

				defaultValueInit?.Invoke(defaultValue);

				return defaultValue;
			}
		}

		public static T GetOrDefault<T>(this IReadOnlySymbolDictionary @this, Symbol<T> key, Action<Symbol<T>, T> defaultValueInit) where T : new() {
			if (@this.TryGet(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new T();

				defaultValueInit.Invoke(key, defaultValue);

				return defaultValue;
			}
		}

		public static T GetOrSet<T>(this ISymbolDictionary @this, Symbol<T> key, T defaultValue) {
			if (@this.TryGet(key, out var existingValue))
				return existingValue;

			else {
				@this.Set(key, defaultValue);

				return defaultValue;
			}
		}

		public static T GetOrSet<T>(this ISymbolDictionary @this, Symbol<T> key, Func<T> defaultValueFactory) {
			if (@this.TryGet(key, out var existingValue))
				return existingValue;

			else {
				@this.Set(key, defaultValueFactory.Invoke().ToVariable(out var defaultValue));

				return defaultValue;
			}
		}

		public static T GetOrSet<T>(this ISymbolDictionary @this, Symbol<T> key, Func<Symbol<T>, T> defaultValueFactory) {
			if (@this.TryGet(key, out var existingValue))
				return existingValue;

			else {
				@this.Set(key, defaultValueFactory.Invoke(key).ToVariable(out var defaultValue));

				return defaultValue;
			}
		}

		public static T GetOrSet<T>(this ISymbolDictionary @this, Symbol<T> key, Action<T>? defaultValueInit = null) where T : new() {
			if (@this.TryGet(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new T();

				defaultValueInit?.Invoke(defaultValue);

				@this.Set(key, defaultValue);

				return defaultValue;
			}
		}

		public static T GetOrSet<T>(this ISymbolDictionary @this, Symbol<T> key, Action<Symbol<T>, T> defaultValueInit) where T : new() {
			if (@this.TryGet(key, out var existingValue))
				return existingValue;

			else {
				var defaultValue = new T();

				defaultValueInit.Invoke(key, defaultValue);

				@this.Set(key, defaultValue);

				return defaultValue;
			}
		}
	}
}
