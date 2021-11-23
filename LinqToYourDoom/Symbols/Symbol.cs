using System;

namespace LinqToYourDoom;

/// <summary>
/// A non generic <see cref="Symbol{T}"/> for use with collections,
/// since C# doesn't have a <see href="https://docs.oracle.com/javase/tutorial/extra/generics/wildcards.html">generic wildcard</see>.
///
/// Prefer using <see cref="Symbol{T}"/> directly instead.
/// </summary>
///
/// <remarks>
/// Due to the constructor being <see langword="private protected"/>,
/// this is effectively an <see langword="abstract"/> <see langword="sealed"/> class.
/// </remarks>
public abstract class Symbol {
	/// <value>
	/// The generic type of a <see cref="Symbol{T}"/>.
	/// </value>
	///
	/// <seealso cref="Symbol{T}.Type"/>
	public abstract Type Type { get; }

	/// <inheritdoc cref="Symbol"/>
	private protected Symbol() {}

	/// <inheritdoc/>
	///
	/// <remarks>
	/// <see cref="ToString"/> is overriden for <b>debug purposes</b>.
	/// Breaking changes here may not be reflected in the library's semantic versioning!
	/// </remarks>
	public override string ToString() => $"{ GetType().PrettyName() }#{ GetHashCode() }";
}

/// <summary>
/// <see cref="Symbol{T}"/> was thought of as a typed equivalent of JavaScript's
/// <see href="https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Symbol">Symbol objects</see>,
/// and can serve as global unique keys in <i>"per-entry-generic collections"</i>,
/// such as <see cref="SymbolDictionary"/>.
/// </summary>
///
/// <remarks>
/// It might be necessary to store <see cref="Symbol{T}"/> instances
/// in <see langword="static"/> fields or properties in order to reach
/// the <i>"global unique key"</i> semantic.
/// </remarks>
///
/// <seealso cref="IReadOnlySymbolDictionary"/>
/// <seealso cref="IWriteOnlySymbolDictionary"/>
/// <seealso cref="ISymbolDictionary"/>
public sealed class Symbol<T> : Symbol {
	/// <value>
	/// <c><see langword="typeof"/>(<typeparamref name="T"/>)</c>
	/// </value>
	public override Type Type => typeof(T);
}
