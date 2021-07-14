using LinqToYourDoom.System;

namespace LinqToYourDoom {
	sealed class Bug : BugException {
		public Bug(string bugId) : base(bugId, "https://github.com/Odepax/linq-to-your-doom/issues") {}
	}
}
