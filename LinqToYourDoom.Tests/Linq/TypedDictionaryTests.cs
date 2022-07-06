using System;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Linq;

static class TypedDictionaryTests {
	static readonly Symbol<string> A = new();
	static readonly Symbol<double> B = new();
	static readonly Symbol<DateTime> C = new();
	static readonly Symbol<Camel> D = new();

	sealed class Camel {}

	static readonly Camel aziz = new();

	[Test]
	public static void Core_CRUD_methods() {
		string? a;
		double b;
		DateTime c;
		Camel? d;

		var dictionary = new Dictionary<Symbol, object>();
		var typedDictionary = dictionary.AsTyped();

		// Empty
		// ----

		using (var enumerator = dictionary.GetEnumerator()) {
			Assert.IsFalse(enumerator.MoveNext());
		}

		Assert.AreEqual(0, dictionary.Count);
		Assert.IsEmpty(dictionary.Keys);
		Assert.IsFalse(typedDictionary.TryGet(A, out a));
		Assert.IsNull(a);

		// Adding an item
		// ----

		dictionary[A] = "A";

		using (var enumerator = dictionary.GetEnumerator()) {
			Assert.IsTrue(enumerator.MoveNext());
			Assert.AreEqual(KeyValuePair.Create(A, "A"), enumerator.Current);
			Assert.IsFalse(enumerator.MoveNext());
		}

		Assert.AreEqual(1, dictionary.Count);
		Assert.AreEqual(new[] { A }, dictionary.Keys);
		Assert.IsTrue(typedDictionary.TryGet(A, out a));
		Assert.AreEqual("A", a);
		Assert.IsFalse(typedDictionary.TryGet(B, out b));
		Assert.AreEqual(default(double), b);

		// Adding more items
		// ----

		dictionary[B] = 3.14;
		dictionary[C] = new DateTime(2021, 07, 07);

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
		Assert.IsTrue(typedDictionary.TryGet(A, out a));
		Assert.AreEqual("A", a);
		Assert.IsTrue(typedDictionary.TryGet(B, out b));
		Assert.AreEqual(3.14, b);
		Assert.IsTrue(typedDictionary.TryGet(C, out c));
		Assert.AreEqual(new DateTime(2021, 07, 07), c);

		// Removing items
		// ----

		dictionary[D] = aziz;

		Assert.IsTrue(typedDictionary.TryRemove(B, out b));
		Assert.AreEqual(3.14, b);
		Assert.IsTrue(typedDictionary.TryRemove(C, out c));
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
		Assert.IsTrue(typedDictionary.TryGet(A, out a));
		Assert.AreEqual("A", a);
		Assert.IsFalse(typedDictionary.TryGet(B, out b));
		Assert.AreEqual(default(double), b);
		Assert.IsFalse(typedDictionary.TryGet(C, out c));
		Assert.AreEqual(default(DateTime), c);
		Assert.IsTrue(typedDictionary.TryGet(D, out d));
		Assert.AreSame(aziz, d);

		// Removing is idempotent
		// ----

		Assert.IsFalse(typedDictionary.TryRemove(B, out b));
		Assert.AreEqual(default(double), b);
		Assert.IsFalse(typedDictionary.TryRemove(C, out c));
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
		Assert.IsTrue(typedDictionary.TryGet(A, out a));
		Assert.AreEqual("A", a);
		Assert.IsFalse(typedDictionary.TryGet(B, out b));
		Assert.AreEqual(default(double), b);
		Assert.IsFalse(typedDictionary.TryGet(C, out c));
		Assert.AreEqual(default(DateTime), c);
		Assert.IsTrue(typedDictionary.TryGet(D, out d));
		Assert.AreSame(aziz, d);

		// Clearing all items
		// ----

		dictionary.Clear();

		using (var enumerator = dictionary.GetEnumerator()) {
			Assert.IsFalse(enumerator.MoveNext());
		}

		Assert.AreEqual(0, dictionary.Count);
		Assert.IsEmpty(dictionary.Keys);
		Assert.IsFalse(typedDictionary.TryGet(A, out a));
		Assert.AreEqual(default(string), a);
		Assert.IsFalse(typedDictionary.TryGet(D, out d));
		Assert.AreSame(default(Camel), d);
	}

	sealed class Camel2 {
		public readonly int Id;
		public Camel2(int id) => Id = id;
	}

	static readonly Symbol<int> INT = new();
	static readonly Symbol<int> ZRO = new();
	static readonly Symbol<Guid> VAL = new();
	static readonly Symbol<Camel2> REF = new();
	static readonly Symbol<Camel2> NUL = new();
	static readonly Symbol<Sample2> SAM = new();
	static readonly Symbol<Sample2> SAN = new();

