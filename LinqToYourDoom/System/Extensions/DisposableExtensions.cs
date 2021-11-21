using System;
using System.Collections.Generic;

namespace LinqToYourDoom {
	public static class DisposableExtensions {
		/// <summary>
		/// Disposes all items in <paramref name="this"/> sequence.
		/// </summary>
		///
		/// <remarks>
		/// <see cref="DisposeAll{T}(T)"/> is <b>eager</b>.
		/// For a <b>lazy</b> equivalent, use <c>@this.<see cref="EnumerableExtensions.Each{T}(IEnumerable{T}, Action{T})">Each</see>(it => it.Dispose())</c>.
		/// </remarks>
		///
		/// <returns> <paramref name="this"/>. </returns>
		public static T DisposeAll<T>(this T @this) where T : IEnumerable<IDisposable> {
			foreach (var disposable in @this)
				disposable.Dispose();

			return @this;
		}

		/// <inheritdoc cref="DisposeAll{T}(T)"/>
		public static IEnumerable<T> DisposeAll<T, TDisposable>(this IEnumerable<T> @this, Func<T, TDisposable> selector) where TDisposable : IDisposable {
			foreach (var item in @this)
				selector.Invoke(item).Dispose();

			return @this;
		}

		/// <inheritdoc cref="DisposeAll{T}(T)"/>
		public static ICollection<T> DisposeAll<T, TDisposable>(this ICollection<T> @this, Func<T, TDisposable> selector) where TDisposable : IDisposable {
			foreach (var item in @this)
				selector.Invoke(item).Dispose();

			return @this;
		}

		/// <inheritdoc cref="DisposeAll{T}(T)"/>
		public static IDictionary<TKey, TValue> DisposeAll<TKey, TValue, TDisposable>(this IDictionary<TKey, TValue> @this, Func<KeyValuePair<TKey, TValue>, TDisposable> selector) where TDisposable : IDisposable {
			foreach (var item in @this)
				selector.Invoke(item).Dispose();

			return @this;
		}
	}
}
