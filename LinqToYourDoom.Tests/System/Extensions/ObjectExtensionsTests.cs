using System.Collections.Generic;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.System.Extensions;

static class ObjectExtensionsTests {
	[Test]
	public static void Local_functions_learning_test_1() {
		var K = 1;

		Assert.AreEqual(1, K);
		{
			Assert.AreEqual(1, K);
			K = 2;
			Assert.AreEqual(2, K);
		}
		Assert.AreEqual(2, K);
	}

	[Test]
	public static void Local_functions_learning_test_2() {
		var K = 1;

		Assert.AreEqual(1, f());
		{
			Assert.AreEqual(1, f());
			K = 2;
			Assert.AreEqual(2, f());
		}
		Assert.AreEqual(2, f());

		int f() => K;
	}

	[Test]
	public static void Local_functions_learning_test_3() {
		var values = new int[3];
		var K = 1;

		for (var i = 0; i < 3; ++i) {
			K *= 10;

			values[i] = f();
		}

		Assert.AreEqual(new[] { 10, 100, 1000 }, values);

		int f() => K;
	}

	[Test]
	public static void Tee() {
		var in1 = 1;
		var in2 = (1, 2);
		var in3 = (1, 2, 3);
		var in4 = (1, 2, 3, 4);
		var in5 = (1, 2, 3, 4, 5);

		var out1 = 0;
		var out2 = 0;
		var out3 = 0;
		var out4 = 0;
		var out5 = 0;

		in1.Tee(out out1);

		Assert.AreEqual(1, out1);

		in2.Tee(out out1, out out2);

		Assert.AreEqual(1, out1);
		Assert.AreEqual(2, out2);

		in3.Tee(out out1, out out2, out out3);

		Assert.AreEqual(1, out1);
		Assert.AreEqual(2, out2);
		Assert.AreEqual(3, out3);

		in4.Tee(out out1, out out2, out out3, out out4);

		Assert.AreEqual(1, out1);
		Assert.AreEqual(2, out2);
		Assert.AreEqual(3, out3);
		Assert.AreEqual(4, out4);

		in5.Tee(out out1, out out2, out out3, out out4, out out5);

		Assert.AreEqual(1, out1);
		Assert.AreEqual(2, out2);
		Assert.AreEqual(3, out3);
		Assert.AreEqual(4, out4);
		Assert.AreEqual(5, out5);

		in1.Tee(out out1, it => it * 10);

		Assert.AreEqual(10, out1);

		in2.Tee(out out1, out out2, it => (it.Item1 * 10, it.Item2 * 2));

		Assert.AreEqual(10, out1);
		Assert.AreEqual(4, out2);

		in3.Tee(out out1, out out2, out out3, it => (it.Item1 * 10, it.Item2 * 2, it.Item3 * 3));

		Assert.AreEqual(10, out1);
		Assert.AreEqual(4, out2);
		Assert.AreEqual(9, out3);

		in4.Tee(out out1, out out2, out out3, out out4, it => (it.Item1 * 10, it.Item2 * 2, it.Item3 * 3, it.Item4 * 4));

		Assert.AreEqual(10, out1);
		Assert.AreEqual(4, out2);
		Assert.AreEqual(9, out3);
		Assert.AreEqual(16, out4);

		in5.Tee(out out1, out out2, out out3, out out4, out out5, it => (it.Item1 * 10, it.Item2 * 2, it.Item3 * 3, it.Item4 * 4, it.Item5 * 5));

		Assert.AreEqual(10, out1);
		Assert.AreEqual(4, out2);
		Assert.AreEqual(9, out3);
		Assert.AreEqual(16, out4);
		Assert.AreEqual(25, out5);
	}

	[Test]
	public static void To() {
		Assert.AreEqual((true, 1, 20), ("A", 2, false).To((a, b, c) => (!c, a.Length, b * 10)));
		Assert.AreEqual("P314", KeyValuePair.Create("PI", 3.14).To((k, v) => k[0..1] + v * 100));
	}

	[Test]
	public static void Also() {
		(bool, int, int) x = default;
		string y = null!;

		("ABC", 1, true).Also((a, b, c) => { x = (!c, a.Length, b * 10); });
		Assert.AreEqual((false, 3, 10), x);

		KeyValuePair.Create("K", 42).Also((k, v) => { y = k[0..1] + v * 100; });
		Assert.AreEqual("K4200", y);

		("A", 2, false).Also((a, b, c) => x = (!c, a.Length, b * 10));
		Assert.AreEqual((true, 1, 20), x);

		KeyValuePair.Create("PI", 3.14).Also((k, v) => y = k[0..1] + v * 100);
		Assert.AreEqual("P314", y);
	}
}
