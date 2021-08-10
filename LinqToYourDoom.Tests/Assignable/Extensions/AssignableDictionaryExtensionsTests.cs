using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Assignable.Extensions {
	static class AssignableDictionaryExtensionsTests {
		sealed class Level1 : IAssignable<Level1, Level1> {
			public Dictionary<int, Level2> Subs { get; } = new();
			public Level2 this[int i] { set => Subs[i] = value; }

			public Level1 Assign(Level1 other, ConflictHandling conflictHandling = default) {
				Subs.Assign(other.Subs, conflictHandling, nameof(Subs));

				return this;
			}
		}

		sealed class Level2 : IAssignable<Level2, Level2> {
			public int Id { get; set; }
			public Level2(int id) => Id = id;

			public Level2 Assign(Level2 other, ConflictHandling conflictHandling = default) {
				Id = Id.Assign(other.Id, conflictHandling, MathD.Avg, nameof(Id));

				return this;
			}
		}

		[Test]
		[TestCase(ConflictHandling.Replace, 15)]
		[TestCase(ConflictHandling.Ignore, 5)]
		[TestCase(ConflictHandling.Merge, 10)]
		[TestCase(ConflictHandling.Throw, null)]
		public static void Assign(ConflictHandling conflictHandling, int? expectedDifferent) {
			var a = new Dictionary<string, Level2> {
				["Null0"] = null!,
				["Null1"] = null!,
				["Null2"] = new Level2(1),
				["Value"] = new Level2(2),
				["Same"] = new Level2(42),
				["Different"] = new Level2(5)
			};

			var b = new Dictionary<string, Level2> {
				["Null1"] = new Level2(1),
				["Null2"] = null!,
				["Null3"] = null!,
				["Same"] = new Level2(42),
				["Different"] = new Level2(15),
				["New"] = new Level2(666)
			};

			if (expectedDifferent.HasValue) {
				a.Assign(b, conflictHandling);

				Assert.IsNull(a["Null0"]);
				Assert.AreEqual(1, a["Null1"].Id);
				Assert.AreEqual(1, a["Null2"].Id);
				Assert.IsNull(a["Null3"]);
				Assert.AreEqual(2, a["Value"].Id);
				Assert.AreEqual(42, a["Same"].Id);
				Assert.AreEqual(expectedDifferent.Value, a["Different"].Id);
				Assert.AreEqual(666, a["New"].Id);
			}

			else {
				var exception = Assert.Throws<AssignConflictException>(() => a.Assign(b, conflictHandling));
				Assert.AreEqual("[Different].Id", exception.Path);
			}
		}

		[Test]
		[TestCase(ConflictHandling.Replace, "B")]
		[TestCase(ConflictHandling.Ignore, "A")]
		[TestCase(ConflictHandling.Merge, "AB")]
		[TestCase(ConflictHandling.Throw, null)]
		public static void Assign_strings(ConflictHandling conflictHandling, string? expectedDifferent) {
			var a = new Dictionary<string, string?> {
				["Null0"] = null,
				["Null1"] = null,
				["Null2"] = "2",
				["Value"] = "V",
				["Same"] = "S",
				["Different"] = "A"
			};

			var b = new Dictionary<string, string?> {
				["Null1"] = "1",
				["Null2"] = null,
				["Null3"] = null,
				["Same"] = "S",
				["Different"] = "B",
				["New"] = "N"
			};

			if (expectedDifferent == null) {
				var exception = Assert.Throws<AssignConflictException>(() => a.Assign(b, StringComparison.Ordinal, conflictHandling, string.Concat));
				Assert.AreEqual("[Different]", exception.Path);
			}

			else {
				a.Assign(b, StringComparison.Ordinal, conflictHandling, string.Concat);

				Assert.IsNull(a["Null0"]);
				Assert.AreEqual("1", a["Null1"]);
				Assert.AreEqual("2", a["Null2"]);
				Assert.IsNull(a["Null3"]);
				Assert.AreEqual("V", a["Value"]);
				Assert.AreEqual("S", a["Same"]);
				Assert.AreEqual(expectedDifferent, a["Different"]);
				Assert.AreEqual("N", a["New"]);
			}
		}

		[Test]
		[TestCase(ConflictHandling.Replace, 40)]
		[TestCase(ConflictHandling.Ignore, 30)]
		[TestCase(ConflictHandling.Merge, 35)]
		[TestCase(ConflictHandling.Throw, null)]
		public static void Assign_ints(ConflictHandling conflictHandling, int? expectedDifferent) {
			var a = new Dictionary<string, int> {
				["Null0"] = 0,
				["Null1"] = 0,
				["Null2"] = 2,
				["Value"] = 10,
				["Same"] = 20,
				["Different"] = 30
			};

			var b = new Dictionary<string, int> {
				["Null1"] = 1,
				["Null2"] = 0,
				["Null3"] = 0,
				["Same"] = 20,
				["Different"] = 40,
				["New"] = 50
			};

			if (expectedDifferent.HasValue) {
				a.Assign(b, conflictHandling, MathD.Avg);

				Assert.AreEqual(0, a["Null0"]);
				Assert.AreEqual(1, a["Null1"]);
				Assert.AreEqual(2, a["Null2"]);
				Assert.AreEqual(0, a["Null3"]);
				Assert.AreEqual(10, a["Value"]);
				Assert.AreEqual(20, a["Same"]);
				Assert.AreEqual(expectedDifferent, a["Different"]);
				Assert.AreEqual(50, a["New"]);
			}

			else {
				var exception = Assert.Throws<AssignConflictException>(() => a.Assign(b, conflictHandling, MathD.Avg));
				Assert.AreEqual("[Different]", exception.Path);
			}
		}

		[Test]
		[TestCase(ConflictHandling.Replace, "https://b.com")]
		[TestCase(ConflictHandling.Ignore, "https://a.com")]
		[TestCase(ConflictHandling.Merge, "https://dev.null")]
		[TestCase(ConflictHandling.Throw, null)]
		public static void Assign_URIs(ConflictHandling conflictHandling, string? expectedDifferent) {
			var a = new Dictionary<string, Uri?> {
				["Null0"] = null,
				["Null1"] = null,
				["Null2"] = new Uri("https://n2.com"),
				["Value"] = new Uri("https://v.com"),
				["Same"] = new Uri("https://s.com"),
				["Different"] = new Uri("https://a.com")
			};

			var b = new Dictionary<string, Uri?> {
				["Null1"] = new Uri("https://n1.com"),
				["Null2"] = null,
				["Null3"] = null,
				["Same"] = new Uri("https://s.com"),
				["Different"] = new Uri("https://b.com"),
				["New"] = new Uri("https://n.com")
			};

			if (expectedDifferent == null) {
				var exception = Assert.Throws<AssignConflictException>(() => a.Assign(b, conflictHandling, (_, _) => new Uri("https://dev.null"), "Uris"));
				Assert.AreEqual("Uris[Different]", exception.Path);
			}

			else {
				a.Assign(b, conflictHandling, (_, _) => new Uri("https://dev.null"));

				Assert.IsNull(a["Null0"]);
				Assert.AreEqual(new Uri("https://n1.com"), a["Null1"]);
				Assert.AreEqual(new Uri("https://n2.com"), a["Null2"]);
				Assert.IsNull(a["Null3"]);
				Assert.AreEqual(new Uri("https://v.com"), a["Value"]);
				Assert.AreEqual(new Uri("https://s.com"), a["Same"]);
				Assert.AreEqual(new Uri(expectedDifferent), a["Different"]);
				Assert.AreEqual(new Uri("https://n.com"), a["New"]);
			}
		}

		[Test]
		public static void AssignConflicException_Path() {
			var a = new Level1 { [1] = new Level2(1), [2] = new Level2(1) };
			var b = new Level1 { [2] = new Level2(2), [3] = new Level2(2) };

			var exception = Assert.Throws<AssignConflictException>(() => a.Assign(b, ConflictHandling.Throw));
			Assert.AreEqual("Subs[2].Id", exception.Path);

			exception = Assert.Throws<AssignConflictException>(() => a.Assign(b, ConflictHandling.Throw, "@var"));
			Assert.AreEqual("@var.Subs[2].Id", exception.Path);
		}
	}
}
