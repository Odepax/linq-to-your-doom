using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Symbols {
	static class InnerTypedDictionaryTests {
		static readonly Symbol<string> A = new();
		static readonly Symbol<double> B = new();
		static readonly Symbol<DateTime> C = new();
		static readonly Symbol<Camel> D = new();

		sealed class Camel {}

		static readonly Camel aziz = new();
		static readonly Camel light = new();

		[Test]
		public static void Core_CRUD_methods() {
			string? a;
			double b;
			DateTime c;
			Camel? d;

			var dictionary = InnerTypedDictionary<Symbol, object>.New();

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
			var dictionary = InnerTypedDictionary<Symbol, object>.New();

			Guid out_VAL;
			Camel2? out_REF;
			Camel2? out_NUL;

			Assert.IsFalse(dictionary.TryGet(VAL, out out_VAL));
			Assert.IsFalse(dictionary.TryGet(REF, out out_REF));
			Assert.IsFalse(dictionary.TryGet(NUL, out out_NUL));
			Assert.AreEqual(Guid.Empty, out_VAL);
			Assert.IsNull(out_REF);
			Assert.IsNull(out_NUL);

			dictionary.Set(VAL, Guid.NewGuid());
			dictionary.Set(REF, new Camel2(42));

			Assert.IsTrue(dictionary.TryGet(VAL, out out_VAL));
			Assert.Throws<InvalidCastException>(() => dictionary.TryGet<Vector2>(VAL, out _));
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

		class Sample3 : IDisposable {
			public bool IsDisposed = false;
			public void Dispose() => IsDisposed = true;
		}

		[Test]
		public static void Set_Remove_Clear_AndDispose() {
			var a = new Sample3();
			var b = new Sample3();
			var c = new Sample3();
			var d = new Sample3();

			var dictionary = InnerTypedDictionary<int, object>.New();

			dictionary.Set(1, a);
			dictionary.Set(2, b);
			dictionary.Set(3, c);

			dictionary.SetAndDispose(1, d);

			Assert.AreEqual(new[] { d, b, c }, dictionary.Values);
			Assert.IsTrue(a.IsDisposed);
			Assert.IsFalse(b.IsDisposed);
			Assert.IsFalse(c.IsDisposed);
			Assert.IsFalse(d.IsDisposed);

			dictionary.RemoveAndDispose(2);

			Assert.AreEqual(new[] { d, c }, dictionary.Values);
			Assert.IsTrue(a.IsDisposed);
			Assert.IsTrue(b.IsDisposed);
			Assert.IsFalse(c.IsDisposed);
			Assert.IsFalse(d.IsDisposed);

			dictionary.ClearAndDispose();

			Assert.IsEmpty(dictionary);
			Assert.IsTrue(a.IsDisposed);
			Assert.IsTrue(b.IsDisposed);
			Assert.IsTrue(c.IsDisposed);
			Assert.IsTrue(d.IsDisposed);
		}

		[Test]
		public static void Get() {
			var dictionary = InnerTypedDictionary<Symbol, object>.New();

			dictionary.Set(REF, new Camel2(42));

			Assert.AreEqual(42, dictionary.Get<Camel2>(REF).Id);
			Assert.AreSame(dictionary.Get<Camel2>(REF), dictionary.Get<object>(REF));
			Assert.Throws<KeyNotFoundException>(() => dictionary.Get<Camel2>(NUL));
		}

		[Test]
		public static void Set() {
			var dictionary = InnerTypedDictionary<Symbol, object>.New();

			dictionary.Set(INT, 1);
			dictionary.TrySet(INT, 2);
			dictionary.TryGet<int>(INT, out var i);

			Assert.AreEqual(1, i);

			dictionary.Set(ZRO, 0);
			dictionary.TrySet(ZRO, 3);
			dictionary.TryGet(ZRO, out i);

			Assert.AreEqual(0, i);

			Assert.Throws<ArgumentException>(() => dictionary.Add(INT, 2));
		}

		[Test]
		public static void GetOrDefault_factory() {
			var invokeCount = 0;
			var dictionary = InnerTypedDictionary<Symbol, object>.New();

			dictionary.Set(INT, 2048);

			Assert.AreEqual(2048, dictionary.GetOrDefault(INT, () => { ++invokeCount; return 100; }));
			Assert.AreEqual(0, invokeCount);

			Assert.AreEqual(100, dictionary.GetOrDefault(ZRO, () => { ++invokeCount; return 100; }));
			Assert.AreEqual(1, invokeCount);
		}

		[Test]
		public static void GetOrSet_value() {
			var dictionary = InnerTypedDictionary<Symbol, object>.New();

			dictionary.Set(INT, 2048);

			Assert.AreEqual(2048, dictionary.GetOrSet(INT, 100));
			Assert.AreEqual(2048, dictionary.Get<int>(INT));

			Assert.AreEqual(100, dictionary.GetOrSet(ZRO, 100));
			Assert.AreEqual(100, dictionary.Get<int>(ZRO));
		}

		[Test]
		public static void GetOrSet_factory() {
			var invokeCount = 0;
			var dictionary = InnerTypedDictionary<Symbol, object>.New();

			dictionary.Set(INT, 2048);

			Assert.AreEqual(2048, dictionary.GetOrSet(INT, () => { ++invokeCount; return 100; }));
			Assert.AreEqual(0, invokeCount);
			Assert.AreEqual(2048, dictionary.Get<int>(INT));

			Assert.AreEqual(100, dictionary.GetOrSet(ZRO, () => { ++invokeCount; return 100; }));
			Assert.AreEqual(1, invokeCount);
			Assert.AreEqual(100, dictionary.Get<int>(ZRO));
		}

		class Sample2 {
			public int Value;
		}

		[Test]
		public static void GetOrSet_action() {
			var invokeCount = 0;
			var dictionary = InnerTypedDictionary<Symbol, object>.New();

			dictionary.Set(SAM, new Sample2 { Value = 2048 });

			Assert.AreEqual(2048, dictionary.GetOrSet<Sample2>(SAM, sample => { ++invokeCount; sample.Value = 100; }).Value);
			Assert.AreEqual(0, invokeCount);
			Assert.AreEqual(2048, dictionary.Get<Sample2>(SAM).Value);

			Assert.AreEqual(100, dictionary.GetOrSet<Sample2>(SAN, sample => { ++invokeCount; sample.Value = 100; }).Value);
			Assert.AreEqual(1, invokeCount);
			Assert.AreEqual(100, dictionary.Get<Sample2>(SAN).Value);
		}

		// Assign
		// ----

		[Test]
		public static void Assign_legacy() {
			var x = InnerTypedDictionary<Symbol, object>.New();
			var y = InnerTypedDictionary<Symbol, object>.New();

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

		sealed class Sample : IAssignable<Sample, Sample> {
			public int Id { get; set; }
			public Sample(int id) => Id = id;

			public Sample Assign(Sample other, ConflictHandling conflictHandling = default) {
				Id = Id.Assign(other.Id, conflictHandling, MathD.Avg, nameof(Id));

				return this;
			}

			public override bool Equals(object? obj) => obj is Sample other && other.Id == Id;
			public override int GetHashCode() => base.GetHashCode();
		}

		[Test]
		[TestCase(ConflictHandling.Replace, 15, "B", 40, "https://b.com")]
		[TestCase(ConflictHandling.Ignore, 5, "A", 30, "https://a.com")]
		[TestCase(ConflictHandling.Merge, 10, "AB", 30, "https://a.com")]
		[TestCase(ConflictHandling.Throw, 0, null, 0, null)]
		public static void Assign(ConflictHandling conflictHandling, int expectedId, string? expectedString, int expectedInt, string? expectedUri) {
			var a = InnerTypedDictionary<string, object?>.New();

			a.Set("Sample_Null0", null as Sample);
			a.Set("Sample_Null1", null as Sample);
			a.Set("Sample_Null2", new Sample(1));
			a.Set("Sample_Value", new Sample(2));
			a.Set("Sample_Same", new Sample(42));
			a.Set("Sample_Different", new Sample(5));
			a.Set("Samples", new Dictionary<string, Sample?> {
				["Null0"] = null,
				["Null1"] = null,
				["Null2"] = new Sample(1),
				["Value"] = new Sample(2),
				["Same"] = new Sample(42),
				["Different"] = new Sample(5)
			});

			a.Set("String_Null0", null as string);
			a.Set("String_Null1", null as string);
			a.Set("String_Null2", "2");
			a.Set("String_Value", "V");
			a.Set("String_Same", "S");
			a.Set("String_Different", "A");
			a.Set("Strings", new Dictionary<string, string?> {
				["Null0"] = null,
				["Null1"] = null,
				["Null2"] = "2",
				["Value"] = "V",
				["Same"] = "S",
				["Different"] = "A"
			});

			a.Set("Int_Null0", 0);
			a.Set("Int_Null1", 0);
			a.Set("Int_Null2", 2);
			a.Set("Int_Value", 10);
			a.Set("Int_Same", 20);
			a.Set("Int_Different", 30);
			a.Set("Ints", new Dictionary<string, int> {
				["Null0"] = 0,
				["Null1"] = 0,
				["Null2"] = 2,
				["Value"] = 10,
				["Same"] = 20,
				["Different"] = 30
			});

			a.Set("Uri_Null0", null as Uri);
			a.Set("Uri_Null1", null as Uri);
			a.Set("Uri_Null2", new Uri("https://n2.com"));
			a.Set("Uri_Value", new Uri("https://v.com"));
			a.Set("Uri_Same", new Uri("https://s.com"));
			a.Set("Uri_Different", new Uri("https://a.com"));
			a.Set("Uris", new Dictionary<string, Uri?> {
				["Null0"] = null,
				["Null1"] = null,
				["Null2"] = new Uri("https://n2.com"),
				["Value"] = new Uri("https://v.com"),
				["Same"] = new Uri("https://s.com"),
				["Different"] = new Uri("https://a.com")
			});

			var b = InnerTypedDictionary<string, object?>.New();

			b.Set("Sample_Null1", null as Sample);
			b.Set("Sample_Null2", new Sample(1));
			b.Set("Sample_Null3", null as Sample);
			b.Set("Sample_Same", new Sample(42));
			b.Set("Sample_Different", new Sample(15));
			b.Set("Sample_New", new Sample(666));
			b.Set("Samples", new Dictionary<string, Sample?> {
				["Null1"] = null,
				["Null2"] = new Sample(1),
				["Null3"] = null,
				["Same"] = new Sample(42),
				["Different"] = new Sample(15),
				["New"] = new Sample(666)
			});

			b.Set("String_Null1", null as string);
			b.Set("String_Null2", "2");
			b.Set("String_Null3", null as string);
			b.Set("String_Same", "S");
			b.Set("String_Different", "B");
			b.Set("String_New", "N");
			b.Set("Strings", new Dictionary<string, string?> {
				["Null1"] = null,
				["Null2"] = "2",
				["Null3"] = null,
				["Same"] = "S",
				["Different"] = "B",
				["New"] = "N"
			});

			b.Set("Int_Null1", 0);
			b.Set("Int_Null2", 2);
			b.Set("Int_Null3", 0);
			b.Set("Int_Same", 20);
			b.Set("Int_Different", 40);
			b.Set("Int_New", 50);
			b.Set("Ints", new Dictionary<string, int> {
				["Null1"] = 0,
				["Null2"] = 2,
				["Null3"] = 0,
				["Same"] = 20,
				["Different"] = 40,
				["New"] = 50
			});

			b.Set("Uri_Null1", null as Uri);
			b.Set("Uri_Null2", new Uri("https://n2.com"));
			b.Set("Uri_Null3", null as Uri);
			b.Set("Uri_Same", new Uri("https://s.com"));
			b.Set("Uri_Different", new Uri("https://b.com"));
			b.Set("Uri_New", new Uri("https://n.com"));
			b.Set("Uris", new Dictionary<string, Uri?> {
				["Null1"] = null,
				["Null2"] = new Uri("https://n2.com"),
				["Null3"] = null,
				["Same"] = new Uri("https://s.com"),
				["Different"] = new Uri("https://b.com"),
				["New"] = new Uri("https://n.com")
			});

			if (conflictHandling == ConflictHandling.Throw) {
				foreach (var (expectedPath, key, subKey) in new[] {
					("[Sample_Different].Id", "Sample_Different", null),
					("[Samples][Different].Id", "Samples", "Different"),
					("[String_Different]", "String_Different", null),
					("[Strings][Different]", "Strings", "Different"),
					("[Int_Different]", "Int_Different", null),
					("[Ints][Different]", "Ints", "Different"),
					("[Uri_Different]", "Uri_Different", null),
					("[Uris][Different]", "Uris", "Different")
				}) {
					var exception = Assert.Throws<AssignConflictException>(() => a.Assign(b, conflictHandling));
					Assert.AreEqual(expectedPath, exception.Path);

					if (subKey == null)
						b.TryRemove<object>(key, out _);

					else {
						b.TryGet(key, out IDictionary? d);
						d!.Remove(subKey);
					}
				}

				return;
			}

			if (conflictHandling == ConflictHandling.Merge) {
				b.TryRemove<object>("Sample_Different", out _);
				b.TryRemove<object>("String_Different", out _);

				b.TryGet("Samples", out IDictionary? samples);
				b.TryGet("Strings", out IDictionary? strings);

				samples!.Remove("Different");
				strings!.Remove("Different");

				foreach (var (key, subKey) in new[] {
					("Int_Different", null),
					("Ints", "Different"),
					("Uri_Different", null),
					("Uris", "Different")
				}) {
					Assert.Throws<InvalidOperationException>(() => a.ShallowClone().Assign(b, conflictHandling));

					if (subKey == null)
						b.TryRemove<object>(key, out _);

					else {
						b.TryGet(key, out IDictionary? d);
						d!.Remove(subKey);
					}
				}

				b.Set("Sample_Different", new Sample(15));
				b.Set("String_Different", "B");

				samples.Add("Different", new Sample(15));
				strings.Add("Different", "B");
			}

			a.Assign(b, conflictHandling);

			a.TryGet<Sample>("Sample_Null0", out var out_Sample_Null0);
			a.TryGet<Sample>("Sample_Null1", out var out_Sample_Null1);
			a.TryGet<Sample>("Sample_Null2", out var out_Sample_Null2);
			a.TryGet<Sample>("Sample_Null3", out var out_Sample_Null3);
			a.TryGet<Sample>("Sample_Value", out var out_Sample_Value);
			a.TryGet<Sample>("Sample_Same", out var out_Sample_Same);
			a.TryGet<Sample>("Sample_Different", out var out_Sample_Different);
			a.TryGet<Sample>("Sample_New", out var out_Sample_New);
			a.TryGet<Dictionary<string, Sample?>>("Samples", out var out_Samples);

			Assert.IsNull(out_Sample_Null0);
			Assert.IsNull(out_Sample_Null1);
			Assert.AreEqual(new Sample(1), out_Sample_Null2);
			Assert.IsNull(out_Sample_Null3);
			Assert.AreEqual(new Sample(2), out_Sample_Value);
			Assert.AreEqual(new Sample(42), out_Sample_Same);
			Assert.AreEqual(new Sample(expectedId), out_Sample_Different);
			Assert.AreEqual(new Sample(666), out_Sample_New);
			CollectionAssert.AreEquivalent(new Dictionary<string, Sample?> {
				["Null0"] = null,
				["Null1"] = null,
				["Null2"] = new Sample(1),
				["Null3"] = null,
				["Value"] = new Sample(2),
				["Same"] = new Sample(42),
				["Different"] = new Sample(expectedId),
				["New"] = new Sample(666)
			}, out_Samples);

			a.TryGet<string>("String_Null0", out var out_String_Null0);
			a.TryGet<string>("String_Null1", out var out_String_Null1);
			a.TryGet<string>("String_Null2", out var out_String_Null2);
			a.TryGet<string>("String_Null3", out var out_String_Null3);
			a.TryGet<string>("String_Value", out var out_String_Value);
			a.TryGet<string>("String_Same", out var out_String_Same);
			a.TryGet<string>("String_Different", out var out_String_Different);
			a.TryGet<string>("String_New", out var out_String_New);
			a.TryGet<Dictionary<string, string?>>("Strings", out var out_Strings);

			Assert.IsNull(out_String_Null0);
			Assert.IsNull(out_String_Null1);
			Assert.AreEqual("2", out_String_Null2);
			Assert.IsNull(out_String_Null3);
			Assert.AreEqual("V", out_String_Value);
			Assert.AreEqual("S", out_String_Same);
			Assert.AreEqual(expectedString, out_String_Different);
			Assert.AreEqual("N", out_String_New);
			CollectionAssert.AreEquivalent(new Dictionary<string, string?> {
				["Null0"] = null,
				["Null1"] = null,
				["Null2"] = "2",
				["Null3"] = null,
				["Value"] = "V",
				["Same"] = "S",
				["Different"] = expectedString,
				["New"] = "N"
			}, out_Strings);

			a.TryGet<int>("Int_Null0", out var out_Int_Null0);
			a.TryGet<int>("Int_Null1", out var out_Int_Null1);
			a.TryGet<int>("Int_Null2", out var out_Int_Null2);
			a.TryGet<int>("Int_Null3", out var out_Int_Null3);
			a.TryGet<int>("Int_Value", out var out_Int_Value);
			a.TryGet<int>("Int_Same", out var out_Int_Same);
			a.TryGet<int>("Int_Different", out var out_Int_Different);
			a.TryGet<int>("Int_New", out var out_Int_New);
			a.TryGet<Dictionary<string, int>>("Ints", out var out_Ints);

			Assert.AreEqual(0, out_Int_Null0);
			Assert.AreEqual(0, out_Int_Null1);
			Assert.AreEqual(2, out_Int_Null2);
			Assert.AreEqual(0, out_Int_Null3);
			Assert.AreEqual(10, out_Int_Value);
			Assert.AreEqual(20, out_Int_Same);
			Assert.AreEqual(expectedInt, out_Int_Different);
			Assert.AreEqual(50, out_Int_New);
			CollectionAssert.AreEquivalent(new Dictionary<string, int> {
				["Null0"] = 0,
				["Null1"] = 0,
				["Null2"] = 2,
				["Null3"] = 0,
				["Value"] = 10,
				["Same"] = 20,
				["Different"] = expectedInt,
				["New"] = 50
			}, out_Ints);

			a.TryGet<Uri>("Uri_Null0", out var out_Uri_Null0);
			a.TryGet<Uri>("Uri_Null1", out var out_Uri_Null1);
			a.TryGet<Uri>("Uri_Null2", out var out_Uri_Null2);
			a.TryGet<Uri>("Uri_Null3", out var out_Uri_Null3);
			a.TryGet<Uri>("Uri_Value", out var out_Uri_Value);
			a.TryGet<Uri>("Uri_Same", out var out_Uri_Same);
			a.TryGet<Uri>("Uri_Different", out var out_Uri_Different);
			a.TryGet<Uri>("Uri_New", out var out_Uri_New);
			a.TryGet<Dictionary<string, Uri?>>("Uris", out var out_Uris);

			var expectedUri_uri = expectedUri is null ? null : new Uri(expectedUri);

			Assert.IsNull(out_Uri_Null0);
			Assert.IsNull(out_Uri_Null1);
			Assert.AreEqual(new Uri("https://n2.com"), out_Uri_Null2);
			Assert.IsNull(out_Uri_Null3);
			Assert.AreEqual(new Uri("https://v.com"), out_Uri_Value);
			Assert.AreEqual(new Uri("https://s.com"), out_Uri_Same);
			Assert.AreEqual(expectedUri_uri, out_Uri_Different);
			Assert.AreEqual(new Uri("https://n.com"), out_Uri_New);
			CollectionAssert.AreEquivalent(new Dictionary<string, Uri?> {
				["Null0"] = null,
				["Null1"] = null,
				["Null2"] = new Uri("https://n2.com"),
				["Null3"] = null,
				["Value"] = new Uri("https://v.com"),
				["Same"] = new Uri("https://s.com"),
				["Different"] = expectedUri_uri,
				["New"] = new Uri("https://n.com")
			}, out_Uris);
		}
	}
}
