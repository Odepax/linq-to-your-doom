using System;

namespace LinqToYourDoom {
	public readonly struct AssignCallbackDictionary : IShallowCloneable<AssignCallbackDictionary> {
		delegate object UntypedAssignCallback(Symbol symbol, object a, object b, ConflictHandling conflictHandling);

		readonly InnerTypedDictionary<Type, UntypedAssignCallback> Storage;
		AssignCallbackDictionary(InnerTypedDictionary<Type, UntypedAssignCallback> storage) => Storage = storage;

		public static AssignCallbackDictionary New() => new(InnerTypedDictionary<Type, UntypedAssignCallback>.New());
		public static AssignCallbackDictionary New(int capacity) => new(InnerTypedDictionary<Type, UntypedAssignCallback>.New(capacity));

		/// <summary>
		/// Adds an assign callback for the given type <typeparamref name="T"/>.
		/// </summary>
		///
		/// <returns> <see langword="this"/>. </returns>
		public AssignCallbackDictionary Add<T>(Func<Symbol<T>, T, T, ConflictHandling, T> assignCallback) {
			Storage.Add<UntypedAssignCallback>(typeof(T), (symbol, a, b, conflictHandling) =>
				assignCallback.Invoke((Symbol<T>) symbol, (T) a, (T) b, conflictHandling)!
			);

			return this;
		}

		/// <summary>
		/// Untyped, global, routing callback, which is used by
		/// <see cref="SymbolDictionary.Assign(IReadOnlySymbolDictionary, ConflictHandling, AssignCallbackDictionary)"/>.
		/// </summary>
		internal object AssignCallback(Symbol symbol, object a, object b, ConflictHandling conflictHandling) =>
			Storage.Get<UntypedAssignCallback>(symbol.Type).Invoke(symbol, a, b, conflictHandling);

		public AssignCallbackDictionary ShallowClone() => new(Storage.ShallowClone());
	}
}
