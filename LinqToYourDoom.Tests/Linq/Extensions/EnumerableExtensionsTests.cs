using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Linq.Extensions;

static class EnumerableExtensionsTests {
	class Folder : IEquatable<string> {
		public string Name = string.Empty;

		Folder? Parent_field;
		public Folder? Parent {
			get => Parent_field;
			set {
				Parent_field = value;
				Parent_field!.Children.Add(this);
			}
		}

		public List<Folder> Children = new();

		public override int GetHashCode() => base.GetHashCode();
		public override bool Equals(object? obj) => Equals(obj as string);
		public bool Equals(string? other) => Name.Equals(other);
	}

	// /
	// '  usr
	// '  '  bin
	// '  '  local
	// '  var
	// '  '  tmp
	// '  '  www
	// '  '  '  app
	// '  '  '  '  src
	static readonly Folder root = new() { Name = string.Empty };
	static readonly Folder usr = new() { Name = "usr", Parent = root };
	static readonly Folder bin = new() { Name = "bin", Parent = usr };
	static readonly Folder local = new() { Name = "local", Parent = usr };
	static readonly Folder var = new() { Name = "var", Parent = root };
	static readonly Folder tmp = new() { Name = "tmp", Parent = var };
	static readonly Folder www = new() { Name = "www", Parent = var };
	static readonly Folder app = new() { Name = "app", Parent = www };
	static readonly Folder src = new() { Name = "src", Parent = app };
	static readonly Folder opt = new() { Name = "opt", Parent = root };

	[Test]
	public static void Recurse_exception_example() {
		var exception = new Exception("Exception 3",
			new Exception("Exception 2",
				new Exception("Exception 1")
			)
		);

		var expected = "threw Exception 3\n\tbecause of Exception 2\n\t\tbecause of Exception 1";

		var actual = exception
			.Recurse(e => e.InnerException)
			.Select((e, depth) =>
				depth == 0
					? "threw " + e.Message
					: '\t'.Repeat(depth) + "because of " + e.Message
			)
			.JoinToString("\n");

		Assert.AreEqual(expected, actual);

		// -- OR --

		actual = "threw " + exception.Message + exception.InnerException
			.Recurse(e => e.InnerException)
			.JoinToStringBuilder((e, depth, output) =>
				output
					.Append('\n')
					.Append('\t', depth + 1)
					.Append("because of ")
					.Append(e.Message)
			)
			.ToString();

		Assert.AreEqual(expected, actual);
	}

	[Test]
	public static void RecurseMany_directory_example() {
		var directory = new Folder {
			Name = ".",
			Children = {
			new Folder { Name = "bin", Children = {
				new Folder { Name = "net5", Children = {
					new Folder { Name = "Debug" }
				} }
			} },
			new Folder { Name = "obj", Children = {
				new Folder { Name = "classes" },
				new Folder { Name = "gen" }
			} },
			new Folder { Name = "src", Children = {
				new Folder { Name = "Lib", Children = {
					new Folder { Name = "Internal", Children = {
						new Folder { Name = "Threading", Children = {
							new Folder { Name = "Tasks" }
						} }
					} }
				} },
				new Folder { Name = "Extensions" },
			} },
		}
		};

		var expected = "1 | ./\n2 | '  src/\n3 | '  '  Lib/\n4 | '  '  '  Internal/\n5 | '  '  Extensions/";

		var actual = directory
			.RecurseManyDepthed((dir, depth) =>
				/* maxDepth, inclusive */ 3 <= depth
					? Array.Empty<Folder>()
					: dir.Children.Where(child =>
							child.Name.DoesNotEqual("bin", StringComparison.OrdinalIgnoreCase)
						&& child.Name.DoesNotEqual("obj", StringComparison.OrdinalIgnoreCase)
					)
			)
			.JoinToStringBuilder((dir, i, output) =>
				output
					.Append(i + 1)
					.Append(" | ")
					.Insert(output.Length, "'  ", dir.Depth)
					.Append(dir.Item.Name)
					.Append('/'),
				separator: "\n"
			)
			.ToString();

		Assert.AreEqual(expected, actual);
	}

