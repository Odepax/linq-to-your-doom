using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.System {
	static class DeepCloneableTests {
		sealed class Example : IDeepCloneable<Example>, IEnumerable<Example> {
			public string Value;
			public List<Example> Children = new();

			public Example DeepClone(uint depth = uint.MaxValue) =>
				new(Value) {
					Children = depth == 0
						? Children.ToList()
						: Children.Select(it => it.DeepClone(depth - 1)).ToList()
				};

			#region Syntax Sugar

			public Example(string value) => Value = value;
			public void Add(Example child) => Children.Add(child);

			public Example this[Index i] => Children[i];

			IEnumerator<Example> IEnumerable<Example>.GetEnumerator() => Children.GetEnumerator();
			IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();

			#endregion
		}

		static Example e = null!;
		static Example d = null!;
		static Example c = null!;
		static Example b = null!;
		static Example a = null!;

		[SetUp]
		public static void SetUp() {
			e = new("E");
			d = new("D");
			c = new("C") { e };
			b = new("B") { c };
			a = new("A") { b, d };
		}

		static readonly Example z = new("Z");

		[Test]
		[TestCase(0u, false, true, true, true, true)]
		[TestCase(1u, false, false, true, false, true)]
		[TestCase(2u, false, false, false, false, true)]
		[TestCase(3u, false, false, false, false, false)]
		[TestCase(4u, false, false, false, false, false)]
		public static void DeepCloning(uint depth, bool sameA, bool sameB, bool sameC, bool sameD, bool sameE) {
			var clone = a.DeepClone(depth);

			Assert.IsTrue(sameA == ReferenceEquals(a, clone));
			Assert.IsTrue(sameB == ReferenceEquals(b, clone[0]));
			Assert.IsTrue(sameC == ReferenceEquals(c, clone[0][0]));
			Assert.IsTrue(sameD == ReferenceEquals(d, clone[1]));
			Assert.IsTrue(sameE == ReferenceEquals(e, clone[0][0][0]));

			Assert.IsTrue(sameA == ReferenceEquals(a.Children, clone.Children));
			Assert.IsTrue(sameB == ReferenceEquals(b.Children, clone[0].Children));
			Assert.IsTrue(sameC == ReferenceEquals(c.Children, clone[0][0].Children));
			Assert.IsTrue(sameD == ReferenceEquals(d.Children, clone[1].Children));
			Assert.IsTrue(sameE == ReferenceEquals(e.Children, clone[0][0][0].Children));

			Assert.AreEqual("A", clone.Value);
			Assert.AreEqual("B", clone[0].Value);
			Assert.AreEqual("C", clone[0][0].Value);
			Assert.AreEqual("D", clone[1].Value);
			Assert.AreEqual("E", clone[0][0][0].Value);

			Assert.AreEqual(2, clone.Children.Count);
			Assert.AreEqual(1, clone[0].Children.Count);
			Assert.AreEqual(1, clone[0][0].Children.Count);
			Assert.AreEqual(0, clone[1].Children.Count);
			Assert.AreEqual(0, clone[0][0][0].Children.Count);

			// Modifications to the clone has no effect on the original.
			clone.Add(z);
			clone[0].Add(z);
			clone[0][0].Add(z);
			clone[1].Add(z);
			clone[0][0][0].Add(z);

			Assert.AreEqual(sameA ? 3 : 2, a.Children.Count);
			Assert.AreEqual(sameB ? 2 : 1, b.Children.Count);
			Assert.AreEqual(sameC ? 2 : 1, c.Children.Count);
			Assert.AreEqual(sameD ? 1 : 0, d.Children.Count);
			Assert.AreEqual(sameE ? 1 : 0, e.Children.Count);
		}
	}
}
