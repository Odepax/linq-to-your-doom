using System;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Assignable.Extensions {
	static class AssignableStringExtensionsTests {
		[Test]
		[TestCase(null, null, ConflictHandling.Replace, "")]
		[TestCase("\t", null, ConflictHandling.Replace, "")]
		[TestCase(null, "\t", ConflictHandling.Replace, "")]
		[TestCase("", null, ConflictHandling.Replace, "")]
		[TestCase("", "\t", ConflictHandling.Replace, "")]
		[TestCase(null, "", ConflictHandling.Replace, "")]
		[TestCase("\t", "", ConflictHandling.Replace, "")]
		[TestCase(null, "B", ConflictHandling.Replace, "B")]
		[TestCase("\t", "B", ConflictHandling.Replace, "B")]
		[TestCase("", "B", ConflictHandling.Replace, "B")]
		[TestCase("A", null, ConflictHandling.Replace, "A")]
		[TestCase("A", "\t", ConflictHandling.Replace, "A")]
		[TestCase("A", "", ConflictHandling.Replace, "A")]
		[TestCase("A", "A", ConflictHandling.Replace, "A")]
		[TestCase("A", "B", ConflictHandling.Replace, "B")]

		[TestCase(null, null, ConflictHandling.Ignore, "")]
		[TestCase("\t", null, ConflictHandling.Ignore, "")]
		[TestCase(null, "\t", ConflictHandling.Ignore, "")]
		[TestCase("", null, ConflictHandling.Ignore, "")]
		[TestCase("", "\t", ConflictHandling.Ignore, "")]
		[TestCase(null, "", ConflictHandling.Ignore, "")]
		[TestCase("\t", "", ConflictHandling.Ignore, "")]
		[TestCase(null, "B", ConflictHandling.Ignore, "B")]
		[TestCase("\t", "B", ConflictHandling.Ignore, "B")]
		[TestCase("", "B", ConflictHandling.Ignore, "B")]
		[TestCase("A", null, ConflictHandling.Ignore, "A")]
		[TestCase("A", "\t", ConflictHandling.Ignore, "A")]
		[TestCase("A", "", ConflictHandling.Ignore, "A")]
		[TestCase("A", "A", ConflictHandling.Ignore, "A")]
		[TestCase("A", "B", ConflictHandling.Ignore, "A")]

		[TestCase(null, null, ConflictHandling.Merge, "")]
		[TestCase("\t", null, ConflictHandling.Merge, "")]
		[TestCase(null, "\t", ConflictHandling.Merge, "")]
		[TestCase("", null, ConflictHandling.Merge, "")]
		[TestCase("", "\t", ConflictHandling.Merge, "")]
		[TestCase(null, "", ConflictHandling.Merge, "")]
		[TestCase("\t", "", ConflictHandling.Merge, "")]
		[TestCase(null, "B", ConflictHandling.Merge, "B")]
		[TestCase("\t", "B", ConflictHandling.Merge, "B")]
		[TestCase("", "B", ConflictHandling.Merge, "B")]
		[TestCase("A", null, ConflictHandling.Merge, "A")]
		[TestCase("A", "\t", ConflictHandling.Merge, "A")]
		[TestCase("A", "", ConflictHandling.Merge, "A")]
		[TestCase("A", "A", ConflictHandling.Merge, "A")]
		[TestCase("A", "B", ConflictHandling.Merge, "AB")]

		[TestCase(null, null, ConflictHandling.Throw, "")]
		[TestCase("\t", null, ConflictHandling.Throw, "")]
		[TestCase(null, "\t", ConflictHandling.Throw, "")]
		[TestCase("", null, ConflictHandling.Throw, "")]
		[TestCase("", "\t", ConflictHandling.Throw, "")]
		[TestCase(null, "", ConflictHandling.Throw, "")]
		[TestCase("\t", "", ConflictHandling.Throw, "")]
		[TestCase(null, "B", ConflictHandling.Throw, "B")]
		[TestCase("\t", "B", ConflictHandling.Throw, "B")]
		[TestCase("", "B", ConflictHandling.Throw, "B")]
		[TestCase("A", null, ConflictHandling.Throw, "A")]
		[TestCase("A", "\t", ConflictHandling.Throw, "A")]
		[TestCase("A", "", ConflictHandling.Throw, "A")]
		[TestCase("A", "A", ConflictHandling.Throw, "A")]
		[TestCase("A", "B", ConflictHandling.Throw, null)]
		public static void Assign(string? a, string? b, ConflictHandling conflictHandling, string? expected) {
			if (expected == null) {
				var exception = Assert.Throws<AssignConflictException>(() => a.Assign(b, StringComparison.Ordinal, conflictHandling, string.Concat, "Test"));

				Assert.AreEqual("Test", exception.Path);
			}

			else {
				var actual = a.Assign(b, StringComparison.Ordinal, conflictHandling, string.Concat, "Test");

				Assert.AreEqual(expected, actual);
			}
		}
	}
}
