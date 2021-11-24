using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LinqToYourDoom;

public static partial class EnumerableExtensions {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> @this) =>
		@this.SelectMany(it => it);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<(int Index, T Value)> SelectIndexed<T>(this IEnumerable<T> @this) =>
		@this.Select((it, i) => (i, it));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<(int Index, T Value)> SelectIndexed<T>(this IEnumerable<T> @this, int startIndex) =>
		@this.Select((it, i) => (startIndex + i, it));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<KeyValuePair<TKey, T>> SelectKeyed<TKey, T>(this IEnumerable<T> @this, Func<T, TKey> keySelector) =>
		@this.Select(it => KeyValuePair.Create(keySelector(it), it));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<KeyValuePair<TKey, T>> SelectKeyed<TKey, T>(this IEnumerable<T> @this, Func<T, int, TKey> keySelector) =>
		@this.Select((it, i) => KeyValuePair.Create(keySelector(it, i), it));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<(T Item, TMapped MappedValue)> SelectMap<T, TMapped>(this IEnumerable<T> @this, Func<T, TMapped> mapSelector) =>
		@this.Select(it => (it, mapSelector(it)));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<(T Item, TMapped MappedValue)> SelectMap<T, TMapped>(this IEnumerable<T> @this, Func<T, int, TMapped> mapSelector) =>
		@this.Select((it, i) => (it, mapSelector(it, i)));

	/// <summary> Alias of <see cref="Enumerable.OfType{TResult}(IEnumerable)"/>. </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> Where<T>(this IEnumerable @this) => @this.OfType<T>();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> @this) =>
		@this.Where(it => it is not null)!;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<string> WhereNotNullNorEmpty(this IEnumerable<string?> @this) =>
		@this.Where(it => !string.IsNullOrEmpty(it))!;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<string> WhereNotNullNorWhiteSpace(this IEnumerable<string?> @this) =>
		@this.Where(it => !string.IsNullOrWhiteSpace(it))!;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> Order<T>(this IEnumerable<T> @this, IComparer<T>? comparer = default) =>
		@this.OrderBy(it => it, comparer);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> OrderDescending<T>(this IEnumerable<T> @this, IComparer<T>? comparer = default) =>
		@this.OrderByDescending(it => it, comparer);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsEmpty<T>(this IEnumerable<T> @this) => !@this.Any();
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsNotEmpty<T>(this IEnumerable<T> @this) => @this.Any();

	public static IEnumerable<T> DefaultIfEmpty<T>(this IEnumerable<T> @this, IEnumerable<T> fallback) {
		using var enumerator = @this.GetEnumerator();

		if (enumerator.MoveNext())
			do yield return enumerator.Current;
			while (enumerator.MoveNext());

		else foreach (var item in fallback)
				yield return item;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static IEnumerable<T> SelectDefault<T>(this IEnumerable<T?> @this, T defaultValue) => @this.Select(it => it ?? defaultValue);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static IEnumerable<T> SelectDefault<T>(this IEnumerable<T?> @this, Func<T> valueFactory) => @this.Select(it => it ?? valueFactory.Invoke());
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static IEnumerable<T> SelectDefault<T>(this IEnumerable<T?> @this, Func<int, T> valueFactory) => @this.Select((it, i) => it ?? valueFactory.Invoke(i));

	/// <remarks> I wouldn't use this one on <see langword="struct"/>s... </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> SelectDefault<T>(this IEnumerable<T?> @this, Action<T> action) where T : new() => @this.Select(it => {
		if (it is null) {
			var defaultValue = new T();

			action.Invoke(defaultValue);

			return defaultValue;
		}

		else return it;
	});

	/// <remarks> I wouldn't use this one on <see langword="struct"/>s... </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> SelectDefault<T>(this IEnumerable<T?> @this, Action<T, int> action) where T : new() => @this.Select((it, i) => {
		if (it is null) {
			var defaultValue = new T();

			action.Invoke(defaultValue, i);

			return defaultValue;
		}

		else return it;
	});

	public static IEnumerable<T> IntersectBy<T, TCompared>(this IEnumerable<T> @this, IEnumerable<T> other, Func<T, TCompared> selector, IEqualityComparer<TCompared>? comparer = default) {
		var set = new HashSet<TCompared>(comparer);

		foreach (var item in other)
			set.Add(selector.Invoke(item));

		foreach (var item in @this)
			if (set.Add(selector.Invoke(item)))
				yield return item;
	}

	public static bool HasDuplicates<T>(this IEnumerable<T> @this, IEqualityComparer<T>? comparer = default) {
		var set = new HashSet<T>(comparer);

		foreach (var item in @this)
			if (!set.Add(item))
				return true;

		return false;
	}

	public static bool HasDuplicatesBy<T, TCompared>(this IEnumerable<T> @this, Func<T, TCompared> selector, IEqualityComparer<TCompared>? comparer = default) {
		var set = new HashSet<TCompared>(comparer);

		foreach (var item in @this)
			if (!set.Add(selector.Invoke(item)))
				return true;

		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool HasNoDuplicate<T>(this IEnumerable<T> @this, IEqualityComparer<T>? comparer = default) => !@this.HasDuplicates(comparer);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool HasNoDuplicateBy<TIn, TOut>(this IEnumerable<TIn> @this, Func<TIn, TOut> selector, IEqualityComparer<TOut>? comparer = default) => !@this.HasDuplicatesBy(selector, comparer);

	/// <summary>
	/// Returns a specified number of contiguous items from the start of a sequence,
	/// ensuring the sequence doesn't contain more or less items.
	/// </summary>
	///
	/// <param name="argumentValidation">
	/// When <paramref name="count"/> is negative and <paramref name="argumentValidation"/> is <see cref="ArgumentValidation.Lenient"/>,
	/// <paramref name="count"/> will be silently considered <c>0</c>
	/// otherwise, an <see cref="ArgumentOutOfRangeException"/> is thrown.
	/// </param>
	///
	/// <exception cref="InvalidOperationException">
	/// When the <paramref name="this"/> doesn't contain exactly <paramref name="count"/> items.
	/// </exception>
	public static IEnumerable<T> TakeExact<T>(this IEnumerable<T> @this, int count, ArgumentValidation argumentValidation = default) {
		if (count < 0) {
			if (argumentValidation == ArgumentValidation.Lenient)
				count = 0;

			else throw new ArgumentOutOfRangeException(nameof(count), "Cannot take a negative number of items.");
		}

		using var enumerator = @this.GetEnumerator();

		for (; 0 < count; --count) {
			if (enumerator.MoveNext())
				yield return enumerator.Current;

			else throw new InvalidOperationException($"The sequence doesn't contain exactly { count } items.");
		}

		if (enumerator.MoveNext())
			throw new InvalidOperationException($"The sequence doesn't contain exactly { count } items.");
	}

	/// <summary>
	/// Returns a specified number of contiguous items from the start of a sequence,
	/// ensuring the sequence doesn't contain less items.
	/// </summary>
	///
	/// <param name="argumentValidation"></param>
	/// When <paramref name="count"/> is negative and <paramref name="argumentValidation"/> is <see cref="ArgumentValidation.Lenient"/>,
	/// <paramref name="count"/> will be silently considered <c>0</c>
	/// otherwise, an <see cref="ArgumentOutOfRangeException"/> is thrown.
	///
	/// <exception cref="InvalidOperationException">
	/// When the <paramref name="this"/> contains less than <paramref name="count"/> items.
	/// </exception>
	public static IEnumerable<T> TakeAtLeast<T>(this IEnumerable<T> @this, int count, ArgumentValidation argumentValidation = default) {
		if (count < 0) {
			if (argumentValidation == ArgumentValidation.Lenient)
				count = 0;

			else throw new ArgumentOutOfRangeException(nameof(count), "Cannot take a negative number of items.");
		}

		using var enumerator = @this.GetEnumerator();

		for (; 0 < count; --count) {
			if (enumerator.MoveNext())
				yield return enumerator.Current;

			else throw new InvalidOperationException($"The sequence doesn't contain at least { count } items.");
		}
	}

	public static T Single<T>(this IEnumerable<T> @this, Exception exception) {
		using var enumerator = @this.GetEnumerator();

		if (enumerator.MoveNext()) {
			var single = enumerator.Current;

			if (!enumerator.MoveNext())
				return single;
		}

		throw exception;
	}

	public static T Single<T>(this IEnumerable<T> @this, Func<Exception> exceptionFactory) {
		using var enumerator = @this.GetEnumerator();

		if (enumerator.MoveNext()) {
			var single = enumerator.Current;

			if (!enumerator.MoveNext())
				return single;
		}

		throw exceptionFactory.Invoke();
	}

	public static T First<T>(this IEnumerable<T> @this, Exception exception) {
		using var enumerator = @this.GetEnumerator();

		if (enumerator.MoveNext())
			return enumerator.Current;

		else throw exception;
	}

	public static T First<T>(this IEnumerable<T> @this, Func<Exception> exceptionFactory) {
		using var enumerator = @this.GetEnumerator();

		if (enumerator.MoveNext())
			return enumerator.Current;

		else throw exceptionFactory.Invoke();
	}

	public static T Last<T>(this IEnumerable<T> @this, Exception exception) {
		using var enumerator = @this.GetEnumerator();

		T last;
		if (enumerator.MoveNext()) {
			do last = enumerator.Current;
			while (enumerator.MoveNext());

			return last;
		}

		else throw exception;
	}

	public static T Last<T>(this IEnumerable<T> @this, Func<Exception> exceptionFactory) {
		using var enumerator = @this.GetEnumerator();

		T last;
		if (enumerator.MoveNext()) {
			do last = enumerator.Current;
			while (enumerator.MoveNext());

			return last;
		}

		else throw exceptionFactory.Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this) where TKey : notnull =>
		@this.ToDictionary(it => it.Key, it => it.Value);
		
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey, TValue)> @this) where TKey : notnull =>
		@this.ToDictionary(it => it.Item1, it => it.Item2);
		
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Dictionary<TKey, IEnumerable<TValue>> ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> @this) where TKey : notnull =>
		@this.ToDictionary(it => it.Key, it => it.AsEnumerable());

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> AddRangeTo<T>(this IEnumerable<T> @this, List<T> collection) {
		collection.AddRange(@this);

		return @this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> AddRangeTo<T>(this IEnumerable<T> @this, ICollection<T> collection) {
		foreach (var item in @this)
			collection.Add(item);

		return @this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> RemoveRangeFrom<T>(this IEnumerable<T> @this, ICollection<T> collection) {
		foreach (var item in @this)
			collection.Remove(item);

		return @this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> SetTo<T>(this IEnumerable<T> @this, List<T> collection) {
		collection.Clear();
		collection.AddRange(@this);

		return @this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> SetTo<T>(this IEnumerable<T> @this, ICollection<T> collection) {
		collection.Clear();

		foreach (var item in @this)
			collection.Add(item);

		return @this;
	}

	[return: NotNullIfNotNull("defaultValue")]
	public static T? MinBy<T, TCompared>(this IEnumerable<T> @this, Func<T, TCompared> selector, T? defaultValue, IComparer<TCompared>? comparer = default) {
		using var enumerator = @this.GetEnumerator();

		if (enumerator.MoveNext()) {
			comparer ??= Comparer<TCompared>.Default;

			var minItem = enumerator.Current;
			var minCompared = selector.Invoke(enumerator.Current);

			while (enumerator.MoveNext()) {
				var currentValue = selector.Invoke(enumerator.Current);

				if (comparer.Compare(currentValue, minCompared) < 0) {
					minCompared = currentValue;
					minItem = enumerator.Current;
				}
			}

			return minItem;
		}

		else return defaultValue;
	}

	[return: NotNullIfNotNull("defaultValue")]
	public static T? MaxBy<T, TCompared>(this IEnumerable<T> @this, Func<T, TCompared> selector, T? defaultValue, IComparer<TCompared>? comparer = default) {
		using var enumerator = @this.GetEnumerator();

		if (enumerator.MoveNext()) {
			comparer ??= Comparer<TCompared>.Default;

			var maxItem = enumerator.Current;
			var maxCompared = selector.Invoke(enumerator.Current);

			while (enumerator.MoveNext()) {
				var currentValue = selector.Invoke(enumerator.Current);

				if (comparer.Compare(maxCompared, currentValue) < 0) {
					maxCompared = currentValue;
					maxItem = enumerator.Current;
				}
			}

			return maxItem;
		}

		else return defaultValue;
	}

	/// <summary>
	/// Builds a sequence of items by crawling up a recursive hierarchy.
	/// <paramref name="this"/> is included as the first item of the returned sequence.
	/// The sequence is empty if <paramref name="this"/> is <see langword="null"/>.
	/// </summary>
	///
	/// <remarks>
	/// <para>
	/// To implement depth limit, use <c>.Take(maxDepth + 1)</c>.
	/// To implement a predicate-based stop, use <c>.TakeWhile(continuePredicate)</c>.
	/// Alternatively, make <paramref name="selector"/> return <see langword="null"/> on purpose.
	///
	/// The depth of <paramref name="this"/> is <c>0</c>.
	/// </para>
	///
	/// <para>
	/// Use <see cref="SelectIndexed{T}(IEnumerable{T})"/> to get the depth of each element of the sequence.
	/// </para>
	/// </remarks>
	///
	/// <param name="selector">
	/// A delegate that, given the "current" item, returns the next recursive item of the sequence,
	/// or <see langword="null"/> to stop the recursion.
	/// </param>
	public static IEnumerable<T> Recurse<T>(this T? @this, Func<T, T?> selector) {
		for (; @this is not null; @this = selector.Invoke(@this))
			yield return @this;
	}

	/// <inheritdoc cref="Recurse"/>
	public static IEnumerable<T> Recurse<T>(this T? @this, Func<T, int, T?> selector) {
		for (var depth = 0; @this is not null; @this = selector.Invoke(@this, depth), ++depth)
			yield return @this;
	}

	/// <summary>
	/// Builds a sequence of items by crawling down a recursive hierarchy.
	/// <paramref name="this"/> is included as the first item of the returned sequence.
	/// The sequence is empty if <paramref name="this"/> is <see langword="null"/>.
	/// </summary>
	///
	/// <remarks>
	/// <para>
	/// To implement depth limit or recursive filtering,
	/// you have to make <paramref name="selector"/> return <see langword="null"/> on purpose.
	///
	/// The depth of <paramref name="this"/> is <c>0</c>.
	/// </para>
	///
	/// <para>
	/// Filtering with <c>.Where(predicate)</c> might recursion for nothing, and result in performance loss.
	/// </para>
	/// </remarks>
	///
	/// <param name="selector">
	/// A delegate that, given the "current" item, returns the next recursive items of the sequence,
	/// or an empty sequence to stop the recursion.
	/// </param>
	public static IEnumerable<T> RecurseMany<T>(this T? @this, Func<T, IEnumerable<T>> selector) {
		if (@this is not null) {
			yield return @this;

			foreach (var child in selector.Invoke(@this).SelectRecurse(selector))
				yield return child;
		}
	}

	/// <inheritdoc cref="RecurseMany"/>
	public static IEnumerable<T> RecurseMany<T>(this T? @this, Func<T, int, IEnumerable<T>> selector) {
		if (@this is not null) {
			yield return @this;

			foreach (var child in selector.Invoke(@this, 0).SelectRecurse(selector, startDepth: 1))
				yield return child;
		}
	}

	/// <inheritdoc cref="RecurseMany"/>
	public static IEnumerable<(int Depth, T Item)> RecurseManyDepthed<T>(this T? @this, Func<T, IEnumerable<T>> selector) {
		if (@this is not null) {
			yield return (0, @this);

			foreach (var child in selector.Invoke(@this).SelectRecurseDepthed(selector, startDepth: 1))
				yield return child;
		}
	}

	/// <inheritdoc cref="RecurseMany"/>
	public static IEnumerable<(int Depth, T Item)> RecurseManyDepthed<T>(this T? @this, Func<T, int, IEnumerable<T>> selector) {
		if (@this is not null) {
			yield return (0, @this);

			foreach (var child in selector.Invoke(@this, 0).SelectRecurseDepthed(selector, startDepth: 1))
				yield return child;
		}
	}

	/// <summary>
	/// Builds a sequence of items by crawling down recursive hierarchies.
	/// </summary>
	///
	/// <remarks>
	/// <para>
	/// To implement depth limit or recursive filtering,
	/// you have to make <paramref name="selector"/> return <see langword="null"/> on purpose.
	///
	/// The depth of <paramref name="this"/> is <c>0</c>.
	/// </para>
	///
	/// <para>
	/// Filtering with <c>.Where(predicate)</c> might recursion for nothing, and result in performance loss.
	/// </para>
	/// </remarks>
	///
	/// <param name="selector">
	/// A delegate that, given the "current" item, returns the next recursive items of the sequence,
	/// or an empty sequence to stop the recursion.
	/// </param>
	public static IEnumerable<T> SelectRecurse<T>(this IEnumerable<T> @this, Func<T, IEnumerable<T>> selector) {
		foreach (var item in @this) {
			yield return item;

			foreach (var child in selector.Invoke(item).SelectRecurse(selector))
				yield return child;
		}
	}

	/// <inheritdoc cref="SelectRecurse"/>
	public static IEnumerable<T> SelectRecurse<T>(this IEnumerable<T> @this, Func<T, int, IEnumerable<T>> selector, int startDepth = 0) {
		foreach (var item in @this) {
			yield return item;

			foreach (var child in selector.Invoke(item, startDepth).SelectRecurse(selector, startDepth + 1))
				yield return child;
		}
	}

	/// <inheritdoc cref="SelectRecurse"/>
	public static IEnumerable<(int Depth, T Item)> SelectRecurseDepthed<T>(this IEnumerable<T> @this, Func<T, IEnumerable<T>> selector, int startDepth = 0) {
		foreach (var item in @this) {
			yield return (startDepth, item);

			foreach (var child in selector.Invoke(item).SelectRecurseDepthed(selector, startDepth + 1))
				yield return child;
		}
	}

	/// <inheritdoc cref="SelectRecurse"/>
	public static IEnumerable<(int Depth, T Item)> SelectRecurseDepthed<T>(this IEnumerable<T> @this, Func<T, int, IEnumerable<T>> selector, int startDepth = 0) {
		foreach (var item in @this) {
			yield return (startDepth, item);

			foreach (var child in selector.Invoke(item, startDepth).SelectRecurseDepthed(selector, startDepth + 1))
				yield return child;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static StringBuilder JoinToStringBuilder(this IEnumerable<string> @this, string separator = "", string prefix = "", string suffix = "", string fallback = "") =>
		@this.JoinToStringBuilder(new StringBuilder(), separator, prefix, suffix, fallback);

	public static StringBuilder JoinToStringBuilder(this IEnumerable<string> @this, StringBuilder output, string separator = "", string prefix = "", string suffix = "", string fallback = "") {
		using var enumerator = @this.GetEnumerator();

		if (enumerator.MoveNext()) {
			output.Append(prefix).Append(enumerator.Current);

			while (enumerator.MoveNext())
				output.Append(separator).Append(enumerator.Current);

			output.Append(suffix);
		}

		else output.Append(fallback);

		return output;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static StringBuilder JoinToStringBuilder<T>(this IEnumerable<T> @this, Action<T, int, StringBuilder> writer, string separator = "", string prefix = "", string suffix = "", string fallback = "") =>
		@this.JoinToStringBuilder(new StringBuilder(), writer, separator, prefix, suffix, fallback);

	public static StringBuilder JoinToStringBuilder<T>(this IEnumerable<T> @this, StringBuilder output, Action<T, int, StringBuilder> writer, string separator = "", string prefix = "", string suffix = "", string fallback = "") {
		using var enumerator = @this.GetEnumerator();

		if (enumerator.MoveNext()) {
			output.Append(prefix);

			var i = 0;

			writer.Invoke(enumerator.Current, i, output);

			while (enumerator.MoveNext()) {
				output.Append(separator);
				writer.Invoke(enumerator.Current, ++i, output);
			}

			output.Append(suffix);
		}

		else output.Append(fallback);

		return output;
	}

	public static string JoinToString(this IEnumerable<string> @this, string separator = "", string prefix = "", string suffix = "", string fallback = "") {
		using var enumerator = @this.GetEnumerator();

		if (enumerator.MoveNext()) {
			var output = new StringBuilder(prefix).Append(enumerator.Current);

			while (enumerator.MoveNext())
				output.Append(separator).Append(enumerator.Current);

			return output.Append(suffix).ToString();
		}

		else return fallback;
	}

	/// <summary>
	/// Performs the specified action on each element of this enumerable.
	/// Note that <see cref="Each{T}(IEnumerable{T}, Action{T})"/> is <b>lazy</b>.
	/// </summary>
	public static IEnumerable<T> Each<T>(this IEnumerable<T> @this, Action<T> action) {
		foreach (var item in @this) {
			action.Invoke(item);

			yield return item;
		}
	}

	/// <summary>
	/// Performs the specified action on each element of this enumerable.
	/// Note that <see cref="Each{T}(IEnumerable{T}, Action{T, int})"/> is <b>lazy</b>.
	/// </summary>
	public static IEnumerable<T> Each<T>(this IEnumerable<T> @this, Action<T, int> action) {
		int i = -1;
		foreach (var item in @this) {
			action.Invoke(item, ++i);

			yield return item;
		}
	}

	/// <summary>
	/// Performs the specified action on each element of this enumerable.
	/// Note that <see cref="Each{TKey, TValue}(IEnumerable{KeyValuePair{TKey, TValue}}, Action{TKey, TValue})"/> is <b>lazy</b>.
	/// </summary>
	public static IEnumerable<KeyValuePair<TKey, TValue>> Each<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Action<TKey, TValue> action) {
		foreach (var item in @this) {
			action.Invoke(item.Key, item.Value);

			yield return item;
		}
	}

	/// <summary>
	/// Performs the specified action on each element of this enumerable.
	/// Note that <see cref="Each{TKey, TValue}(IEnumerable{KeyValuePair{TKey, TValue}}, Action{TKey, TValue, int})"/> is <b>lazy</b>.
	/// </summary>
	public static IEnumerable<KeyValuePair<TKey, TValue>> Each<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Action<TKey, TValue, int> action) {
		int i = -1;
		foreach (var item in @this) {
			action.Invoke(item.Key, item.Value, ++i);

			yield return item;
		}
	}

	/// <summary>
	/// Equivalent of <see cref="Each{T}(IEnumerable{T}, Action{T})"/>,
	/// but taking a <see cref="Func{T, TResult}"/> and discarding the returned value.
	///
	/// This override discards the returned value instead of blocking the compilation
	/// when <paramref name="action"/> is a method that does not return <see cref="void"/>.
	/// </summary>
	public static IEnumerable<T> Each<T, T_>(this IEnumerable<T> @this, Func<T, T_> action) {
		foreach (var item in @this) {
			_ = action.Invoke(item);

			yield return item;
		}
	}

	/// <summary>
	/// Equivalent of <see cref="Each{T}(IEnumerable{T}, Action{T, int})"/>,
	/// but taking a <see cref="Func{T1, T2, TResult}"/> and discarding the returned value.
	///
	/// This override discards the returned value instead of blocking the compilation
	/// when <paramref name="action"/> is a method that does not return <see cref="void"/>.
	/// </summary>
	public static IEnumerable<T> Each<T, T_>(this IEnumerable<T> @this, Func<T, int, T_> action) {
		int i = -1;
		foreach (var item in @this) {
			_ = action.Invoke(item, ++i);

			yield return item;
		}
	}

	/// <summary>
	/// Equivalent of <see cref="Each{TKey, TValue}(IEnumerable{KeyValuePair{TKey, TValue}}, Action{TKey, TValue})"/>,
	/// but taking a <see cref="Func{T1, T2, TResult}"/> and discarding the returned value.
	///
	/// This override discards the returned value instead of blocking the compilation
	/// when <paramref name="action"/> is a method that does not return <see cref="void"/>.
	/// </summary>
	public static IEnumerable<KeyValuePair<TKey, TValue>> Each<TKey, TValue, T_>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TKey, TValue, T_> action) {
		foreach (var item in @this) {
			_ = action.Invoke(item.Key, item.Value);

			yield return item;
		}
	}

	/// <summary>
	/// Equivalent of <see cref="Each{TKey, TValue}(IEnumerable{KeyValuePair{TKey, TValue}}, Action{TKey, TValue, int})"/>,
	/// but taking a <see cref="Func{T1, T2, T3, TResult}"/> and discarding the returned value.
	///
	/// This override discards the returned value instead of blocking the compilation
	/// when <paramref name="action"/> is a method that does not return <see cref="void"/>.
	/// </summary>
	public static IEnumerable<KeyValuePair<TKey, TValue>> Each<TKey, TValue, T_>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TKey, TValue, int, T_> action) {
		int i = -1;
		foreach (var item in @this) {
			_ = action.Invoke(item.Key, item.Value, ++i);

			yield return item;
		}
	}

	public static IEnumerable<TOut> Select<TKey, TValue, TOut>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TKey, TValue, TOut> selector) {
		foreach (var item in @this)
			yield return selector.Invoke(item.Key, item.Value);
	}
		
	public static IEnumerable<TOut> Select<TKey, TValue, TOut>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TKey, TValue, int, TOut> selector) {
		var i = -1;
		foreach (var item in @this)
			yield return selector.Invoke(item.Key, item.Value, ++i);
	}

	public static IEnumerable<KeyValuePair<TKey, TValue>> Where<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TKey, TValue, bool> predicate) {
		foreach (var item in @this)
			if (predicate.Invoke(item.Key, item.Value))
				yield return item;
	}

	public static IEnumerable<KeyValuePair<TKey, TValue>> Where<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TKey, TValue, int, bool> predicate) {
		var i = -1;
		foreach (var item in @this)
			if (predicate.Invoke(item.Key, item.Value, ++i))
				yield return item;
	}

	/// <summary>
	/// Tries to project each element of a sequence into a new form,
	/// returning only those elements that were successfully be converted.
	/// </summary>
	///
	/// <returns>
	/// An enumerable containing the outputs of <paramref name="trySelector"/>,
	/// where <paramref name="trySelector"/> returned <see langword="true"/>.
	/// </returns>
	public static IEnumerable<TOut> TrySelect<TIn, TOut>(this IEnumerable<TIn> @this, TryFunc<TIn, TOut> trySelector) {
		foreach (var item in @this)
			if (trySelector.Invoke(item, out var @out))
				yield return @out;
	}

	/// <inheritdoc cref="TrySelect{TIn, TOut}(IEnumerable{TIn}, TryFunc{TIn, TOut})"/>
	public static IEnumerable<TOut> TrySelect<TIn, TOut>(this IEnumerable<TIn> @this, TryFunc<TIn, int, TOut> trySelector) {
		int i = -1;
		foreach (var item in @this)
			if (trySelector.Invoke(item, ++i, out var @out))
				yield return @out;
	}

	/// <inheritdoc cref="TrySelect{TIn, TOut}(IEnumerable{TIn}, TryFunc{TIn, TOut})"/>
	public static IEnumerable<TOut> TrySelect<TKey, TValue, TOut>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, TryFunc<TKey, TValue, TOut> trySelector) {
		foreach (var item in @this)
			if (trySelector.Invoke(item.Key, item.Value, out var @out))
				yield return @out;
	}

	/// <inheritdoc cref="TrySelect{TIn, TOut}(IEnumerable{TIn}, TryFunc{TIn, TOut})"/>
	public static IEnumerable<TOut> TrySelect<TKey, TValue, TOut>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, TryFunc<TKey, TValue, int, TOut> trySelector) {
		int i = -1;
		foreach (var item in @this)
			if (trySelector.Invoke(item.Key, item.Value, ++i, out var @out))
				yield return @out;
	}

	/// <inheritdoc cref="TrySelect{TIn, TOut}(IEnumerable{TIn}, TryFunc{TIn, TOut})"/>
	public static IEnumerable<TOut> TrySelect<TIn, TOut>(this IEnumerable<TIn> @this, Func<TIn, (bool, TOut)> trySelector) {
		foreach (var item in @this) {
			var (@in, @out) = trySelector.Invoke(item);

			if (@in) yield return @out;
		}
	}

	/// <inheritdoc cref="TrySelect{TIn, TOut}(IEnumerable{TIn}, TryFunc{TIn, TOut})"/>
	public static IEnumerable<TOut> TrySelect<TIn, TOut>(this IEnumerable<TIn> @this, Func<TIn, int, (bool, TOut)> trySelector) {
		int i = -1;
		foreach (var item in @this) {
			var (@in, @out) = trySelector.Invoke(item, ++i);

			if (@in) yield return @out;
		}
	}

	/// <inheritdoc cref="TrySelect{TIn, TOut}(IEnumerable{TIn}, TryFunc{TIn, TOut})"/>
	public static IEnumerable<TOut> TrySelect<TKey, TValue, TOut>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TKey, TValue, (bool, TOut)> trySelector) {
		foreach (var item in @this) {
			var (@in, @out) = trySelector.Invoke(item.Key, item.Value);

			if (@in) yield return @out;
		}
	}

	/// <inheritdoc cref="TrySelect{TIn, TOut}(IEnumerable{TIn}, TryFunc{TIn, TOut})"/>
	public static IEnumerable<TOut> TrySelect<TKey, TValue, TOut>(this IEnumerable<KeyValuePair<TKey, TValue>> @this, Func<TKey, TValue, int, (bool, TOut)> trySelector) {
		int i = -1;
		foreach (var item in @this) {
			var (@in, @out) = trySelector.Invoke(item.Key, item.Value, ++i);

			if (@in) yield return @out;
		}
	}

	/// <summary>
	/// Eagerly enumerates <paramref name="this"/> sequence,
	/// effectively invoking the transformations in the pipeline,
	/// like <see langword="foreach"/> would do.
	/// </summary>
	///
	/// <returns> <paramref name="this"/>. </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<T> Enumerate<T>(this IEnumerable<T> @this) {
		foreach (var _ in @this)
			continue;

		return @this;
	}
}
