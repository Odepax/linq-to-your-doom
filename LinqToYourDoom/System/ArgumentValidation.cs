namespace LinqToYourDoom.System {
	/// <summary>
	/// Defines what behavior to adopt when validating method arguments.
	/// Default is <see cref="Strict"/>.
	/// </summary>
	public enum ArgumentValidation {
		/// <summary>
		/// When instructed to be strict about argument validation,
		/// a method should fail fast whenever an arguments doesn't meet its pre-requisites.
		/// </summary>
		Strict,

		/// <summary>
		/// When instructed to be lenient about argument validation,
		/// a method should attempt to get around arguments not meeting their pre-requisites,
		/// e.g. coalescing <see langword="null"/> values to empty strings or known default objects,
		/// rearranging the arguments in case <c>max &lt; min</c>,
		/// ensuring a collection is indeed sorted, etc...
		/// </summary>
		Lenient
	}
}
