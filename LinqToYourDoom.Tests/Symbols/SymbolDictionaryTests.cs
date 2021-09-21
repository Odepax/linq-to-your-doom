using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Symbols {
	static class SymbolDictionaryTests {
		[Test]
		public static void Assignment_to_IShallowCloneable_and_IAssignable() {
			IShallowCloneable<IReadOnlySymbolDictionary> _1 = new SymbolDictionary();
			IShallowCloneable<ISymbolDictionary> _2 = new SymbolDictionary();
			IShallowCloneable<SymbolDictionary> _3 = new SymbolDictionary();

			IAssignable<IReadOnlySymbolDictionary, IReadOnlySymbolDictionary> _4 = new SymbolDictionary();
			IAssignable<IReadOnlySymbolDictionary, ISymbolDictionary> _5 = new SymbolDictionary();
			IAssignable<IReadOnlySymbolDictionary, SymbolDictionary> _6 = new SymbolDictionary();
			IAssignable<SymbolDictionary, IReadOnlySymbolDictionary> _7 = new SymbolDictionary();
			IAssignable<SymbolDictionary, ISymbolDictionary> _8 = new SymbolDictionary();
			IAssignable<SymbolDictionary, SymbolDictionary> _9 = new SymbolDictionary();
		}

		[Test]
		public static void AssignableSymbolDictionary() {
			var d = new SymbolDictionary();
			var r = new SymbolDictionary() as IReadOnlySymbolDictionary;
			var l = new List<IAssignable<IReadOnlySymbolDictionary, ISymbolDictionary>>();

			l.Add(d);

			ISymbolDictionary dd = l.First().Assign(r);

			Assert.AreSame(d, dd);
		}

		static readonly Symbol<string> A = new();
		static readonly Symbol<double> B = new();
		static readonly Symbol<DateTime> C = new();
		static readonly Symbol<Camel> D = new();

		sealed class Camel {}

		static readonly Camel aziz = new();
		static readonly Camel light = new();

		[Test]
		public static void Crud() {
			string? a;
			double b;
			DateTime c;
			Camel? d;

			var dictionary = new SymbolDictionary();

			// Empty
			// ----

			using (var enumerator = dictionary.GetEnumerator()) {
				Assert.IsFalse(enumerator.MoveNext());
			}

			Assert.AreEqual(0, dictionary.Count);
			Assert.IsEmpty(dictionary.Keys);
			Assert.IsFalse(dictionary.TryGet(A, out a));
			Assert.IsNull(a);

			// Adding an item
			// ----

			dictionary.Set(A, "A");

			using (var enumerator = dictionary.GetEnumerator()) {
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(A, "A"), enumerator.Current);
				Assert.IsFalse(enumerator.MoveNext());
			}

			Assert.AreEqual(1, dictionary.Count);
			Assert.AreEqual(new[] { A }, dictionary.Keys);
			Assert.IsTrue(dictionary.TryGet(A, out a));
			Assert.AreEqual("A", a);
			Assert.IsFalse(dictionary.TryGet(B, out b));
			Assert.AreEqual(default(double), b);

			// Adding more items
			// ----

			dictionary.Set(B, 3.14);
			dictionary.Set(C, new DateTime(2021, 07, 07));

			using (var enumerator = dictionary.GetEnumerator()) {
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(A, "A"), enumerator.Current);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(B, 3.14), enumerator.Current);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(C, new DateTime(2021, 07, 07)), enumerator.Current);
				Assert.IsFalse(enumerator.MoveNext());
			}

			Assert.AreEqual(3, dictionary.Count);
			Assert.AreEqual(new Symbol[] { A, B, C }, dictionary.Keys);
			Assert.IsTrue(dictionary.TryGet(A, out a));
			Assert.AreEqual("A", a);
			Assert.IsTrue(dictionary.TryGet(B, out b));
			Assert.AreEqual(3.14, b);
			Assert.IsTrue(dictionary.TryGet(C, out c));
			Assert.AreEqual(new DateTime(2021, 07, 07), c);

			// Removing items
			// ----

			dictionary.Set(D, aziz);

			Assert.IsTrue(dictionary.TryRemove(B, out b));
			Assert.AreEqual(3.14, b);
			Assert.IsTrue(dictionary.TryRemove(C, out c));
			Assert.AreEqual(new DateTime(2021, 07, 07), c);

			using (var enumerator = dictionary.GetEnumerator()) {
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(A, "A"), enumerator.Current);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(D, aziz), enumerator.Current);
				Assert.IsFalse(enumerator.MoveNext());
			}

			Assert.AreEqual(2, dictionary.Count);
			Assert.AreEqual(new Symbol[] { A, D }, dictionary.Keys);
			Assert.IsTrue(dictionary.TryGet(A, out a));
			Assert.AreEqual("A", a);
			Assert.IsFalse(dictionary.TryGet(B, out b));
			Assert.AreEqual(default(double), b);
			Assert.IsFalse(dictionary.TryGet(C, out c));
			Assert.AreEqual(default(DateTime), c);
			Assert.IsTrue(dictionary.TryGet(D, out d));
			Assert.AreSame(aziz, d);

			// Removing is idempotent
			// ----

			Assert.IsFalse(dictionary.TryRemove(B, out b));
			Assert.AreEqual(default(double), b);
			Assert.IsFalse(dictionary.TryRemove(C, out c));
			Assert.AreEqual(default(DateTime), c);

			using (var enumerator = dictionary.GetEnumerator()) {
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(A, "A"), enumerator.Current);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(D, aziz), enumerator.Current);
				Assert.IsFalse(enumerator.MoveNext());
			}

			Assert.AreEqual(2, dictionary.Count);
			Assert.AreEqual(new Symbol[] { A, D }, dictionary.Keys);
			Assert.IsTrue(dictionary.TryGet(A, out a));
			Assert.AreEqual("A", a);
			Assert.IsFalse(dictionary.TryGet(B, out b));
			Assert.AreEqual(default(double), b);
			Assert.IsFalse(dictionary.TryGet(C, out c));
			Assert.AreEqual(default(DateTime), c);
			Assert.IsTrue(dictionary.TryGet(D, out d));
			Assert.AreSame(aziz, d);

			// Clearing all items
			// ----

			dictionary.Clear();

			using (var enumerator = dictionary.GetEnumerator()) {
				Assert.IsFalse(enumerator.MoveNext());
			}

			Assert.AreEqual(0, dictionary.Count);
			Assert.IsEmpty(dictionary.Keys);
			Assert.IsFalse(dictionary.TryGet(A, out a));
			Assert.AreEqual(default(string), a);
			Assert.IsFalse(dictionary.TryGet(D, out d));
			Assert.AreSame(default(Camel), d);
		}

		[Test]
		public static void Assign() {
			var x = new SymbolDictionary();
			var y = new SymbolDictionary();

			// Assign empty
			// ----

			using (var enumerator = x.ShallowClone().Assign(y).GetEnumerator()) {
				Assert.IsFalse(enumerator.MoveNext());
			}

			// Assign other
			// ----

			x.Set(A, "A");
			y.Set(B, 3.14);

			using (var enumerator = x.ShallowClone().Assign(y).GetEnumerator()) {
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(A, "A"), enumerator.Current);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(B, 3.14), enumerator.Current);
				Assert.IsFalse(enumerator.MoveNext());
			}

			// Assign override
			// ----

			y.Set(A, "Y");

			using (var enumerator = x.ShallowClone().Assign(y, ConflictHandling.Replace).GetEnumerator()) {
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(A, "Y"), enumerator.Current);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(B, 3.14), enumerator.Current);
				Assert.IsFalse(enumerator.MoveNext());
			}

			using (var enumerator = x.ShallowClone().Assign(y, ConflictHandling.Ignore).GetEnumerator()) {
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(A, "A"), enumerator.Current);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(B, 3.14), enumerator.Current);
				Assert.IsFalse(enumerator.MoveNext());
			}

			var exception = Assert.Throws<AssignConflictException>(() => x.ShallowClone().Assign(y, ConflictHandling.Throw));
			StringAssert.IsMatch(/* lang=regex */ @"\[Symbol<String>#\d+\]", exception!.Path.ToString());

			// Assign override (class)
			// ----

			y.Clear();

			x.Set(D, aziz);
			y.Set(D, light);

			using (var enumerator = x.ShallowClone().Assign(y, ConflictHandling.Replace).GetEnumerator()) {
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(A, "A"), enumerator.Current);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(D, light), enumerator.Current);
				Assert.IsFalse(enumerator.MoveNext());
			}

			using (var enumerator = x.ShallowClone().Assign(y, ConflictHandling.Ignore).GetEnumerator()) {
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(A, "A"), enumerator.Current);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(KeyValuePair.Create(D, aziz), enumerator.Current);
				Assert.IsFalse(enumerator.MoveNext());
			}

			exception = Assert.Throws<AssignConflictException>(() => x.ShallowClone().Assign(y, ConflictHandling.Throw));
			StringAssert.IsMatch(/* lang=regex */ @"\[Symbol<Camel>#\d+\]", exception!.Path.ToString());
		}

		[Test]
		public static void AssignCustomCallbacks() {
			var a = new SymbolDictionary();

			a.Set(A, "A");
			a.Set(B, 3.14d);
			a.Set(C, new DateTime(2021, 03, 21));
			a.Set(D, aziz);

			var b = new SymbolDictionary();

			b.Set(A, "B");
			b.Set(C, new DateTime(2022, 01, 12));
			b.Set(D, light);

			var c = a.Assign(b, ConflictHandling.Merge,
				// Here _conflictHandling is the constant ConflictHandling.Merge, so it's safe to ignore the parameter.
				// Same for _symbol, it's not needed here.
				AssignCallbackDictionary.New(4)
					.Add<string>((_symbol, a, b, _conflictHandling) => string.Concat(a, b))
					.Add<double>((_symbol, a, b, _conflictHandling) => a + b)
					.Add<DateTime>((_symbol, a, b, _conflictHandling) => MathD.Max(a, b))
					.Add<Camel>((_symbol, a, b, _conflictHandling) => a == aziz ? a : b)
			);

			Assert.AreEqual(new[] {
				new KeyValuePair<Symbol, object>(A, "AB"),
				new KeyValuePair<Symbol, object>(B, 3.14d),
				new KeyValuePair<Symbol, object>(C, new DateTime(2022, 01, 12)),
				new KeyValuePair<Symbol, object>(D, aziz)
			}, c);
		}
	}
}
