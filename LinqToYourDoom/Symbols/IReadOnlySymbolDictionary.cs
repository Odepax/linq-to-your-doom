using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LinqToYourDoom {
	/// <inheritdoc cref="SymbolDictionary"/>
	public interface IReadOnlySymbolDictionary : IReadOnlyCollection<KeyValuePair<Symbol, object>> {
		/// <value>
		/// A collection that contains the keys of this <see cref="SymbolDictionary"/>.
		/// </value>
		IEnumerable<Symbol> Keys { get; }

		/// <summary>
		/// Gets the <paramref name="value"/> associated with the specified <paramref name="key"/>.
		/// </summary>
		///
		/// <param name="value">
		/// When this method returns <see langword="true"/>,
		/// this parameter is set to the value associated with the specified <paramref name="key"/>.
		/// </param>
		///
		/// <returns>
		/// <see langword="true"/> if this <see cref="SymbolDictionary"/> contains a value
		/// for the specified <paramref name="key"/>, <see langword="false"/> otherwise.
		/// </returns>
		bool TryGet<T>(Symbol<T> key, [NotNullWhen(true)] out T? value);
	}
}
