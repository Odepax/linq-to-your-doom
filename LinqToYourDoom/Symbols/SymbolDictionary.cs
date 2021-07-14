using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LinqToYourDoom.Assignable;
using LinqToYourDoom.Linq.Extensions;
using LinqToYourDoom.System;

namespace LinqToYourDoom.Symbols {
	/// <summary>
	/// A collection where the <i>"type"</i> of each key matches the type of its value
	/// on a key-value-pair basis.
	/// </summary>
	///
	/// <remarks>
	/// <see cref="SymbolDictionary"/> is a typed value bag: unlike <see cref="IDictionary{TKey, TValue}"/>,
	/// where all keys have the same type <c>K</c>, and all values have the same type <c>V</c>,
	/// <see cref="SymbolDictionary"/> uses <see cref="Symbol{T}"/> as keys to maintain type safety
	/// while allowing each key-value-pair to have its distinct type.
	/// </remarks>
	public sealed class SymbolDictionary : ISymbolDictionary, IShallowCloneable<SymbolDictionary>, IAssignable<IReadOnlySymbolDictionary, SymbolDictionary> {
		readonly InnerTypedDictionary<Symbol, object?> Storage;
		SymbolDictionary(InnerTypedDictionary<Symbol, object?> storage) => Storage = storage;

		public SymbolDictionary() : this(InnerTypedDictionary<Symbol, object?>.New()) {}
		public SymbolDictionary(int capacity) : this(InnerTypedDictionary<Symbol, object?>.New(capacity)) {}

		public int Count => Storage.Count;
		public IEnumerable<Symbol> Keys => Storage.Keys;

		public bool TryGet<T>(Symbol<T> key, [NotNullWhen(true)] out T? value) => Storage.TryGet(key, out value);
		public void Set<T>(Symbol<T> key, T value) => Storage.Set(key, value);
		public bool TryRemove<T>(Symbol<T> key, [NotNullWhen(true)] out T? removedValue) => Storage.TryRemove(key, out removedValue);
		public void Clear() => Storage.Clear();

		public IEnumerator<KeyValuePair<Symbol, object>> GetEnumerator() => Storage.WhereValueNotNull().GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public SymbolDictionary ShallowClone() => new(Storage.ShallowClone());

		public SymbolDictionary Assign(IReadOnlySymbolDictionary other, ConflictHandling conflictHandling = ConflictHandling.Replace) {
			Storage.Assign(other!, conflictHandling);

			return this;
		}
	}
}
