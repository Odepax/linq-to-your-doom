using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LinqToYourDoom {
	public static class LazyExtensions {
		/// <summary>
		/// Equivalent of <see cref="Enumerable.Cast{TResult}(IEnumerable)"/> for a <see cref="Lazy{T}"/> object.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Lazy<T> As<T>(this Lazy<object> @this, LazyThreadSafetyMode mode = default) =>
			new(() => (T) @this.Value, mode);

		/// <summary>
		/// Equivalent of <see cref="Enumerable.Select{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/> for a <see cref="Lazy{T}"/> object.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Lazy<TOut> To<TIn, TOut>(this Lazy<TIn> @this, Func<TIn, TOut> selector, LazyThreadSafetyMode mode = default) =>
			new(() => selector.Invoke(@this.Value), mode);

		/// <summary>
		/// Equivalent of <see cref="List{T}.ForEach(Action{T})"/> for a <see cref="Lazy{T}"/> object.
		///
		/// Note that <see cref="List{T}.ForEach(Action{T})"/> is <b>eager</b>,
		/// whereas <see cref="Do{T}(Lazy{T}, Action{T}, LazyThreadSafetyMode)"/> is <b>lazy</b>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Lazy<T> Do<T>(this Lazy<T> @this, Action<T> action, LazyThreadSafetyMode mode = default) =>
			new(() => {
				var value = @this.Value;

				action.Invoke(value);

				return value;
			}, mode);
	}
}
