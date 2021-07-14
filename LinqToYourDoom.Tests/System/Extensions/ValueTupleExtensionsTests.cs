using System;
using LinqToYourDoom.System;
using LinqToYourDoom.System.Extensions;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.System.Extensions {
	static class ValueTupleExtensionsTests {
		static readonly string[] Empty = Array.Empty<string>();
		static readonly string[] One = new[] { "one" };
		static readonly string[] Two = new[] { "one", "two" };
		static readonly string[] Three = new[] { "one", "two", "three" };

		[Test]
		public static void ToTuple() {
			Assert.Throws<InvalidOperationException>(() => Empty.ToTuple2(ArgumentValidation.Strict));
			Assert.Throws<InvalidOperationException>(() => One.ToTuple2(ArgumentValidation.Strict));
			Assert.AreEqual(("one", "two"), Two.ToTuple2(ArgumentValidation.Strict));
			Assert.Throws<InvalidOperationException>(() => Three.ToTuple2(ArgumentValidation.Strict));

			Assert.Throws<InvalidOperationException>(() => Empty.ToTuple2(ArgumentValidation.Lenient));
			Assert.Throws<InvalidOperationException>(() => One.ToTuple2(ArgumentValidation.Lenient));
			Assert.AreEqual(("one", "two"), Two.ToTuple2(ArgumentValidation.Lenient));
			Assert.AreEqual(("one", "two"), Three.ToTuple2(ArgumentValidation.Lenient));

			Assert.AreEqual(("_", "_"), Empty.ToTuple2("_", ArgumentValidation.Strict));
			Assert.AreEqual(("one", "_"), One.ToTuple2("_", ArgumentValidation.Strict));
			Assert.AreEqual(("one", "two"), Two.ToTuple2("_", ArgumentValidation.Strict));
			Assert.Throws<InvalidOperationException>(() => Three.ToTuple2("_", ArgumentValidation.Strict));

			Assert.AreEqual(("_", "_"), Empty.ToTuple2("_", ArgumentValidation.Lenient));
			Assert.AreEqual(("one", "_"), One.ToTuple2("_", ArgumentValidation.Lenient));
			Assert.AreEqual(("one", "two"), Two.ToTuple2("_", ArgumentValidation.Lenient));
			Assert.AreEqual(("one", "two"), Three.ToTuple2("_", ArgumentValidation.Lenient));

			Assert.AreEqual(("0", "1"), Empty.ToTuple2(i => i.ToString(), ArgumentValidation.Strict));
			Assert.AreEqual(("one", "1"), One.ToTuple2(i => i.ToString(), ArgumentValidation.Strict));
			Assert.AreEqual(("one", "two"), Two.ToTuple2(i => i.ToString(), ArgumentValidation.Strict));
			Assert.Throws<InvalidOperationException>(() => Three.ToTuple2(i => i.ToString(), ArgumentValidation.Strict));

			Assert.AreEqual(("0", "1"), Empty.ToTuple2(i => i.ToString(), ArgumentValidation.Lenient));
			Assert.AreEqual(("one", "1"), One.ToTuple2(i => i.ToString(), ArgumentValidation.Lenient));
			Assert.AreEqual(("one", "two"), Two.ToTuple2(i => i.ToString(), ArgumentValidation.Lenient));
			Assert.AreEqual(("one", "two"), Three.ToTuple2(i => i.ToString(), ArgumentValidation.Lenient));
		}
	}
}
