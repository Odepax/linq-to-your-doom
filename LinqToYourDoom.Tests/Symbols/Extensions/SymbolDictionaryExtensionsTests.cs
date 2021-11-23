using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Symbols.Extensions;

static class SymbolDictionaryExtensionsTests {
	sealed class Camel {
		public readonly int Id;
		public Camel(int id) => Id = id;
	}

	static readonly Symbol<int> INT = new();
	static readonly Symbol<int> ZRO = new();
	static readonly Symbol<Guid> VAL = new();
	static readonly Symbol<Camel> REF = new();
	static readonly Symbol<Camel> NUL = new();
	static readonly Symbol<Sample> SAM = new();
	static readonly Symbol<Sample> SAN = new();

	[Test]
	public static void SymbolDictionary() {
		var dictionary = new SymbolDictionary();

		Guid out_VAL;
		Camel? out_REF;
		Camel? out_NUL;

		Assert.IsFalse(dictionary.TryGet(VAL, out out_VAL));
		Assert.IsFalse(dictionary.TryGet(REF, out out_REF));
		Assert.IsFalse(dictionary.TryGet(NUL, out out_NUL));
		Assert.AreEqual(Guid.Empty, out_VAL);
		Assert.IsNull(out_REF);
		Assert.IsNull(out_NUL);

		dictionary.Set(VAL, Guid.NewGuid());
		dictionary.Set(REF, new Camel(42));

		Assert.IsTrue(dictionary.TryGet(VAL, out out_VAL));
		Assert.IsTrue(dictionary.TryGet(REF, out out_REF));
		Assert.IsFalse(dictionary.TryGet(NUL, out out_NUL));
		Assert.AreNotEqual(Guid.Empty, out_VAL);
		Assert.AreEqual(42, out_REF!.Id);
		Assert.IsNull(out_NUL);

		Assert.IsTrue(dictionary.TryRemove(REF, out out_REF));
		Assert.AreEqual(42, out_REF!.Id);

		Assert.IsFalse(dictionary.TryRemove(REF, out out_REF));
		Assert.IsNull(out_REF);

		Assert.IsFalse(dictionary.TryGet(REF, out out_REF));
		Assert.IsNull(out_REF);

		dictionary.Clear();

		Assert.IsFalse(dictionary.TryGet(VAL, out out_VAL));
		Assert.IsFalse(dictionary.TryGet(REF, out out_REF));
		Assert.IsFalse(dictionary.TryGet(NUL, out out_NUL));
		Assert.AreEqual(Guid.Empty, out_VAL);
		Assert.IsNull(out_REF);
		Assert.IsNull(out_NUL);
	}

	[Test]
	public static void Get() {
		var dictionary = new SymbolDictionary();

		dictionary.Set(REF, new Camel(42));

		Assert.AreEqual(42, dictionary.Get(REF).Id);
		Assert.Throws<KeyNotFoundException>(() => dictionary.Get(NUL));
	}

	[Test]
	public static void Set() {
		var dictionary = new SymbolDictionary();

		dictionary.Set(INT, 1);
		dictionary.SetDefault(INT, 2);
		dictionary.TryGet(INT, out var i);

		Assert.AreEqual(1, i);

		dictionary.Set(ZRO, 0);
		dictionary.SetDefault(ZRO, 3);
		dictionary.TryGet(ZRO, out i);

		Assert.AreEqual(0, i);

		Assert.Throws<ArgumentException>(() => dictionary.Add(INT, 2));
	}

	[Test]
	public static void GetOrDefault_factory() {
		var invokeCount = 0;
		var dictionary = new SymbolDictionary();

		dictionary.Set(INT, 2048);

		Assert.AreEqual(2048, dictionary.GetOrDefault(INT, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(0, invokeCount);

		Assert.AreEqual(100, dictionary.GetOrDefault(ZRO, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(1, invokeCount);
	}

	[Test]
	public static void GetOrSet_value() {
		var dictionary = new SymbolDictionary();

		dictionary.Set(INT, 2048);

		Assert.AreEqual(2048, dictionary.GetOrSet(INT, 100));
		Assert.AreEqual(2048, dictionary.Get(INT));

		Assert.AreEqual(100, dictionary.GetOrSet(ZRO, 100));
		Assert.AreEqual(100, dictionary.Get(ZRO));
	}

	[Test]
	public static void GetOrSet_factory() {
		var invokeCount = 0;
		var dictionary = new SymbolDictionary();

		dictionary.Set(INT, 2048);

		Assert.AreEqual(2048, dictionary.GetOrSet(INT, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(0, invokeCount);
		Assert.AreEqual(2048, dictionary.Get(INT));

		Assert.AreEqual(100, dictionary.GetOrSet(ZRO, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(1, invokeCount);
		Assert.AreEqual(100, dictionary.Get(ZRO));
	}

	class Sample {
		public int Value;
	}

	[Test]
	public static void GetOrSet_action() {
		var invokeCount = 0;
		var dictionary = new SymbolDictionary();

		dictionary.Set(SAM, new Sample { Value = 2048 });

		Assert.AreEqual(2048, dictionary.GetOrSet(SAM, sample => { ++invokeCount; sample.Value = 100; }).Value);
		Assert.AreEqual(0, invokeCount);
		Assert.AreEqual(2048, dictionary.Get(SAM).Value);

		Assert.AreEqual(100, dictionary.GetOrSet(SAN, sample => { ++invokeCount; sample.Value = 100; }).Value);
		Assert.AreEqual(1, invokeCount);
		Assert.AreEqual(100, dictionary.Get(SAN).Value);
	}
}
