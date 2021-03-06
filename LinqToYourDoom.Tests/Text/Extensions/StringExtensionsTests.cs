using System;
using System.Text;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Text.Extensions;

static class StringExtensionsTests {
	[Test]
	[TestCase("", 0, "")]
	[TestCase("", 2, "")]
	[TestCase("", -3, "")]
	[TestCase("abcde", 2, "cde")]
	[TestCase("abcde", 12, "")]
	[TestCase("abcde", 0, "abcde")]
	[TestCase("abcde", -1, "e")]
	[TestCase("abcde", -3, "cde")]
	[TestCase("abcde", -12, "abcde")]
	public static void LenientSubstring(string value, int startIndex, string expected) {
		var actual = value.LenientSubstring(startIndex);

		Assert.AreEqual(expected, actual);
	}

	[Test]
	[TestCase("", 0, 0, "")]
	[TestCase("", 0, 2, "")]
	[TestCase("", 0, -3, "")]
	[TestCase("", 2, 0, "")]
	[TestCase("", 2, 3, "")]
	[TestCase("", 2, -1, "")]
	[TestCase("", -3, 0, "")]
	[TestCase("", -3, 2, "")]
	[TestCase("", -3, -1, "")]
	[TestCase("abcde", 2, 0, "")]
	[TestCase("abcde", 2, 2, "cd")]
	[TestCase("abcde", 2, 12, "cde")]
	[TestCase("abcde", 2, -1, "cd")]
	[TestCase("abcde", 2, -12, "")]
	[TestCase("abcde", 12, 3, "")]
	[TestCase("abcde", 12, -1, "")]
	[TestCase("abcde", 0, 3, "abc")]
	[TestCase("abcde", 0, 7, "abcde")]
	[TestCase("abcde", 0, -2, "abc")]
	[TestCase("abcde", 0, -12, "")]
	[TestCase("abcde", -1, 1, "e")]
	[TestCase("abcde", -1, 3, "e")]
	[TestCase("abcde", -1, -8, "")]
	[TestCase("abcde", -3, 2, "cd")]
	[TestCase("abcde", -3, 8, "cde")]
	[TestCase("abcde", -3, -1, "cd")]
	[TestCase("abcde", -3, -3, "")]
	[TestCase("abcde", -3, -12, "")]
	[TestCase("abcde", -12, 0, "")]
	[TestCase("abcde", -12, -2, "abc")]
	[TestCase("abcde", -12, -7, "")]
	[TestCase("abcde", -12, 4, "abcd")]
	[TestCase("abcde", -12, 9, "abcde")]
	public static void LenientSubstring(string value, int startIndex, int length, string expected) {
		var actual = value.LenientSubstring(startIndex, length);

		Assert.AreEqual(expected, actual);
	}

	[Test]
	[TestCase("", -2, false, default(char))]
	[TestCase("", 0, false, default(char))]
	[TestCase("", 1, false, default(char))]
	[TestCase("abc", -1, false, default(char))]
	[TestCase("abc", 0, true, 'a')]
	[TestCase("abc", 8, false, default(char))]
	public static void TryGetCharAt(string value, int i, bool expectedReturn, char expectedChar) {
		var actualReturn = value.TryGetCharAt(i, out var actualChar);

		Assert.AreEqual(expectedReturn, actualReturn);
		Assert.AreEqual(expectedChar, actualChar);
	}

	[Test]
	[TestCase("xy", -5, "")]
	[TestCase("xy", -1, "")]
	[TestCase("xy", 0, "")]
	[TestCase("xy", 1, "xy")]
	[TestCase("xy", 5, "xyxyxyxyxy")]
	[TestCase("", 3, "")]
	public static void Repeat(string value, int count, string expected) {
		var actual = value.Repeat(count, ArgumentValidation.Lenient);

		Assert.AreEqual(expected, actual);

		if (count < 0) Assert.Throws<ArgumentOutOfRangeException>(() => value.Repeat(count));
	}

	[Test]
	[TestCase("xy", -5, "", "/")]
	[TestCase("xy", -1, "", "/")]
	[TestCase("xy", 0, "", "/")]
	[TestCase("xy", 1, "xy", "<xy>")]
	[TestCase("xy", 5, "xyxyxyxyxy", "<xy, xy, xy, xy, xy>")]
	[TestCase("", 3, "", "/")]
	public static void RepeatToBuilder(string value, int count, string expectedA, string expectedB) {
		var outputA = new StringBuilder();
		var outputB = new StringBuilder();

		value.RepeatToBuilder(count, outputA, ArgumentValidation.Lenient);
		value.RepeatToBuilder(count, outputB, separator: ", ", prefix: "<", suffix: ">", fallback: "/", ArgumentValidation.Lenient);

		Assert.AreEqual(expectedA, outputA.ToString());
		Assert.AreEqual(expectedB, outputB.ToString());

		if (count < 0) {
			Assert.Throws<ArgumentOutOfRangeException>(() => value.RepeatToBuilder(count, outputA));
			Assert.Throws<ArgumentOutOfRangeException>(() => value.RepeatToBuilder(count, outputB, separator: ", ", prefix: "<", suffix: ">", fallback: "/"));
		}
	}

