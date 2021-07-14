using System;
using System.Collections.Generic;

namespace LinqToYourDoom.Linq {
	public static class EqualityComparer {
		/// <summary>
		/// Returns an equality comparer that uses the given <paramref name="equals"/> delegate
		/// and <see cref="object.GetHashCode"/>.
		/// </summary>
		public static IEqualityComparer<T> From<T>(Func<T?, T?, bool> equals) =>
			new DelegateEqualityComparer<T>(equals);

		sealed class DelegateEqualityComparer<T> : IEqualityComparer<T> {
			readonly Func<T?, T?, bool> EqualsDelegate;

			internal DelegateEqualityComparer(Func<T?, T?, bool> equalsDelegate) =>
				EqualsDelegate = equalsDelegate;

			public bool Equals(T? x, T? y) => EqualsDelegate.Invoke(x, y);
			public int GetHashCode(T obj) => obj!.GetHashCode();
		}

		/// <summary>
		/// Returns an equality comparer that uses the given <paramref name="equals"/>
		/// and <paramref name="getHashCode"/> delegates.
		/// </summary>
		public static IEqualityComparer<T> From<T>(Func<T?, T?, bool> equals, Func<T, int> getHashCode) =>
			new DelegateEqualityAndHashCodeComparer<T>(equals, getHashCode);

		sealed class DelegateEqualityAndHashCodeComparer<T> : IEqualityComparer<T> {
			readonly Func<T?, T?, bool> EqualsDelegate;
			readonly Func<T, int> GetHashCodeDelegate;

			internal DelegateEqualityAndHashCodeComparer(Func<T?, T?, bool> equalsDelegate, Func<T, int> getHashCodeDelegate) {
				EqualsDelegate = equalsDelegate;
				GetHashCodeDelegate = getHashCodeDelegate;
			}

			public bool Equals(T? x, T? y) => EqualsDelegate.Invoke(x, y);
			public int GetHashCode(T obj) => GetHashCodeDelegate.Invoke(obj);
		}

		/// <summary>
		/// Returns an equality comparer that uses <see cref="object.Equals(object?, object?)"/>
		/// and the given <paramref name="getHashCode"/> delegate.
		/// </summary>
		public static IEqualityComparer<T> From<T>(Func<T, int> getHashCode) =>
			new DelegateHashCodeComparer<T>(getHashCode);

		sealed class DelegateHashCodeComparer<T> : IEqualityComparer<T> {
			readonly Func<T, int> GetHashCodeDelegate;

			internal DelegateHashCodeComparer(Func<T, int> getHashCodeDelegate) =>
				GetHashCodeDelegate = getHashCodeDelegate;

			public bool Equals(T? x, T? y) => object.Equals(x, y);
			public int GetHashCode(T obj) => GetHashCodeDelegate.Invoke(obj);
		}
	}
}
