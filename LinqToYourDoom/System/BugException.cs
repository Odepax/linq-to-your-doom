using System;

namespace LinqToYourDoom.System {
	/// <summary>
	/// A base class for exceptions to be thrown when some suposedly unreachable case has been reached,
	/// e.g. in the <see langword="default"/> case of a <see langword="switch"/> on an enumeration...
	/// </summary>
	public abstract class BugException : NotImplementedException {
		/// <inheritdoc cref="BugException"/>
		///
		/// <param name="bugId">
		/// A GUID to identify the bug easily.
		/// </param>
		///
		/// <param name="reportUrl">
		/// The URL to your issue tracker, or the e-mail address of your customer service.
		/// </param>
		protected BugException(string bugId, string reportUrl) : base(bugId + ": This is a bug. Please report it at " + reportUrl + '.') {}
	}
}
