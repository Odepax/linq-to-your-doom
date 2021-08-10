using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Symbols {
	static class SymbolTests {
		[Test]
		public static new void ToString() {
			StringAssert.IsMatch(/* lang=regex */ @"Symbol<Int32>#\d+", new Symbol<int>().ToString());
			StringAssert.IsMatch(/* lang=regex */ @"Symbol<String>#\d+", new Symbol<string>().ToString());
			StringAssert.IsMatch(/* lang=regex */ @"Symbol<Single>#\d+", new Symbol<float>().ToString());
			StringAssert.IsMatch(/* lang=regex */ @"Symbol<DateTime\[\]>#\d+", new Symbol<DateTime[]>().ToString());
			StringAssert.IsMatch(/* lang=regex */ @"Symbol<Dictionary<String, IReadOnlyList<Single>>>#\d+", new Symbol<Dictionary<string, IReadOnlyList<float>>>().ToString());
		}
	}
}
