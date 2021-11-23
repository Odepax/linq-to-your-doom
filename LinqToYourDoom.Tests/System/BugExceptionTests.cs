using NUnit.Framework;

namespace LinqToYourDoom.Tests.System;

static class BugExceptionTests {
	sealed class TestBug : BugException {
		public TestBug(string bugId) : base(bugId, "omg://test.trolls") {}
	}

	[Test]
	public static void Bug() {
		try {
			throw new TestBug("0");

			#pragma warning disable CS0162
			// > Unreachable code
			//
			// Intent 100
			Assert.Fail("Well... THAT'd be a bug...");
			#pragma warning restore CS0162
		}

		catch (TestBug bug) {
			Assert.AreEqual("0: This is a bug. Please report it at omg://test.trolls.", bug.Message);
		}
	}
}
