using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LinqToYourDoom;

/// <summary>
/// Represents a random generator.
/// The implementation <b>must be thread-safe</b>.
/// The implementation <b>may be pseudo-random</b>.
/// </summary>
///
/// <remarks>
/// <i>D</i> stands for <i>Doom</i>, of course...
/// </remarks>
public partial interface IRandomD {
	/// <summary>
	/// Returns a random item amongst the <paramref name="values"/> of a 0-based indexed collection of contiguous items.
	/// </summary>
	///
	/// <exception cref="ArgumentException">
	/// When <paramref name="values"/> is empty.
	/// </exception>
	public T In<T>(IReadOnlyList<T> values);

	/// <summary>
	/// Tries to return a random item amongst the <paramref name="values"/> of a 0-based indexed collection of contiguous items.
	/// </summary>
	///
	/// <returns>
	/// <see langword="true"/> if an item was selected and <paramref name="out"/> is set,
	/// <see langword="false"/> if <paramref name="values"/> was an empty collection.
	/// </returns>
	public bool TryIn<T>(IReadOnlyList<T> values, [MaybeNullWhen(false)] out T? @out);

	/// <summary>
	/// Returns a random item amongst the <paramref name="values"/> of a 0-based indexed collection of contiguous items,
	/// removing the selected value from the collection.
	/// </summary>
	///
	/// <exception cref="ArgumentException">
	/// When <paramref name="values"/> is empty.
	/// </exception>
	public T Pop<T>(IList<T> values);

	/// <summary>
	/// Tries to return a random item amongst the <paramref name="values"/> of a 0-based indexed collection of contiguous items,
	/// removing the selected value from the collection.
	/// </summary>
	///
	/// <returns>
	/// <see langword="true"/> if an item was selected and <paramref name="out"/> is set,
	/// <see langword="false"/> if <paramref name="values"/> was an empty collection.
	/// </returns>
	public bool TryPop<T>(IList<T> values, [MaybeNullWhen(false)] out T? @out);

	/// <returns> Either <c>+1</c> or <c>-1</c> with a 50-50% chance. </returns>
	public int Sign();

	/// <returns> Either <see langword="true"/> or <see langword="false"/> with a 50-50% chance. </returns>
	public bool Bool();
}
