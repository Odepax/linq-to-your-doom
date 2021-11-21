using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LinqToYourDoom {
	public static partial class ObjectExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool DoesNotEqual<T>(this T? @this, T? other) =>
			!Equals(@this, other);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsContainedIn<T>(this T @this, IEnumerable<T> source, IEqualityComparer<T>? comparer = default) =>
			source.Contains(@this, comparer);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsContainedIn<T>(this T @this, ICollection<T> source) =>
			source.Contains(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T AddTo<T>(this T @this, ICollection<T> collection) {
			collection.Add(@this);

			return @this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T RemoveFrom<T>(this T @this, ICollection<T> collection) {
			collection.Remove(@this);

			return @this;
		}

		/// <summary>
		/// Returns the declared/<b>build-time</b> type of <paramref name="this"/>,
		/// as opposed to <see cref="object.GetType"/>, which returns the actual/<b>run-time</b> type.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Type GetDeclaredType<T>(this T @this) =>
			typeof(T); // Stolen from https://stackoverflow.com/questions/9033/hidden-features-of-c/1789985#1789985

		/// <summary>
		/// Equivalent of <see cref="Enumerable.Cast{TResult}(IEnumerable)"/> for a single object.
		///
		/// Note that <see cref="Enumerable.Cast{TResult}(IEnumerable)"/> is <b>lazy</b>,
		/// while <see cref="As{T}(object)"/> is <b>eager</b>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T As<T>(this object @this) => (T) @this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static KeyValuePair<TKey, TValue> ToKeyValuePair<TKey, TValue>(this ValueTuple<TKey, TValue> @this) =>
			new(@this.Item1, @this.Item2);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTuple<TKey, TValue> ToTuple<TKey, TValue>(this KeyValuePair<TKey, TValue> @this) =>
			new(@this.Key, @this.Value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Tee<T>(this T @this, out T @out) => @out = @this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TIn Tee<TIn, TOut>(this TIn @this, out TOut @out, Func<TIn, TOut> selector) {
			@out = selector.Invoke(@this);

			return @this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static KeyValuePair<TKey, TValue> Tee<TKey, TValue>(this KeyValuePair<TKey, TValue> @this, out TKey out1, out TValue out2) {
			(out1, out2) = @this;

			return @this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TIn Tee<TIn, TKey, TValue>(this TIn @this, out TKey out1, out TValue out2, Func<TIn, KeyValuePair<TKey, TValue>> selector) {
			(out1, out2) = selector.Invoke(@this);

			return @this;
		}

		/// <summary>
		/// Equivalent of <see cref="Enumerable.Select{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/> for a single object.
		///
		/// Note that <see cref="Enumerable.Select{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/> is <b>lazy</b>,
		/// whereas <see cref="To{TIn, TOut}(TIn, Func{TIn, TOut})"/> is <b>eager</b>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TOut To<TIn, TOut>(this TIn @this, Func<TIn, TOut> selector) => selector.Invoke(@this);

		/// <summary>
		/// Equivalent of <see cref="Enumerable.Select{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/> for a key-value pair.
		///
		/// Note that <see cref="Enumerable.Select{TSource, TResult}(IEnumerable{TSource}, Func{TSource, TResult})"/> is <b>lazy</b>,
		/// whereas <see cref="To{TKey, TValue, TOut}(KeyValuePair{TKey, TValue}, Func{TKey, TValue, TOut})"/> is <b>eager</b>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TOut To<TKey, TValue, TOut>(this KeyValuePair<TKey, TValue> @this, Func<TKey, TValue, TOut> selector) => selector.Invoke(@this.Key, @this.Value);

		/// <summary>
		/// Equivalent of <see cref="List{T}.ForEach(Action{T})"/> for a single object.
		///
		/// Note that both <see cref="List{T}.ForEach(Action{T})"/>
		/// and <see cref="Into{T}(T, Action{T})"/> are <b>eager</b>.
		/// </summary>
		///
		/// <remarks>
		/// This method could have been named <i>Also</i>, like its <see href="https://kotlinlang.org/docs/scope-functions.html#also">Kotlin counterpart</see>,
		/// but <c>Math.random()</c> settled on <i>Into</i>.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Into<T>(this T @this, Action<T> action) {
			action.Invoke(@this);

			return @this;
		}

		/// <summary>
		/// Equivalent of <see cref="Into{T}(T, Action{T})"/>,
		/// but taking a <see cref="Func{T, TResult}"/> and discarding the returned value.
		///
		/// This override discards the returned value instead of blocking the compilation
		/// when <paramref name="action"/> is a method that does not return <see cref="void"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Into<T, T_>(this T @this, Func<T, T_> action) {
			_ = action.Invoke(@this);

			return @this;
		}

		/// <summary>
		/// Equivalent of <see cref="List{T}.ForEach(Action{T})"/> for a key-value pair.
		///
		/// Note that both <see cref="List{T}.ForEach(Action{T})"/>
		/// and <see cref="Into{TKey, TValue}(KeyValuePair{TKey, TValue}, Action{TKey, TValue})"/> are <b>eager</b>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static KeyValuePair<TKey, TValue> Into<TKey, TValue>(this KeyValuePair<TKey, TValue> @this, Action<TKey, TValue> action) {
			action.Invoke(@this.Key, @this.Value);

			return @this;
		}

		/// <summary>
		/// Equivalent of <see cref="Into{TKey, TValue}(KeyValuePair{TKey, TValue}, Action{TKey, TValue})"/>,
		/// but taking a <see cref="Func{T1, T2, TResult}"/> and discarding the returned value.
		///
		/// This override discards the returned value instead of blocking the compilation
		/// when <paramref name="action"/> is a method that does not return <see cref="void"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static KeyValuePair<TKey, TValue> Into<TKey, TValue, T_>(this KeyValuePair<TKey, TValue> @this, Func<TKey, TValue, T_> action) {
			_ = action.Invoke(@this.Key, @this.Value);

			return @this;
		}
	}
}
