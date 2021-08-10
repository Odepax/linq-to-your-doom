using System.Linq;
using NUnit.Framework;

namespace LinqToYourDoom.Tests {
	// 41DA3A82-0B89-4CB9-AF10-8E4D00FF60E1
	//
	// Directories in the source code of the main assembly are not supposed to provide sub-namespaces.
	//
	// ReSharper has an option to prevent marked directories from participating to namespacing,
	// i.e. https://www.jetbrains.com/help/resharper/Refactorings__Adjust_Namespaces.html,
	// but VS2019 doesn't.
	//
	// I wanted to flatten the assembly into a single namespace,
	// while preserving a nice organization in the solution explorer,
	// so I'm using this reflection-based test to assert that a sub-namespace hasn't sneaked in
	// at the occasion of a coding session.
	static class SingleNamespaceTests {
		[Test]
		public static void SingleNamespace() {
			var linqToYourDoomNamespaces = typeof(LinqToYourDoom.ObjectExtensions)
				.Assembly
				.GetTypes()
				.Select(type => type.Namespace)
				.Where(@namespace => @namespace != null && @namespace.StartsWith(nameof(LinqToYourDoom)))
				.Distinct()
				.ToArray();

			Assert.AreEqual(1, linqToYourDoomNamespaces.Length);
			Assert.AreEqual(nameof(LinqToYourDoom), linqToYourDoomNamespaces[0]);
		}
	}
}