	[Test]
	public static void Extra_methods() {
		var dictionary = new Dictionary<Symbol, object>();
		var typedDictionary = dictionary.AsTyped();

		Guid out_VAL;
		Camel2? out_REF;
		Camel2? out_NUL;

		Assert.IsFalse(typedDictionary.TryGet(VAL, out out_VAL));
		Assert.IsFalse(typedDictionary.TryGet(REF, out out_REF));
		Assert.IsFalse(typedDictionary.TryGet(NUL, out out_NUL));
		Assert.AreEqual(Guid.Empty, out_VAL);
		Assert.IsNull(out_REF);
		Assert.IsNull(out_NUL);

		dictionary[VAL] = Guid.NewGuid();
		dictionary[REF] = new Camel2(42);

		Assert.IsTrue(typedDictionary.TryGet(VAL, out out_VAL));
		Assert.Throws<InvalidCastException>(() => dictionary.AsTyped().TryGet<Vector2>(VAL, out _));
		Assert.IsTrue(typedDictionary.TryGet(REF, out out_REF));
		Assert.IsFalse(typedDictionary.TryGet(NUL, out out_NUL));
		Assert.AreNotEqual(Guid.Empty, out_VAL);
		Assert.AreEqual(42, out_REF!.Id);
		Assert.IsNull(out_NUL);

		Assert.IsTrue(typedDictionary.TryRemove(REF, out out_REF));
		Assert.AreEqual(42, out_REF!.Id);

		Assert.IsFalse(typedDictionary.TryRemove(REF, out out_REF));
		Assert.IsNull(out_REF);

		Assert.IsFalse(typedDictionary.TryGet(REF, out out_REF));
		Assert.IsNull(out_REF);

		dictionary.Clear();

		Assert.IsFalse(typedDictionary.TryGet(VAL, out out_VAL));
		Assert.IsFalse(typedDictionary.TryGet(REF, out out_REF));
		Assert.IsFalse(typedDictionary.TryGet(NUL, out out_NUL));
		Assert.AreEqual(Guid.Empty, out_VAL);
		Assert.IsNull(out_REF);
		Assert.IsNull(out_NUL);
	}

	[Test]
	public static void Get() {
		var dictionary = new Dictionary<Symbol, object>();
		var typedDictionary = dictionary.AsTyped();

		dictionary[REF] = new Camel2(42);

		Assert.AreEqual(42, typedDictionary.Get<Camel2>(REF).Id);
		Assert.AreSame(typedDictionary.Get<Camel2>(REF), typedDictionary.Get<object>(REF));
		Assert.Throws<KeyNotFoundException>(() => dictionary.AsTyped().Get<Camel2>(NUL));
	}

	[Test]
	public static void Set() {
		var dictionary = new Dictionary<Symbol, object>();
		var typedDictionary = dictionary.AsTyped();

		dictionary[INT] = 1;
		dictionary.TryAdd(INT, 2);
		typedDictionary.TryGet<int>(INT, out var i);

		Assert.AreEqual(1, i);

		dictionary[ZRO] = 0;
		dictionary.TryAdd(ZRO, 3);
		typedDictionary.TryGet(ZRO, out i);

		Assert.AreEqual(0, i);

		Assert.Throws<ArgumentException>(() => dictionary.Add(INT, 2));
	}

	[Test]
	public static void GetOrDefault_factory() {
		var invokeCount = 0;
		var dictionary = new Dictionary<Symbol, object>();
		var typedDictionary = dictionary.AsTyped();

		dictionary[INT] = 2048;

		Assert.AreEqual(2048, typedDictionary.GetOrDefault(INT, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(0, invokeCount);

		Assert.AreEqual(100, typedDictionary.GetOrDefault(ZRO, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(1, invokeCount);
	}

	[Test]
	public static void GetOrSet_value() {
		var dictionary = new Dictionary<Symbol, object>();
		var typedDictionary = dictionary.AsTyped();

		dictionary[INT] = 2048;

		Assert.AreEqual(2048, typedDictionary.GetOrSet(INT, 100));
		Assert.AreEqual(2048, typedDictionary.Get<int>(INT));

		Assert.AreEqual(100, typedDictionary.GetOrSet(ZRO, 100));
		Assert.AreEqual(100, typedDictionary.Get<int>(ZRO));
	}

	[Test]
	public static void GetOrSet_factory() {
		var invokeCount = 0;
		var dictionary = new Dictionary<Symbol, object>();
		var typedDictionary = dictionary.AsTyped();

		dictionary[INT] = 2048;

		Assert.AreEqual(2048, typedDictionary.GetOrSet(INT, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(0, invokeCount);
		Assert.AreEqual(2048, typedDictionary.Get<int>(INT));

		Assert.AreEqual(100, typedDictionary.GetOrSet(ZRO, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(1, invokeCount);
		Assert.AreEqual(100, typedDictionary.Get<int>(ZRO));
	}

	class Sample2 {
		public int Value;
	}

	[Test]
	public static void GetOrSet_action() {
		var invokeCount = 0;
		var dictionary = new Dictionary<Symbol, object>();
		var typedDictionary = dictionary.AsTyped();

		dictionary[SAM] = new Sample2 { Value = 2048 };

		Assert.AreEqual(2048, typedDictionary.GetOrSet<Sample2>(SAM, sample => { ++invokeCount; sample.Value = 100; }).Value);
		Assert.AreEqual(0, invokeCount);
		Assert.AreEqual(2048, typedDictionary.Get<Sample2>(SAM).Value);

		Assert.AreEqual(100, typedDictionary.GetOrSet<Sample2>(SAN, sample => { ++invokeCount; sample.Value = 100; }).Value);
		Assert.AreEqual(1, invokeCount);
		Assert.AreEqual(100, typedDictionary.Get<Sample2>(SAN).Value);
	}
}