	[Test]
	public static void Recurse_TT() {
		Assert.IsEmpty((null as Folder).Recurse(folder => folder.Parent));

		Assert.AreEqual("", root.Recurse(folder => folder.Parent).Reverse().Select(folder => folder.Name).JoinToString("/"));
		Assert.AreEqual("/usr", usr.Recurse(folder => folder.Parent).Reverse().Select(folder => folder.Name).JoinToString("/"));
		Assert.AreEqual("/usr/bin", bin.Recurse(folder => folder.Parent).Reverse().Select(folder => folder.Name).JoinToString("/"));
		Assert.AreEqual("/usr/local", local.Recurse(folder => folder.Parent).Reverse().Select(folder => folder.Name).JoinToString("/"));
		Assert.AreEqual("/var", var.Recurse(folder => folder.Parent).Reverse().Select(folder => folder.Name).JoinToString("/"));
		Assert.AreEqual("/var/tmp", tmp.Recurse(folder => folder.Parent).Reverse().Select(folder => folder.Name).JoinToString("/"));
		Assert.AreEqual("/var/www", www.Recurse(folder => folder.Parent).Reverse().Select(folder => folder.Name).JoinToString("/"));
		Assert.AreEqual("/var/www/app", app.Recurse(folder => folder.Parent).Reverse().Select(folder => folder.Name).JoinToString("/"));
		Assert.AreEqual("/var/www/app/src", src.Recurse(folder => folder.Parent).Reverse().Select(folder => folder.Name).JoinToString("/"));
	}

	[Test]
	public static void Recurse_TiT() {
		var depths = new List<(string, int)>();

		void AssertRecurse(Folder? folder, object expectedResult, object expectedDepths) {
			depths.Clear();
			Assert.AreEqual(expectedResult, folder.Recurse((folder, depth) => { depths.Add((folder.Name, depth)); return folder.Parent; }));
			Assert.AreEqual(expectedDepths, depths);
		}

		AssertRecurse(null, Array.Empty<string>(), Array.Empty<(string, int)>());

		AssertRecurse(root, new[] { "" }, new[] { ("", 0) });
		AssertRecurse(usr, new[] { "usr", "" }, new[] { ("usr", 0), ("", 1) });
		AssertRecurse(bin, new[] { "bin", "usr", "" }, new[] { ("bin", 0), ("usr", 1), ("", 2) });
		AssertRecurse(local, new[] { "local", "usr", "" }, new[] { ("local", 0), ("usr", 1), ("", 2) });
		AssertRecurse(var, new[] { "var", "", }, new[] { ("var", 0), ("", 1) });
		AssertRecurse(tmp, new[] { "tmp", "var", "" }, new[] { ("tmp", 0), ("var", 1), ("", 2) });
		AssertRecurse(www, new[] { "www", "var", "" }, new[] { ("www", 0), ("var", 1), ("", 2) });
		AssertRecurse(app, new[] { "app", "www", "var", "" }, new[] { ("app", 0), ("www", 1), ("var", 2), ("", 3) });
		AssertRecurse(src, new[] { "src", "app", "www", "var", "" }, new[] { ("src", 0), ("app", 1), ("www", 2), ("var", 3), ("", 4) });
	}

