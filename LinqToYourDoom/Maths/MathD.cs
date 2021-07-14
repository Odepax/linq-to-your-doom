using System;
using System.Runtime.CompilerServices;

namespace LinqToYourDoom.Maths {
	public static partial class MathD {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static (T Min, T Max) MinMax<T>(T a, T b) where T : IComparable<T> =>
			a.CompareTo(b) < 0 ? (a, b) : (b, a);
	}
}
