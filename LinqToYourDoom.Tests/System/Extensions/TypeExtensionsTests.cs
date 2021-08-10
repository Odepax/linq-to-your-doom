using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.System.Extensions {
	static class TypeExtensionsTests {
		interface IX {}
		interface IY {}
		interface IZ : IX, IY {}

		interface IA {}
		interface IB : IA {}
		interface IC : IB {}

		abstract class A : IX {}
		class B : A {}
		class C : B, IY {}
		class D : B, IZ {}

		[Test]
		[TestCase(typeof(A), typeof(B), false)]
		[TestCase(typeof(A), typeof(C), false)]
		[TestCase(typeof(B), typeof(A), true)]
		[TestCase(typeof(B), typeof(C), false)]
		[TestCase(typeof(C), typeof(A), true)]
		[TestCase(typeof(C), typeof(B), true)]

		[TestCase(typeof(A), typeof(IX), true)]
		[TestCase(typeof(A), typeof(IY), false)]
		[TestCase(typeof(B), typeof(IX), true)]
		[TestCase(typeof(B), typeof(IY), false)]
		[TestCase(typeof(C), typeof(IX), true)]
		[TestCase(typeof(C), typeof(IY), true)]

		[TestCase(typeof(IA), typeof(IB), false)]
		[TestCase(typeof(IA), typeof(IC), false)]
		[TestCase(typeof(IB), typeof(IA), true)]
		[TestCase(typeof(IB), typeof(IC), false)]
		[TestCase(typeof(IC), typeof(IA), true)]
		[TestCase(typeof(IC), typeof(IB), true)]

		[TestCase(typeof(D), typeof(IX), true)]
		[TestCase(typeof(D), typeof(IY), true)]
		[TestCase(typeof(D), typeof(IZ), true)]
		[TestCase(typeof(D), typeof(A), true)]
		[TestCase(typeof(D), typeof(B), true)]
		[TestCase(typeof(D), typeof(C), false)]

		[TestCase(typeof(List<A>), typeof(IEnumerable<B>), false)]
		[TestCase(typeof(List<A>), typeof(IEnumerable<C>), false)]
		[TestCase(typeof(List<B>), typeof(IEnumerable<A>), true)]
		[TestCase(typeof(List<B>), typeof(IEnumerable<C>), false)]
		[TestCase(typeof(List<C>), typeof(IEnumerable<A>), true)]
		[TestCase(typeof(List<C>), typeof(IEnumerable<B>), true)]

		[TestCase(typeof(List<A>), typeof(IEnumerable<IX>), true)]
		[TestCase(typeof(List<A>), typeof(IEnumerable<IY>), false)]
		[TestCase(typeof(List<B>), typeof(IEnumerable<IX>), true)]
		[TestCase(typeof(List<B>), typeof(IEnumerable<IY>), false)]
		[TestCase(typeof(List<C>), typeof(IEnumerable<IX>), true)]
		[TestCase(typeof(List<C>), typeof(IEnumerable<IY>), true)]

		[TestCase(typeof(List<IA>), typeof(IEnumerable<IB>), false)]
		[TestCase(typeof(List<IA>), typeof(IEnumerable<IC>), false)]
		[TestCase(typeof(List<IB>), typeof(IEnumerable<IA>), true)]
		[TestCase(typeof(List<IB>), typeof(IEnumerable<IC>), false)]
		[TestCase(typeof(List<IC>), typeof(IEnumerable<IA>), true)]
		[TestCase(typeof(List<IC>), typeof(IEnumerable<IB>), true)]

		[TestCase(typeof(List<D>), typeof(IEnumerable<IX>), true)]
		[TestCase(typeof(List<D>), typeof(IEnumerable<IY>), true)]
		[TestCase(typeof(List<D>), typeof(IEnumerable<IZ>), true)]
		[TestCase(typeof(List<D>), typeof(IEnumerable<A>), true)]
		[TestCase(typeof(List<D>), typeof(IEnumerable<B>), true)]
		[TestCase(typeof(List<D>), typeof(IEnumerable<C>), false)]
		public static void Inherits_Implements(Type a, Type b, bool expected) {
			Assert.AreEqual(expected, a.Inherits(b));
			Assert.AreEqual(expected, a.Implements(b));
		}

		[Test]
		[TestCase(typeof(List<A>), typeof(IEnumerable<>), typeof(IEnumerable<A>))]
		[TestCase(typeof(Dictionary<string, A>), typeof(IEnumerable<>), typeof(IEnumerable<KeyValuePair<string, A>>))]
		[TestCase(typeof(Dictionary<string, A>), typeof(IReadOnlyDictionary<,>), typeof(IReadOnlyDictionary<string, A>))]
		[TestCase(typeof(Dictionary<string, float>), (typeof(IEnumerable<>)), typeof(IEnumerable<KeyValuePair<string, float>>))]
		public static void ImplementsGeneric(Type a, Type b, Type expectedInterface) {
			Assert.IsFalse(a.Inherits(b));
			Assert.IsFalse(a.Implements(b));

			Assert.IsTrue(a.ImplementsGeneric(b, out var @interface));
			Assert.AreEqual(expectedInterface, @interface);
		}

		[Test]
		public static void Reflection_learning_test() {
			var target = typeof(IAssignable<,>).MakeGenericType(typeof(string), typeof(IDictionary<string, float>));

			Assert.IsTrue(typeof(IAssignable<string, IDictionary<string, float>>).Implements(target));
			Assert.IsTrue(typeof(IAssignable<object, Dictionary<string, float>>).Implements(target));
		}

		[Test]
		public static void PrettyName() {
			Assert.AreEqual("Int32", typeof(int).PrettyName());
			Assert.AreEqual("String", typeof(string).PrettyName());
			Assert.AreEqual("Single", typeof(float).PrettyName());
			Assert.AreEqual("DateTime[]", typeof(DateTime[]).PrettyName());
			Assert.AreEqual("Dictionary<String, IReadOnlyList<Single>>", typeof(Dictionary<string, IReadOnlyList<float>>).PrettyName());
		}
	}
}
