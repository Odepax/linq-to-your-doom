using NUnit.Framework;

namespace LinqToYourDoom.Tests.Math.Extensions {
	static class BooleanExtensionsTests {
		[Test]
		public static void To_() {
			Assert.AreEqual(0, false.ToUShort());
			Assert.AreEqual(1, true.ToSByte());
			Assert.AreEqual(0, false.ToInt());
			Assert.AreEqual(1, true.ToInt());
			Assert.AreEqual(0f, false.ToFloat());
			Assert.AreEqual(1d, true.ToDouble());
		}

		[Test]
		public static void ToBool() {
			Assert.IsFalse(0.ToBool());
			Assert.IsTrue(1.ToBool());
			Assert.IsTrue((-42).ToBool());

			Assert.IsFalse(0f.ToBool());
			Assert.IsTrue(1f.ToBool());
			Assert.IsFalse(float.NaN.ToBool());

			Assert.IsFalse(0d.ToBool());
			Assert.IsTrue(1d.ToBool());
			Assert.IsFalse(double.NaN.ToBool());
		}
	}
}
