using NUnit.Framework;

namespace LinqToYourDoom.Tests.Math {
	static class MathDTests {
		[Test]
		[TestCase(1, 1, 1)]
		[TestCase(1, 12, 1)]
		[TestCase(-12, 1, 1)]
		[TestCase(42, -12, -12)]
		[TestCase(-1, -12, -1)]
		public static void AbsMin(int a, int b, int expected) {
			var actual = MathD.AbsMin(a, b);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		[TestCase(1, 1, 1)]
		[TestCase(1, 12, 12)]
		[TestCase(-12, 1, -12)]
		[TestCase(42, -12, 42)]
		[TestCase(-1, -12, -12)]
		public static void AbsMax(int a, int b, int expected) {
			var actual = MathD.AbsMax(a, b);

			Assert.AreEqual(expected, actual);
		}
	}
}
