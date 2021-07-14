using System.Diagnostics.CodeAnalysis;

namespace LinqToYourDoom.Symbols {
	/// <inheritdoc cref="SymbolDictionary"/>
	public interface IWriteOnlySymbolDictionary {
		/// <summary>
		/// Adds a <paramref name="value"/> under the specified <paramref name="key"/> to this <see cref="ISymbolDictionary"/>,
		/// overriding any existing value.
		/// </summary>
		void Set<T>(Symbol<T> key, T value);

		/// <summary>
		/// Removes the item with the specified <paramref name="key"/> from this <see cref="SymbolDictionary"/>.
		/// </summary>
		///
		/// <returns>
		/// <see langword="true"/> if the item existed in this <see cref="SymbolDictionary"/>,
		/// <see langword="false"/> otherwise.
		///
		/// For an idempotent removal, just ignore the return value.
		/// </returns>
		bool TryRemove<T>(Symbol<T> key, [NotNullWhen(true)] out T? removedValue);

		/// <summary>
		/// Removes all items from this <see cref="SymbolDictionary"/>.
		/// </summary>
		void Clear();
	}
}
