namespace LinqToYourDoom {
	/// <summary>
	/// Defines the behavior to adopt when assigning an <see cref="IAssignable{TIn, TOut}"/>
	/// from another object containing non-default and non-equal data.
	///
	/// Default is <see cref="Replace"/>.
	/// </summary>
	public enum ConflictHandling {
		/// <summary>
		/// Wipe out anything existent and override the contents entirely.
		/// The data from the <b>other</b> object takes prevalence.
		/// </summary>
		Replace,

		/// <summary>
		/// The data from the <b>assigned</b> object takes prevalence.
		///
		/// Leave existing files as they are.
		/// Existing directories won't provision their children.
		/// </summary>
		Ignore,

		/// <summary>
		/// The conflicting pieces of data are somehow combined,
		/// however it's done is left to the implementation.
		///
		/// Files get their contents appended to existing ones.
		/// Directories preserve existing children.
		/// </summary>
		Merge,

		/// <summary>
		/// An exception is thrown when the data is different.
		/// </summary>
		Throw,

		Left = Ignore,
		Right = Replace,
		Combine = Merge,
		Error = Throw
	}
}
