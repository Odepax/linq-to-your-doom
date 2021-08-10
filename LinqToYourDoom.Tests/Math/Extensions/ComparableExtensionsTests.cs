using System;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Math.Extensions {
	static class ComparableExtensionsTests {
		[Test]
		[TestCase(10, -100, 10)]
		[TestCase(10, 4, 10)]
		[TestCase(10, 10, 10)]
		[TestCase(10, 12, 12)]
		[TestCase(10, 200, 200)]
		public static void CoerceAtLeast(int value, int min, int expected) {
			var actual = value.CoerceAtLeast(min);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase(10, -100, -100)]
		[TestCase(10, 4, 4)]
		[TestCase(10, 10, 10)]
		[TestCase(10, 12, 10)]
		[TestCase(10, 200, 10)]
		public static void CoerceAtMost(int value, int max, int expected) {
			var actual = value.CoerceAtMost(max);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase(50, 10, 20, 20)]
		[TestCase(50, 10, 50, 50)]
		[TestCase(50, 10, 70, 50)]
		[TestCase(50, 50, 50, 50)]
		[TestCase(50, 50, 70, 50)]
		[TestCase(50, 60, 70, 60)]
		public static void CoerceIn(int value, int min, int max, int expected) {
			var actual = value.CoerceIn(min, max);
			var actualLenientReverse = value.CoerceIn(max, min, ArgumentValidation.Lenient);

			Assert.AreEqual(expected, actual);
			Assert.AreEqual(expected, actualLenientReverse);

			if (min == max) {
				var actualStrictReverse = value.CoerceIn(max, min);

				Assert.AreEqual(expected, actualStrictReverse);
			}

			else Assert.Throws<ArgumentException>(() => value.CoerceIn(max, min));
		}
	}
}
