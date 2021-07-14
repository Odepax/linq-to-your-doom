using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LinqToYourDoom.Assignable.Extensions {
	public static class AssignableValueExtensions {
		/// <inheritdoc cref="Assign{T}(T, T, IEqualityComparer{T}, ConflictHandling, Func{T, T, T}, string?)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? Assign<T>(
			this T? @this,
			T? other,
			// Default equality comparer here...
			ConflictHandling conflictHandling,
			Func<T, T, T> mergeSelector,
			string? propertyName = null
		) => Assign(@this, other, EqualityComparer<T>.Default, conflictHandling, mergeSelector, propertyName);

		/// <summary>
		/// Merges two values according to the <see cref="IAssignable{TIn, TOut}"/> rules.
		/// </summary>
		///
		/// <param name="comparer">
		/// If <c><paramref name="comparer"/>.Equals(x, <see langword="default"/>)</c>
		/// returns <see langword="false"/>, then it is assumed that <c>x <see langword="is not null"/></c>.
		/// </param>
		///
		/// <param name="mergeSelector">
		/// A custom merging function to call when <paramref name="this"/> and <paramref name="other"/> are conflicting
		/// and <paramref name="conflictHandling"/> is <see cref="ConflictHandling.Merge"/>.
		/// </param>
		///
		/// <param name="propertyName">
		/// The name of the property being assigned. This value serves as a debug hint,
		/// which will be assigned to <see cref="AssignConflictException.Path"/> if the values conflict.
		/// </param>
		///
		/// <returns>
		/// This method will return the first non-<see langword="default"/> parameter,
		/// or fallback to <c><see langword="default"/>(<typeparamref name="T"/>)</c>.
		/// </returns>
		///
		/// <exception cref="AssignConflictException">
		/// When <paramref name="this"/> and <paramref name="other"/> have non-equal and non-<see langword="default"/> values,
		/// and <paramref name="conflictHandling"/> is <see cref="ConflictHandling.Throw"/>.
		/// </exception>
		public static T? Assign<T>(
			this T? @this,
			T? other,
			IEqualityComparer<T> comparer,
			ConflictHandling conflictHandling,
			Func<T, T, T> mergeSelector,
			string? propertyName = null
		) => (
			  comparer.Equals(@this, default) ? other
			: comparer.Equals(other, default) ? @this
			: comparer.Equals(other, @this) ? @this
			: conflictHandling == ConflictHandling.Replace ? other
			: conflictHandling == ConflictHandling.Ignore ? @this
			: conflictHandling == ConflictHandling.Merge ? mergeSelector.Invoke(@this!, other!)
			: conflictHandling == ConflictHandling.Throw ? throw new AssignConflictException(propertyName)
			: throw new Bug("C8CB9E44-E1E7-4002-837A-7C90617F0761")
		);
	}
}