	[Test]
	public static void RecurseMany_TT() {
		var encountered = new List<string>();

		void AssertRecurseMany(Folder? folder, object expectedResult, object expectedEncountered) {
			encountered.Clear();
			Assert.AreEqual(expectedResult, folder.RecurseMany(folder => { encountered.Add(folder.Name); return folder.Children; }));
			Assert.AreEqual(expectedEncountered, encountered);
		}

		AssertRecurseMany(null, Array.Empty<string>(), Array.Empty<string>());

		AssertRecurseMany(root, new[] { "", "usr", "bin", "local", "var", "tmp", "www", "app", "src", "opt" }, new[] { "", "usr", "bin", "local", "var", "tmp", "www", "app", "src", "opt" });
		AssertRecurseMany(usr, new[] { "usr", "bin", "local" }, new[] { "usr", "bin", "local" });
		AssertRecurseMany(bin, new[] { "bin" }, new[] { "bin" });
		AssertRecurseMany(local, new[] { "local" }, new[] { "local" });
		AssertRecurseMany(var, new[] { "var", "tmp", "www", "app", "src" }, new[] { "var", "tmp", "www", "app", "src" });
		AssertRecurseMany(tmp, new[] { "tmp" }, new[] { "tmp" });
		AssertRecurseMany(www, new[] { "www", "app", "src" }, new[] { "www", "app", "src" });
		AssertRecurseMany(app, new[] { "app", "src" }, new[] { "app", "src" });
		AssertRecurseMany(src, new[] { "src" }, new[] { "src" });

		encountered.Clear();
		Assert.AreEqual(new[] { "", "var", "tmp", "www", "app", "src", "opt" }, root.RecurseMany(folder => { encountered.Add(folder.Name); return folder.Children.Where(child => child.Name != "usr"); }));
		Assert.AreEqual(new[] { "", "var", "tmp", "www", "app", "src", "opt" }, encountered);
	}

	[Test]
	public static void RecurseMany_TiT() {
		var encountered = new List<(string, int)>();

		void AssertRecurseMany(Folder? folder, object expectedResult, object expectedEncountered) {
			encountered.Clear();
			Assert.AreEqual(expectedResult, folder.RecurseMany((folder, depth) => { encountered.Add((folder.Name, depth)); return folder.Children; }));
			Assert.AreEqual(expectedEncountered, encountered);
		}

		AssertRecurseMany(null, Array.Empty<string>(), Array.Empty<(string, int)>());

		AssertRecurseMany(root, new[] { "", "usr", "bin", "local", "var", "tmp", "www", "app", "src", "opt" }, new[] { ("", 0), ("usr", 1), ("bin", 2), ("local", 2), ("var", 1), ("tmp", 2), ("www", 2), ("app", 3), ("src", 4), ("opt", 1) });
		AssertRecurseMany(usr, new[] { "usr", "bin", "local" }, new[] { ("usr", 0), ("bin", 1), ("local", 1) });
		AssertRecurseMany(bin, new[] { "bin" }, new[] { ("bin", 0) });
		AssertRecurseMany(local, new[] { "local" }, new[] { ("local", 0) });
		AssertRecurseMany(var, new[] { "var", "tmp", "www", "app", "src" }, new[] { ("var", 0), ("tmp", 1), ("www", 1), ("app", 2), ("src", 3) });
		AssertRecurseMany(tmp, new[] { "tmp" }, new[] { ("tmp", 0) });
		AssertRecurseMany(www, new[] { "www", "app", "src" }, new[] { ("www", 0), ("app", 1), ("src", 2) });
		AssertRecurseMany(app, new[] { "app", "src" }, new[] { ("app", 0), ("src", 1) });
		AssertRecurseMany(src, new[] { "src" }, new[] { ("src", 0) });

		encountered.Clear();
		Assert.AreEqual(new[] { "", "usr", "bin", "local", "var", "tmp", "www", "opt" }, root.RecurseMany((folder, depth) => { encountered.Add((folder.Name, depth)); return 2 <= depth ? Array.Empty<Folder>() : folder.Children; }));
		Assert.AreEqual(new[] { ("", 0), ("usr", 1), ("bin", 2), ("local", 2), ("var", 1), ("tmp", 2), ("www", 2), ("opt", 1) }, encountered);
	}

