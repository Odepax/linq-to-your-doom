using System;
using System.Runtime.CompilerServices;

namespace LinqToYourDoom {
	/// <summary> <i>D</i> stands for <i>Doom</i>, of course... </summary>
	public static partial class MathD {
		public static readonly RandomD Random = new();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static (T Min, T Max) MinMax<T>(T a, T b) where T : IComparable<T> =>
			a.CompareTo(b) < 0 ? (a, b) : (b, a);
	}
}
