using System.Runtime.CompilerServices;

namespace LinqToYourDoom.Assignable.Extensions {
	public static class AssignableExtensions {
		/// <inheritdoc cref="Assign{TAssignable, TIn, TOut}(TAssignable, TIn, ConflictHandling, string?)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TOut Assign<TIn, TOut>(
			this TIn @this,
			TIn other,
			ConflictHandling conflictHandling = default,
			string? propertyName = null
		) where TIn : IAssignable<TIn, TOut> =>
			@this.Assign<TIn, TIn, TOut>(other, conflictHandling, propertyName);

		/// <inheritdoc cref="Assign{TAssignable, TIn, TOut}(TAssignable, TIn, ConflictHandling, string?)"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TOut Assign<TIn, TOut>(
			this TOut @this,
			TIn other,
			ConflictHandling conflictHandling = default,
			string? propertyName = null
		) where TOut : IAssignable<TIn, TOut> =>
			@this.Assign<TOut, TIn, TOut>(other, conflictHandling, propertyName);

		/// <summary>
		/// An extension that surrounds a call to <see cref="IAssignable{TIn, TOut}.Assign(TIn, ConflictHandling)"/>,
		/// and prepends <paramref name="propertyName"/> to the <see cref="AssignConflictException.Path">path</see>
		/// of any thrown <see cref="AssignConflictException"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TOut Assign<TAssignable, TIn, TOut>(
			this TAssignable @this,
			TIn other,
			ConflictHandling conflictHandling = default,
			string? propertyName = null
		) where TAssignable : IAssignable<TIn, TOut> {
			try {
				return @this.Assign(other, conflictHandling);
			}

			catch (AssignConflictException conflict) {
				conflict.PrependProperty(propertyName);

				throw;
			}
		}
	}
}