	[Test]
	[Ignore("Eye & compiler only")]
	public static void RepeatToBuilder_disambiguation() {
		var output = new StringBuilder();

		// string string.Repeat(int, ArgumentValidation)
		string.Empty.Repeat(0);
		string.Empty.Repeat(0, ArgumentValidation.Lenient);

		// string string.Repeat(int, string, string, string, string, ArgumentValidation)
		string.Empty.Repeat(0, "", "", "", "");
		string.Empty.Repeat(0, "", "", "", "", ArgumentValidation.Lenient);
		string.Empty.Repeat(0, separator: "", prefix: "", suffix: "", fallback: "");
		string.Empty.Repeat(0, separator: "", prefix: "", suffix: "", fallback: "", ArgumentValidation.Lenient);

		// string string.Repeat(int, ArgumentValidation, string, string, string, string)
		string.Empty.Repeat(0, ArgumentValidation.Lenient, "", "", "", "");
		string.Empty.Repeat(0, ArgumentValidation.Lenient, separator: "", prefix: "", suffix: "", fallback: "");

		// StringBuilder string.RepeatToBuilder(int, ArgumentValidation)
		string.Empty.RepeatToBuilder(0);
		string.Empty.RepeatToBuilder(0, ArgumentValidation.Lenient);

		// StringBuilder string.RepeatToBuilder(int, string, string, string, string, ArgumentValidation)
		string.Empty.RepeatToBuilder(0, "", "", "", "");
		string.Empty.RepeatToBuilder(0, "", "", "", "", ArgumentValidation.Lenient);
		string.Empty.RepeatToBuilder(0, separator: "", prefix: "", suffix: "", fallback: "");
		string.Empty.RepeatToBuilder(0, separator: "", prefix: "", suffix: "", fallback: "", ArgumentValidation.Lenient);

		// StringBuilder string.RepeatToBuilder(int, ArgumentValidation, string, string, string, string)
		string.Empty.RepeatToBuilder(0, ArgumentValidation.Lenient, "", "", "", "");
		string.Empty.RepeatToBuilder(0, ArgumentValidation.Lenient, separator: "", prefix: "", suffix: "", fallback: "");

		// StringBuilder string.RepeatToBuilder(int, StringBuilder, ArgumentValidation)
		string.Empty.RepeatToBuilder(0, output);
		string.Empty.RepeatToBuilder(0, output, ArgumentValidation.Lenient);

		// StringBuilder string.RepeatToBuilder(int, StringBuilder, string, string, string, string, ArgumentValidation)
		string.Empty.RepeatToBuilder(0, output, "", "", "", "");
		string.Empty.RepeatToBuilder(0, output, "", "", "", "", ArgumentValidation.Lenient);
		string.Empty.RepeatToBuilder(0, output, separator: "", prefix: "", suffix: "", fallback: "");
		string.Empty.RepeatToBuilder(0, output, separator: "", prefix: "", suffix: "", fallback: "", ArgumentValidation.Lenient);

		// StringBuilder string.RepeatToBuilder(int, StringBuilder, ArgumentValidation, string, string, string, string)
		string.Empty.RepeatToBuilder(0, output, ArgumentValidation.Lenient, "", "", "", "");
		string.Empty.RepeatToBuilder(0, output, ArgumentValidation.Lenient, separator: "", prefix: "", suffix: "", fallback: "");
	}

	[Test]
	[TestCase("abc abc", "a", "bc bc")]
	[TestCase("abc abc", "b", "ac ac")]
	[TestCase("abc abc", "bc", "a a")]
	[TestCase("abc abc", " ab", "abcc")]
	[TestCase("abc abc", " ", "abcabc")]
	[TestCase("abc abc", "xy", "abc abc")]
	[TestCase("", "a", "")]
	public static void Remove(string value, string toRemove, string expected) {
		var actual = value.Remove(toRemove);

		Assert.AreEqual(expected, actual);
	}

	[Test]
	public static void Append() {
		Assert.AreEqual("Xx", (null as string).Append("Xx"));
		Assert.AreEqual("Xx", string.Empty.Append("Xx"));
		Assert.AreEqual("XxYy", "Xx".Append("Yy"));

		Assert.AreEqual("Xx", ((null as string)?.Append('.')).Append("Xx"));
		Assert.AreEqual(".Xx", string.Empty?.Append('.').Append("Xx"));
		Assert.AreEqual("Xx.Yy", "Xx"?.Append('.').Append("Yy"));
	}

	[Test]
	public static void Prepend() {
		Assert.AreEqual("Xx", (null as string).Prepend("Xx"));
		Assert.AreEqual("Xx", string.Empty.Prepend("Xx"));
		Assert.AreEqual("YyXx", "Xx".Prepend("Yy"));

		Assert.AreEqual("Xx", ((null as string)?.Prepend('.')).Prepend("Xx"));
		Assert.AreEqual("Xx.", string.Empty?.Prepend('.').Prepend("Xx"));
		Assert.AreEqual("Yy.Xx", "Xx"?.Prepend('.').Prepend("Yy"));
	}

	[Test]
	[TestCase("")]
	[TestCase("Whatever")]
	public static void ToBytes(string data) {
		Assert.AreEqual(data, data.ToAsciiBytes().ToAsciiString());
		Assert.AreEqual(data, data.ToUtf8Bytes().ToUtf8String());
		Assert.AreEqual(data, data.ToUtf16Bytes().ToUtf16String());
		Assert.AreEqual(data, data.ToUtf32Bytes().ToUtf32String());
	}
}