	[Test]
	public static void RecurseManyDepthed_TT() {
		var encountered = new List<string>();

		void AssertRecurseManyDepthed(Folder? folder, object expectedResult, object expectedEncountered) {
			encountered.Clear();
			Assert.AreEqual(expectedResult, folder.RecurseManyDepthed(folder => { encountered.Add(folder.Name); return folder.Children; }));
			Assert.AreEqual(expectedEncountered, encountered);
		}

		AssertRecurseManyDepthed(null, Array.Empty<string>(), Array.Empty<string>());

		AssertRecurseManyDepthed(root, new[] { (0, ""), (1, "usr"), (2, "bin"), (2, "local"), (1, "var"), (2, "tmp"), (2, "www"), (3, "app"), (4, "src"), (1, "opt") }, new[] { "", "usr", "bin", "local", "var", "tmp", "www", "app", "src", "opt" });
		AssertRecurseManyDepthed(usr, new[] { (0, "usr"), (1, "bin"), (1, "local") }, new[] { "usr", "bin", "local" });
		AssertRecurseManyDepthed(bin, new[] { (0, "bin") }, new[] { "bin" });
		AssertRecurseManyDepthed(local, new[] { (0, "local") }, new[] { "local" });
		AssertRecurseManyDepthed(var, new[] { (0, "var"), (1, "tmp"), (1, "www"), (2, "app"), (3, "src") }, new[] { "var", "tmp", "www", "app", "src" });
		AssertRecurseManyDepthed(tmp, new[] { (0, "tmp") }, new[] { "tmp" });
		AssertRecurseManyDepthed(www, new[] { (0, "www"), (1, "app"), (2, "src") }, new[] { "www", "app", "src" });
		AssertRecurseManyDepthed(app, new[] { (0, "app"), (1, "src") }, new[] { "app", "src" });
		AssertRecurseManyDepthed(src, new[] { (0, "src") }, new[] { "src" });

		encountered.Clear();
		Assert.AreEqual(new[] { (0, ""), (1, "var"), (2, "tmp"), (2, "www"), (3, "app"), (4, "src"), (1, "opt") }, root.RecurseManyDepthed(folder => { encountered.Add(folder.Name); return folder.Children.Where(child => child.Name != "usr"); }));
		Assert.AreEqual(new[] { "", "var", "tmp", "www", "app", "src", "opt" }, encountered);
	}

	[Test]
	public static void RecurseManyDepthed_TiT() {
		var encountered = new List<(string, int)>();

		void AssertRecurseManyDepthed(Folder? folder, object expectedResult, object expectedEncountered) {
			encountered.Clear();
			Assert.AreEqual(expectedResult, folder.RecurseManyDepthed((folder, depth) => { encountered.Add((folder.Name, depth)); return folder.Children; }));
			Assert.AreEqual(expectedEncountered, encountered);
		}

		AssertRecurseManyDepthed(null, Array.Empty<string>(), Array.Empty<(string, int)>());

		AssertRecurseManyDepthed(root, new[] { (0, ""), (1, "usr"), (2, "bin"), (2, "local"), (1, "var"), (2, "tmp"), (2, "www"), (3, "app"), (4, "src"), (1, "opt") }, new[] { ("", 0), ("usr", 1), ("bin", 2), ("local", 2), ("var", 1), ("tmp", 2), ("www", 2), ("app", 3), ("src", 4), ("opt", 1) });
		AssertRecurseManyDepthed(usr, new[] { (0, "usr"), (1, "bin"), (1, "local") }, new[] { ("usr", 0), ("bin", 1), ("local", 1) });
		AssertRecurseManyDepthed(bin, new[] { (0, "bin") }, new[] { ("bin", 0) });
		AssertRecurseManyDepthed(local, new[] { (0, "local") }, new[] { ("local", 0) });
		AssertRecurseManyDepthed(var, new[] { (0, "var"), (1, "tmp"), (1, "www"), (2, "app"), (3, "src") }, new[] { ("var", 0), ("tmp", 1), ("www", 1), ("app", 2), ("src", 3) });
		AssertRecurseManyDepthed(tmp, new[] { (0, "tmp") }, new[] { ("tmp", 0) });
		AssertRecurseManyDepthed(www, new[] { (0, "www"), (1, "app"), (2, "src") }, new[] { ("www", 0), ("app", 1), ("src", 2) });
		AssertRecurseManyDepthed(app, new[] { (0, "app"), (1, "src") }, new[] { ("app", 0), ("src", 1) });
		AssertRecurseManyDepthed(src, new[] { (0, "src") }, new[] { ("src", 0) });

		encountered.Clear();
		Assert.AreEqual(new[] { (0, ""), (1, "usr"), (2, "bin"), (2, "local"), (1, "var"), (2, "tmp"), (2, "www"), (1, "opt") }, root.RecurseManyDepthed((folder, depth) => { encountered.Add((folder.Name, depth)); return 2 <= depth ? Array.Empty<Folder>() : folder.Children; }));
		Assert.AreEqual(new[] { ("", 0), ("usr", 1), ("bin", 2), ("local", 2), ("var", 1), ("tmp", 2), ("www", 2), ("opt", 1) }, encountered);
	}

