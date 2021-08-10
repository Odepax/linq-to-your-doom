using System;

namespace LinqToYourDoom {
	/// <summary>
	/// Cloning, with explicit <b>shallow</b> sematic lacked by <see cref="ICloneable"/>.
	/// </summary>
	///
	/// <typeparam name="TSelf">
	/// Meant to be the type implementing this interface,
	/// e.g. <c>class Cat : IShallowCloneable&lt;Cat&gt; {}</c>.
	/// </typeparam>
	public interface IShallowCloneable<out TSelf> {
		TSelf ShallowClone();
	}
}
