using System;
using LinqToYourDoom.Text.Extensions;

namespace LinqToYourDoom.Assignable.Extensions {
	public static class AssignableStringExtensions {
		/// <summary>
		/// Merges two <see cref="string"/>s according to the <see cref="IAssignable{TIn, TOut}"/> rules.
		/// </summary>
		///
		/// <param name="mergeSelector">
		/// A custom merging function to call if <paramref name="this"/> and <paramref name="other"/> are conflicting
		/// and <paramref name="conflictHandling"/> is <see cref="ConflictHandling.Merge"/>, e.g. <see cref="string.Concat(string?, string?)"/>.
		/// </param>
		///
		/// <param name="propertyName">
		/// The name of the property being assigned. This value serves as a debug hint,
		/// which will be assigned to <see cref="AssignConflictException.Path"/> if the values conflict.
		/// </param>
		///
		/// <returns>
		/// This method doesn't propagate <see langword="null"/> and blank strings:
		/// it will return the first parameter that isn't <see cref="string.IsNullOrWhiteSpace(string?)">null or blank</see>,
		/// or fallback to <see cref="string.Empty"/>.
		/// </returns>
		///
		/// <exception cref="AssignConflictException">
		/// When <paramref name="this"/> and <paramref name="other"/> have non-equal and non-blank values,
		/// and <paramref name="conflictHandling"/> is <see cref="ConflictHandling.Throw"/>.
		/// </exception>
		public static string Assign(
			this string? @this,
			string? other,
			StringComparison comparisonType,
			ConflictHandling conflictHandling,
			Func<string, string, string> mergeSelector,
			string? propertyName = null
		) => (
			  @this.IsNullOrWhiteSpace() ? other.EmptyIfNullOrWhiteSpace()
			: other.IsNullOrWhiteSpace() ? @this
			: other.Equals(@this, comparisonType) ? @this
			: conflictHandling == ConflictHandling.Replace ? other
			: conflictHandling == ConflictHandling.Ignore ? @this
			: conflictHandling == ConflictHandling.Merge ? mergeSelector.Invoke(@this, other)
			: conflictHandling == ConflictHandling.Throw ? throw new AssignConflictException(propertyName)
			: throw new Bug("3BBA0DFC-220D-45F2-AA6F-397D4461E227")
		);
	}
}
