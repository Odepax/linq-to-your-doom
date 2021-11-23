using System;
using System.Collections.Generic;
using System.Text;

namespace LinqToYourDoom;

public class IndentedStringBuilder {
	public StringBuilder Output { get; }
	public string IndentString { get; set; }

	byte IndentLevel = 0;
	bool MustIndent = true;

	public IndentedStringBuilder(/*                  */ string indentString = "\t") : this(new StringBuilder(), indentString) {}
	public IndentedStringBuilder(/*    */ int capacity, string indentString = "\t") : this(new StringBuilder(capacity), indentString) {}
	public IndentedStringBuilder(/*   */ string? value, string indentString = "\t") : this(new StringBuilder(value), indentString) {}
	public IndentedStringBuilder(StringBuilder? output, string indentString = "\t") {
		Output = output ?? new();
		IndentString = indentString;
	}

	public int Length { get => Output.Length; set => Output.Length = value; }
	public int Capacity { get => Output.Capacity; set => Output.Capacity = value; }

	public char this[int index] { get => Output[index]; set => Output[index] = value; }

	public IndentedStringBuilder Indent() {
		++IndentLevel;

		return this;
	}

	public IndentedStringBuilder Unindent() {
		if (0 < IndentLevel)
			--IndentLevel;

		return this;
	}

	/// <summary>
	/// Throws an <see cref="InvalidOperationException"/> if the current indentation level
	/// of this <see cref="IndentedStringBuilder"/> isn't as <paramref name="expected"/>.
	/// </summary>
	public IndentedStringBuilder AssertIndentLevel(int expected = 0) {
		if (IndentLevel != expected)
			throw new InvalidOperationException($"Indentation level is { IndentLevel } instead of expected { expected }.");

		return this;
	}

	public IndentedStringBuilder AppendSingleIndent() {
		Output.Append(IndentString);

		return this;
	}

	public IndentedStringBuilder AppendIndent() {
		for (var i = 0; i < IndentLevel; ++i)
			Output.Append(IndentString);

		return this;
	}

	/// <summary>
	/// Appends a new line.
	/// The next non-new-line append will append indentations up to the current indent level.
	/// </summary>
	public IndentedStringBuilder AppendLine() {
		Output.AppendLine();

		MustIndent = true;

		return this;
	}

	public IndentedStringBuilder AppendLine(string value) => Append(value).AppendLine();

	public IndentedStringBuilder Append(string value) {
		if (MustIndent) {
			MustIndent = false;

			AppendIndent();
		}

		Output.Append(value);

		return this;
	}

	public IndentedStringBuilder AppendWithoutIndent(string value) {
		if (MustIndent)
			MustIndent = false;

		Output.Append(value);

		return this;
	}

	public IndentedStringBuilder AppendJoin(IEnumerable<string> values, string separator = "", string prefix = "", string suffix = "", string fallback = "") {
		using var enumerator = values.GetEnumerator();

		if (enumerator.MoveNext()) {
			Append(prefix);
			Append(enumerator.Current);

			while (enumerator.MoveNext()) {
				Append(separator);
				Append(enumerator.Current);
			}

			Append(suffix);
		}

		else Append(fallback);

		return this;
	}

	public override string ToString() => Output.ToString();
}
