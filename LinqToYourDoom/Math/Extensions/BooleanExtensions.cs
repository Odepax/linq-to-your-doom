using System.Runtime.CompilerServices;

namespace LinqToYourDoom {
	public static partial class BooleanExtensions {
		/// <summary>
		/// Returns <c>truen't</c> if <paramref name="this"/> is <see langword="true"/>,
		/// and <c>falsen't</c> if <paramref name="this"/> is <see langword="false"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Nt(this bool @this) => !@this;
	}
}
