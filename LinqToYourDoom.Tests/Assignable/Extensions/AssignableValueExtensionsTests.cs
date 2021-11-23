using System;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Assignable.Extensions;

static class AssignableValueExtensionsTests {
	[Test]
	[TestCase(0, 0, ConflictHandling.Replace, 0)]
	[TestCase(0, 2, ConflictHandling.Replace, 2)]
	[TestCase(1, 0, ConflictHandling.Replace, 1)]
	[TestCase(1, 1, ConflictHandling.Replace, 1)]
	[TestCase(1, 2, ConflictHandling.Replace, 2)]

	[TestCase(0, 0, ConflictHandling.Ignore, 0)]
	[TestCase(0, 2, ConflictHandling.Ignore, 2)]
	[TestCase(1, 0, ConflictHandling.Ignore, 1)]
	[TestCase(1, 1, ConflictHandling.Ignore, 1)]
	[TestCase(1, 2, ConflictHandling.Ignore, 1)]

	[TestCase(0, 0, ConflictHandling.Merge, 0)]
	[TestCase(0, 2, ConflictHandling.Merge, 2)]
	[TestCase(1, 0, ConflictHandling.Merge, 1)]
	[TestCase(1, 1, ConflictHandling.Merge, 1)]
	[TestCase(1, 2, ConflictHandling.Merge, 12)]

	[TestCase(0, 0, ConflictHandling.Throw, 0)]
	[TestCase(0, 2, ConflictHandling.Throw, 2)]
	[TestCase(1, 0, ConflictHandling.Throw, 1)]
	[TestCase(1, 1, ConflictHandling.Throw, 1)]
	[TestCase(1, 2, ConflictHandling.Throw, null)]
	public static void Assign_int(int a, int b, ConflictHandling conflictHandling, int? expected) {
		if (expected.HasValue) {
			var actual = a.Assign(b, conflictHandling, (a, b) => 10 * a + b, "Test");

			Assert.AreEqual(expected.Value, actual);
		}

		else {
			var exception = Assert.Throws<AssignConflictException>(() => a.Assign(b, conflictHandling, (a, b) => 10 * a + b, "Test"));

			Assert.AreEqual("Test", exception.Path);
		}
	}

	[Test]
	[TestCase(null, null, ConflictHandling.Replace, null, false)]
	[TestCase(null, "https://b.com", ConflictHandling.Replace, "https://b.com", false)]
	[TestCase("https://a.com", null, ConflictHandling.Replace, "https://a.com", false)]
	[TestCase("https://a.com", "https://a.com", ConflictHandling.Replace, "https://a.com", false)]
	[TestCase("https://a.com", "https://b.com", ConflictHandling.Replace, "https://b.com", false)]

	[TestCase(null, null, ConflictHandling.Ignore, null, false)]
	[TestCase(null, "https://b.com", ConflictHandling.Ignore, "https://b.com", false)]
	[TestCase("https://a.com", null, ConflictHandling.Ignore, "https://a.com", false)]
	[TestCase("https://a.com", "https://a.com", ConflictHandling.Ignore, "https://a.com", false)]
	[TestCase("https://a.com", "https://b.com", ConflictHandling.Ignore, "https://a.com", false)]

	[TestCase(null, null, ConflictHandling.Merge, null, false)]
	[TestCase(null, "https://b.com", ConflictHandling.Merge, "https://b.com", false)]
	[TestCase("https://a.com", null, ConflictHandling.Merge, "https://a.com", false)]
	[TestCase("https://a.com", "https://a.com", ConflictHandling.Merge, "https://a.com", false)]
	[TestCase("https://a.com", "https://b.com", ConflictHandling.Merge, "https://dev.null", false)]

	[TestCase(null, null, ConflictHandling.Throw, null, false)]
	[TestCase(null, "https://b.com", ConflictHandling.Throw, "https://b.com", false)]
	[TestCase("https://a.com", null, ConflictHandling.Throw, "https://a.com", false)]
	[TestCase("https://a.com", "https://a.com", ConflictHandling.Throw, "https://a.com", false)]
	[TestCase("https://a.com", "https://b.com", ConflictHandling.Throw, null, true)]
	public static void Assign_Uri(string? a, string? b, ConflictHandling conflictHandling, string? expected, bool expectException) {
		var a_uri = a == null ? null : new Uri(a);
		var b_uri = b == null ? null : new Uri(b);
		var expected_uri = expected == null ? null : new Uri(expected);
			
		if (expectException) {
			var exception = Assert.Throws<AssignConflictException>(() => a_uri.Assign(b_uri, conflictHandling, (_, _) => new Uri("https://dev.null"), "Test"));

			Assert.AreEqual("Test", exception.Path);
		}

		else {
			var actual = a_uri.Assign(b_uri, conflictHandling, (_, _) => new Uri("https://dev.null"), "Test");

			Assert.AreEqual(expected_uri, actual);
		}
	}
}