	static readonly string[] Empty = Array.Empty<string>();
	static readonly string[] One = new[] { "one" };
	static readonly string[] Many = new[] { "one", "two", "three" };

	class CustomException : Exception { }

	[Test]
	public static void Single_value() {
		Assert.Throws<CustomException>(() => Empty.Single(new CustomException()));
		Assert.AreEqual("one", One.Single(new CustomException()));
		Assert.Throws<CustomException>(() => Many.Single(new CustomException()));
	}

	[Test]
	public static void Single_factory() {
		Assert.Throws<CustomException>(() => Empty.Single(() => new CustomException()));
		Assert.AreEqual("one", One.Single(() => new CustomException()));
		Assert.Throws<CustomException>(() => Many.Single(() => new CustomException()));
	}

	[Test]
	public static void First_value() {
		Assert.Throws<CustomException>(() => Empty.First(new CustomException()));
		Assert.AreEqual("one", One.First(new CustomException()));
		Assert.AreEqual("one", Many.First(new CustomException()));
	}

	[Test]
	public static void First_factory() {
		Assert.Throws<CustomException>(() => Empty.First(() => new CustomException()));
		Assert.AreEqual("one", One.First(() => new CustomException()));
		Assert.AreEqual("one", Many.First(() => new CustomException()));
	}

	[Test]
	public static void Last_value() {
		Assert.Throws<CustomException>(() => Empty.Last(new CustomException()));
		Assert.AreEqual("one", One.Last(new CustomException()));
		Assert.AreEqual("three", Many.Last(new CustomException()));
	}

	[Test]
	public static void Last_factory() {
		Assert.Throws<CustomException>(() => Empty.Last(() => new CustomException()));
		Assert.AreEqual("one", One.Last(() => new CustomException()));
		Assert.AreEqual("three", Many.Last(() => new CustomException()));
	}

	[Test]
	public static void JoinToString() {
		Assert.AreEqual("", Empty.JoinToString(", "));
		Assert.AreEqual("", Empty.JoinToString(", ", "[ ", " ]"));
		Assert.AreEqual("/", Empty.JoinToString(", ", "[ ", " ]", "/"));

		Assert.AreEqual("one", One.JoinToString(", "));
		Assert.AreEqual("[ one ]", One.JoinToString(", ", "[ ", " ]"));
		Assert.AreEqual("[ one ]", One.JoinToString(", ", "[ ", " ]", "/"));

		Assert.AreEqual("one, two, three", Many.JoinToString(", "));
		Assert.AreEqual("[ one, two, three ]", Many.JoinToString(", ", "[ ", " ]"));
		Assert.AreEqual("[ one, two, three ]", Many.JoinToString(", ", "[ ", " ]", "/"));
	}

