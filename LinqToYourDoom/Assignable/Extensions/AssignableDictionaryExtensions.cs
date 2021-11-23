using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LinqToYourDoom;

public static class AssignableDictionaryExtensions {
	/// <inheritdoc cref="Assign{TDictionary, TKey, TValue}(TDictionary, IEnumerable{KeyValuePair{TKey, TValue}}, IEqualityComparer{TValue}, ConflictHandling, string?)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TDictionary Assign<TDictionary, TKey, TValue>(
		this TDictionary @this,
		#nullable disable
		IEnumerable<KeyValuePair<TKey, TValue>> other,
		#nullable enable
		// Default equality comparer...
		ConflictHandling conflictHandling,
		string? propertyName = null
	)
	#nullable disable
	where TDictionary : IDictionary<TKey, TValue>
	where TValue : IAssignable<TValue, TValue> =>
	#nullable enable
		@this.Assign(other, EqualityComparer<TValue>.Default, conflictHandling, propertyName);

	/// <summary>
	/// <para>
	/// Merges an<paramref name="other"/> dictionary
	/// into <paramref name="this"/> dictionary of <see cref="IAssignable{TIn, TOut}"/> objects,
	/// pairing the objects using the dictionaries' keys,
	/// following <see langword="null"/>-replacement rules of <see cref="IAssignable{TIn, TOut}"/>,
	/// and leveraging <see cref="IAssignable{TIn, TOut}.Assign(TIn, ConflictHandling)"/>.
	/// </para>
	///
	/// <para>
	/// This method prepends <paramref name="propertyName"/> and a string representation
	/// of the conflicting dictionary key to the <see cref="AssignConflictException.Path">path</see>
	/// of any thrown <see cref="AssignConflictException"/>.
	/// </para>
	/// </summary>
	///
	/// <param name="comparer">
	/// Used to triage <see langword="default"/> values in the dictionaries.
	///
	/// If <c><paramref name="comparer"/>.Equals(x, <see langword="default"/>)</c>
	/// returns <see langword="false"/>, then it is assumed that <c>x <see langword="is not null"/></c>.
	/// </param>
	///
	/// <param name="propertyName">
	/// The name of the property being assigned. This value serves as a debug hint,
	/// which will be assigned to <see cref="AssignConflictException.Path"/> if the values conflict.
	/// </param>
	///
	/// <returns>
	/// <paramref name="this"/> dictionary, once values have been assigned from <paramref name="other"/>.
	///
	/// Sneaky <see langword="default"/> values in <paramref name="this"/> dictionary will be preserved.
	/// Sneaky <see langword="default"/> values in the <paramref name="other"/> dictionary will be added into <paramref name="this"/>.
	/// </returns>
	public static TDictionary Assign<TDictionary, TKey, TValue>(
		this TDictionary @this,
		#nullable disable
		IEnumerable<KeyValuePair<TKey, TValue>> other,
		#nullable enable
		IEqualityComparer<TValue> comparer,
		ConflictHandling conflictHandling,
		string? propertyName = null
	)
	#nullable disable
	where TDictionary : IDictionary<TKey, TValue>
	where TValue : IAssignable<TValue, TValue> {
	#nullable enable
		foreach (var (key, otherValue) in other)
			try {
				@this[key] = (
						!@this.TryGetValue(key, out var thisValue) || comparer.Equals(thisValue, default) ? otherValue
					: comparer.Equals(otherValue, default) ? thisValue
					: thisValue.Assign(otherValue, conflictHandling)
				);
			}

			catch (AssignConflictException conflict) {
				conflict.PrependPropertyAndIndexer(propertyName, key?.ToString());

				throw;
			}

		return @this;
	}

	/// <summary>
	/// <para>
	/// Merges an<paramref name="other"/> string dictionary into <paramref name="this"/> one,
	/// pairing the values using the dictionaries' keys,
	/// following <see langword="null"/>-replacement rules of <see cref="IAssignable{TIn, TOut}"/>,
	/// and leveraging <see cref="AssignableStringExtensions.Assign(string?, string?, StringComparison, ConflictHandling, Func{string, string, string}, string?)"/>.
	/// </para>
	///
	/// <para>
	/// This method prepends <paramref name="propertyName"/> and a string representation
	/// of the conflicting dictionary key to the <see cref="AssignConflictException.Path">path</see>
	/// of any thrown <see cref="AssignConflictException"/>.
	/// </para>
	/// </summary>
	///
	/// <param name="propertyName">
	/// The name of the property being assigned. This value serves as a debug hint,
	/// which will be assigned to <see cref="AssignConflictException.Path"/> if the values conflict.
	/// </param>
	///
	/// <returns>
	/// <paramref name="this"/> dictionary, once values have been assigned from <paramref name="other"/>.
	///
	/// Sneaky <see langword="null"/> values in <paramref name="this"/> dictionary will be preserved.
	/// Sneaky <see langword="null"/> values in the <paramref name="other"/> dictionary will be added into <paramref name="this"/>.
	/// </returns>
	public static TDictionary Assign<TDictionary, TKey>(
		this TDictionary @this,
		#nullable disable
		IEnumerable<KeyValuePair<TKey, string>> other,
		#nullable enable
		StringComparison comparisonType,
		ConflictHandling conflictHandling,
		Func<string, string, string> mergeSelector,
		string? propertyName = null
	#nullable disable
	) where TDictionary : IDictionary<TKey, string> {
	#nullable enable
		foreach (var (key, otherValue) in other)
			try {
				@this[key] = @this.TryGetValue(key, out var thisValue) && thisValue != null
					? thisValue.Assign(otherValue, comparisonType, conflictHandling, mergeSelector)
					: otherValue;
			}

			catch (AssignConflictException conflict) {
				conflict.PrependPropertyAndIndexer(propertyName, key?.ToString());

				throw;
			}

		return @this;
	}


	/// <inheritdoc cref="Assign{TDictionary, TKey, TValue}(TDictionary, IEnumerable{KeyValuePair{TKey, TValue}}, IEqualityComparer{TValue}, ConflictHandling, Func{TValue, TValue, TValue}, string?)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TDictionary Assign<TDictionary, TKey, TValue>(
		this TDictionary @this,
		#nullable disable
		IEnumerable<KeyValuePair<TKey, TValue>> other,
		#nullable enable
		// Default comparer...
		ConflictHandling conflictHandling,
		Func<TValue, TValue, TValue> mergeSelector,
		string? propertyName = null
	#nullable disable
	) where TDictionary : IDictionary<TKey, TValue> =>
	#nullable enable
		@this.Assign(other, EqualityComparer<TValue>.Default, conflictHandling, mergeSelector, propertyName);

	/// <summary>
	/// <para>
	/// Merges an<paramref name="other"/> dictionary into <paramref name="this"/> one,
	/// pairing the objects using the dictionaries' keys,
	/// following <see langword="null"/>-replacement rules of <see cref="IAssignable{TIn, TOut}"/>,
	/// and leveraging <see cref="AssignableValueExtensions.Assign{T}(T, T, IEqualityComparer{T}, ConflictHandling, Func{T, T, T}, string?)"/>.
	/// </para>
	///
	/// <para>
	/// This method prepends <paramref name="propertyName"/> and a string representation
	/// of the conflicting dictionary key to the <see cref="AssignConflictException.Path">path</see>
	/// of any thrown <see cref="AssignConflictException"/>.
	/// </para>
	/// </summary>
	///
	/// <param name="comparer">
	/// Used to triage <see langword="default"/> values in the dictionaries.
	///
	/// If <c><paramref name="comparer"/>.Equals(x, <see langword="default"/>)</c>
	/// returns <see langword="false"/>, then it is assumed that <c>x <see langword="is not null"/></c>.
	/// </param>
	///
	/// <param name="propertyName">
	/// The name of the property being assigned. This value serves as a debug hint,
	/// which will be assigned to <see cref="AssignConflictException.Path"/> if the values conflict.
	/// </param>
	///
	/// <returns>
	/// <paramref name="this"/> dictionary, once values have been assigned from <paramref name="other"/>.
	///
	/// Sneaky <see langword="default"/> values in <paramref name="this"/> dictionary will be preserved.
	/// Sneaky <see langword="default"/> values in the <paramref name="other"/> dictionary will be added into <paramref name="this"/>.
	/// </returns>
	public static TDictionary Assign<TDictionary, TKey, TValue>(
		this TDictionary @this,
		#nullable disable
		IEnumerable<KeyValuePair<TKey, TValue>> other,
		#nullable enable
		IEqualityComparer<TValue> comparer,
		ConflictHandling conflictHandling,
		Func<TValue, TValue, TValue> mergeSelector,
		string? propertyName = null
	#nullable disable
	) where TDictionary : IDictionary<TKey, TValue> {
	#nullable enable
		foreach (var (key, otherValue) in other)
			try {
				@this[key] = @this.TryGetValue(key, out var thisValue)
					? thisValue.Assign(otherValue, comparer, conflictHandling, mergeSelector)
					: otherValue;
			}

			catch (AssignConflictException conflict) {
				conflict.PrependPropertyAndIndexer(propertyName, key?.ToString());

				throw;
			}

		return @this;
	}
}
