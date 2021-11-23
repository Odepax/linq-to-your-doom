namespace LinqToYourDoom;

/// <summary>
/// <see cref="IAssignable{TIn, TOut}"/> was thought of as a deep counterpart to JavaScript's
/// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/assign"><c>Object.assign()</c></see>.
/// It works better on <see langword="sealed"/> mutable data classes,
/// but nothing stops you from using it on function objects or immutable structures.
/// </summary>
///
/// <typeparam name="TIn">
/// The type that this object is assignable from.
/// </typeparam>
///
/// <typeparam name="TOut">
/// Meant to be the type implementing this interface,
/// e.g. <c>class Cat : IAssignable&lt;Cat, Cat&gt; {}</c>.
///
/// Use the same as <typeparamref name="TIn"/>
/// unless assigning from <typeparamref name="TIn"/> doesn't return the same type of object.
/// </typeparam>
public interface IAssignable<in TIn, out TOut> {
	/// <remarks>
	/// <para>
	/// Data should always be assigned to <i>"fill the <see langword="null"/>s"</i>,
	/// i.e. overriding <see langword="default"/> values takes precedence
	/// over the <paramref name="conflictHandling"/> policy.
	/// </para>
	///
	/// <para>
	/// The implementer isn't assumed to be mutable or immutable.
	/// Obviously, for mutable types, the <see langword="this"/> receiver object should be modified.,
	/// but the <typeparamref name="TOut"/> return value of <see cref="Assign(TIn, ConflictHandling)"/>
	/// gives the occasion to make immutable types assignable as well.
	/// </para>
	/// </remarks>
	///
	/// <param name="other">
	/// Data from <paramref name="other"/> will be copied into <see langword="this"/> object.
	/// </param>
	///
	/// <returns>
	/// If mutable: <see langword="this"/>, once assigned from <paramref name="other"/>.
	/// If immutable: a new merged object.
	/// </returns>
	///
	/// <exception cref="AssignConflictException">
	/// When <paramref name="other"/> contains non-default and different/conflicting data,
	/// and <paramref name="conflictHandling"/> is <see cref="ConflictHandling.Throw"/>.
	/// </exception>
	///
	/// <example>
	/// Implementation Hints
	///
	/// Table:
	///
	/// <code>
	/// '   | Data Source  | Result = f(conflictHandling)         |
	/// | # | this | other | Replace | Ignore | Merge | Throw     |
	/// | - | ---- | ----- | ------- | ------ | ----- | --------- |
	/// | 1 | null | null  | null    '        '       '           |
	/// | 2 | null | O     | O       '        '       '           |
	/// | 3 | T    | null  | T       '        '       '           |
	/// | 4 | T    | T     | T       '        '       '           |
	/// | 5 | T    | O     | O       | T      | T * O | Konflikt! |
	/// </code>
	///
	/// Flow:
	///
	/// <code>
	/// #1 #2 |      if (this  is null) => other
	/// #3    | else if (other is null) => this
	/// #4    | else if (this == other) => this
	/// '     | else
	/// #5.1  |      if (conflictHandling == Replace) => other
	/// #5.2  | else if (conflictHandling == Ignore)  => this
	/// #5.3  | else if (conflictHandling == Merge)   => Implementation-specific...
	/// #5.4  | else if (conflictHandling == Throw)   => throw
	/// </code>
	/// </example>
	TOut Assign(TIn other, ConflictHandling conflictHandling = default);
}