	[Test]
	public static void JoinToStringBuilder() {
		Assert.AreEqual("", Empty.JoinToStringBuilder(", ").ToString());
		Assert.AreEqual("", Empty.JoinToStringBuilder(", ", "[ ", " ]").ToString());
		Assert.AreEqual("/", Empty.JoinToStringBuilder(", ", "[ ", " ]", "/").ToString());

		Assert.AreEqual("one", One.JoinToStringBuilder(", ").ToString());
		Assert.AreEqual("[ one ]", One.JoinToStringBuilder(", ", "[ ", " ]").ToString());
		Assert.AreEqual("[ one ]", One.JoinToStringBuilder(", ", "[ ", " ]", "/").ToString());

		Assert.AreEqual("one, two, three", Many.JoinToStringBuilder(", ").ToString());
		Assert.AreEqual("[ one, two, three ]", Many.JoinToStringBuilder(", ", "[ ", " ]").ToString());
		Assert.AreEqual("[ one, two, three ]", Many.JoinToStringBuilder(", ", "[ ", " ]", "/").ToString());
	}

	static readonly string[] Duplicates = new[] { "a", "b", "a", "c" };
	static readonly string[] DuplicatesBy = new[] { "aa", "abc", "ab", "abcd" };
	static readonly string[] NoDuplicates = new[] { "a", "b", "c", "d" };
	static readonly string[] NoDuplicatesBy = new[] { "a", "ab", "abc", "abcd" };

	[Test]
	public static void HasDuplicates() {
		Assert.IsTrue(Duplicates.HasDuplicates());
		Assert.IsTrue(DuplicatesBy.HasDuplicatesBy(it => it.Length));

		Assert.IsFalse(NoDuplicates.HasDuplicates());
		Assert.IsFalse(NoDuplicatesBy.HasDuplicatesBy(it => it.Length));
	}

	[Test]
	public static void DistinctBy() {
		Assert.AreEqual(new[] { "aa", "abc", "abcd" }, DuplicatesBy.DistinctBy(it => it.Length));
		Assert.AreEqual(new[] { "a", "ab", "abc", "abcd" }, NoDuplicatesBy.DistinctBy(it => it.Length));
	}

	[Test]
	public static void IntersectBy() {
		Assert.AreEqual(Array.Empty<string>(), DuplicatesBy.IntersectBy(NoDuplicatesBy, it => it.Length));
		Assert.AreEqual(new[] { "a" }, NoDuplicatesBy.IntersectBy(DuplicatesBy, it => it.Length));
	}

	[Test]
	public static void UnionBy() {
		Assert.AreEqual(new[] { "aa", "abc", "abcd", "a" }, DuplicatesBy.UnionBy(NoDuplicatesBy, it => it.Length));
		Assert.AreEqual(new[] { "a", "ab", "abc", "abcd" }, NoDuplicatesBy.UnionBy(DuplicatesBy, it => it.Length));
	}

