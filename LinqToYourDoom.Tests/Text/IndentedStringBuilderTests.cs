using System;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Text {
	static class IndentedStringBuilderTests {
		[Test]
		public static void Indent_Unindent_AssertIndentLevel() {
			var builder = new IndentedStringBuilder()
				.AssertIndentLevel(0)
				.Indent()
				.Unindent()
				.AssertIndentLevel(0)
				.Indent()
				.Indent()
				.AssertIndentLevel(2)
				.Unindent()
				.Unindent()
				.Unindent()
				.Unindent()
				.AssertIndentLevel(0);

			Assert.Throws<InvalidOperationException>(() => builder.Unindent().Indent().AssertIndentLevel(0));
		}

		[Test]
		public static void AppendSingleIndent() =>
			Assert.AreEqual("\t", new IndentedStringBuilder().AppendSingleIndent().ToString());

		[Test]
		public static void AppendIndent() {
			Assert.AreEqual("", new IndentedStringBuilder().AppendIndent().ToString());
			Assert.AreEqual("\t\t", new IndentedStringBuilder().Indent().Indent().AppendIndent().ToString());
		}

		[Test]
		public static void AppendLine_Append_AppendWithoutIndent() =>
			Assert.AreEqual($"I{ Environment.NewLine }L{ Environment.NewLine }\tI{ Environment.NewLine }\tL{ Environment.NewLine }WII{ Environment.NewLine }\t\tI", new IndentedStringBuilder()
				.Append("I").AppendLine()
				.AppendLine("L")
				.Indent()
				.Append("I").AppendLine()
				.AppendLine("L")
				.AppendWithoutIndent("W")
				.Append("I")
				.Indent()
				.Append("I").AppendLine()
				.Append("I")
				.ToString()
			);

		[Test]
		public static void Append_does_not_count_lines() =>
			Assert.AreEqual($"0{ Environment.NewLine }\t1\n0{ Environment.NewLine }", new IndentedStringBuilder()
				.AppendLine("0")
				.Indent()
				.AppendLine("1\n0")
				.ToString()
			);

		[Test]
		public static void AppendJoin() =>
			Assert.AreEqual($"<A, B, C>{ Environment.NewLine }--", new IndentedStringBuilder()
				.AppendJoin(new[] { "A", "B", "C" }, ", ", "<", ">", "--")
				.AppendLine()
				.AppendJoin(new string[0], ", ", "<", ">", "--")
				.ToString()
			);
	}
}
