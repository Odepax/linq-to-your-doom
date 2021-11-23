using System;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Text.Extensions;

static class CharExtensionsTests {
	[Test]
	[TestCase('x', -5, "")]
	[TestCase('x', -1, "")]
	[TestCase('x', 0, "")]
	[TestCase('x', 1, "x")]
	[TestCase('x', 5, "xxxxx")]
	public static void Repeat(char @char, int count, string expected) {
		var actual = @char.Repeat(count, ArgumentValidation.Lenient);

		Assert.AreEqual(expected, actual);

		if (count < 0) Assert.Throws<ArgumentOutOfRangeException>(() => @char.Repeat(count));
	}
}