	[Test]
	public static void AddRangeTo() {
		var source = new[] { 4, 5, 6 };
		var destination = new List<int> { 1, 2, 3 };

		source.AddRangeTo(destination);

		CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6 }, destination);
	}

	[Test]
	public static void DefaultIfEmpty() {
		Assert.AreEqual(new[] { "A", "B" }, Empty.DefaultIfEmpty(new[] { "A", "B" }));
		Assert.AreEqual(new[] { "one" }, One.DefaultIfEmpty(new[] { "A", "B" }));
	}

	class Sample { public int X; }

	[Test]
	public static void SelectDefault() {
		var source = new[] {
			new Sample { X = 1 },
			new Sample { X = 2 },
			null,
			new Sample { X = 4 },
			null
		};

		var i = 42;

		Assert.AreEqual(new[] { 1, 2, 42, 4, 42 }, source.SelectDefault(new Sample { X = 42 }).Select(it => it.X));
		Assert.AreEqual(new[] { 1, 2, 43, 4, 44 }, source.SelectDefault(() => new Sample { X = ++i }).Select(it => it.X));
		Assert.AreEqual(new[] { 1, 2, 20, 4, 40 }, source.SelectDefault(i => new Sample { X = i * 10 }).Select(it => it.X));
		Assert.AreEqual(new[] { 1, 2, 45, 4, 46 }, source.SelectDefault(it => it.X = ++i).Select(it => it.X));
		Assert.AreEqual(new[] { 1, 2, 20, 4, 40 }, source.SelectDefault((it, i) => it.X = i * 10).Select(it => it.X));
	}

	[Test]
	public static void MinBy_MaxBy() {
		Assert.AreEqual("one", Many.MinBy(it => it.Length));
		Assert.AreEqual("three", Many.MaxBy(it => it.Length));

		Assert.IsNull(Empty.MinBy(it => it.Length));
		Assert.IsEmpty(Empty.MaxBy(it => it.Length, string.Empty));
	}

	[Test]
	public static void AbsMin_AbsMax() {
		var source = new[] { 1, -2, 3, 4, -5 };

		Assert.AreEqual(1, source.AbsMin());
		Assert.AreEqual(-5, source.AbsMax());
	}

	[Test]
	public static void TakeExact() {
		Assert.Throws<InvalidOperationException>(() => Empty.TakeExact(1).ToList());
		Assert.AreEqual(new[] { "one" }, One.TakeExact(1));
		Assert.Throws<InvalidOperationException>(() => Many.TakeExact(1).ToList());

		Assert.IsEmpty(Empty.TakeExact(0));
		Assert.Throws<ArgumentOutOfRangeException>(() => One.TakeExact(-1, ArgumentValidation.Strict).ToList());
		Assert.IsEmpty(Empty.TakeExact(-1, ArgumentValidation.Lenient));
	}

	[Test]
	public static void TakeAtLeast() {
		Assert.Throws<InvalidOperationException>(() => Empty.TakeAtLeast(1).ToList());
		Assert.AreEqual(new[] { "one" }, One.TakeAtLeast(1));
		Assert.AreEqual(new[] { "one" }, Many.TakeAtLeast(1));

		Assert.IsEmpty(Empty.TakeAtLeast(0));
		Assert.Throws<ArgumentOutOfRangeException>(() => One.TakeAtLeast(-1, ArgumentValidation.Strict).ToList());
		Assert.IsEmpty(One.TakeAtLeast(-1, ArgumentValidation.Lenient));
	}

	[Test]
	public static void TrySelect() {
		var enumerableOfDictionaries = new[] {
			new Dictionary<string, float> {
				["A"] = 69.0f,
				["B"] = 42.0f,
				["C"] = 3.14f
			},
			new Dictionary<string, float> {
				["A"] = 0
			},
			new Dictionary<string, float> {
				["B"] = 666f,
				["X"] = +1f,
				["Y"] = -1f
			}
		};

		Assert.AreEqual(new[] {
			42.0f,
			666f
		}, enumerableOfDictionaries.TrySelect((Dictionary<string, float> bag, out float value) => bag.TryGetValue("B", out value)));

		Assert.AreEqual(new[] {
			(true, 42.0f),
			(false, default),
			(true, 666f)
		}, enumerableOfDictionaries.Select(bag => (bag.TryGetValue("B", out var value), value)));
	}

	[Test]
	public static void Eumerate() {
		var invokeCount = 0;

		Enumerable
			.Range(0, 3)
			.Each(_ => ++invokeCount)
			.Tee(out var enumerable);

		Assert.AreEqual(0, invokeCount);

		enumerable.Enumerate();

		Assert.AreEqual(3, invokeCount);
	}
}
