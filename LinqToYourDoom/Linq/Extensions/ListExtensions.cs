using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LinqToYourDoom;

public static class ListExtensions {
	/// <summary>
	/// Alias for <see cref="List{T}.Clear()"/> then <see cref="List{T}.AddRange(IEnumerable{T})"/>.
	/// </summary>
	///
	/// <returns>
	/// <paramref name="this"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static List<T> Set<T>(this List<T> @this, IEnumerable<T> collection) {
		@this.Clear();
		@this.AddRange(collection);

		return @this;
	}
}
