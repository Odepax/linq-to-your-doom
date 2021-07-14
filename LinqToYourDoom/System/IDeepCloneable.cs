using System;

namespace LinqToYourDoom.System {
	/// <summary>
	/// Cloning, with explicit <b>deep</b> sematic lacked by <see cref="ICloneable"/>.
	/// </summary>
	///
	/// <typeparam name="TSelf">
	/// Meant to be the type implementing this interface,
	/// e.g. <c>class Cat : IDeepCloneable&lt;Cat&gt; {}</c>.
	/// </typeparam>
	public interface IDeepCloneable<out TSelf> : IShallowCloneable<TSelf> {
		/// <param name="depth">
		/// 0-based: a value of <c>0</c> is equivalent to a shallow cloning,
		/// i.e. it does <b>NOT</b> just <c>return this;</c>
		/// </param>
		TSelf DeepClone(uint depth = uint.MaxValue);

		TSelf IShallowCloneable<TSelf>.ShallowClone() => DeepClone(0);
	}
}
