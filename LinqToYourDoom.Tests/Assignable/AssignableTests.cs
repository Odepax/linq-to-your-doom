using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Assignable;

static class AssignableTests {
	sealed class Cat : IAssignable<Cat, Cat> {
		public int Id { get; }
		public string Name { get; set; } = string.Empty;
		public double PurrPower { get; set; }
		public List<Cat> Friends { get; } = new();

		public Cat(int id) => Id = id;

		public Cat Assign(Cat other, ConflictHandling conflictHandling = default) {
			Name = Name.Assign(other.Name, StringComparison.InvariantCulture, conflictHandling, string.Concat, nameof(Name));
			PurrPower = PurrPower.Assign(other.PurrPower, conflictHandling, MathD.Avg, nameof(PurrPower));

			Friends
				.ToDictionary(it => it.Id)
				.Assign(other.Friends.SelectKeyed(it => it.Id), conflictHandling, nameof(Friends))
				.Values
				.SetTo(Friends);

			return this;
		}
	}

	[Test]
	public static void Cat_example() {
		var a = new Cat(1) { Name = "A", PurrPower = 0.10, Friends = {
			new Cat(3) { Name = "C" }
		} };

		var b = new Cat(2) { Name = "B", PurrPower = 0.20, Friends = {
			new Cat(3) { PurrPower = 0.30, Friends = {
				new Cat(4) { Name = "D", PurrPower = 0.40 }
			} }
		} };

		a.Assign(b); /* a == new Cat(1) { Name = "B", PurrPower = 0.20, Friends = {
			new Cat(3) { Name = "C", PurrPower = 0.30, Friends = {
				new Cat(4) { Name = "D", PurrPower = 0.40 }
			} }
		} }; */

		Assert.AreEqual(1, a.Id);
		Assert.AreEqual("B", a.Name);
		Assert.AreEqual(0.20, a.PurrPower);
		Assert.AreEqual(1, a.Friends.Count);
		Assert.AreEqual(3, a.Friends[0].Id);
		Assert.AreEqual("C", a.Friends[0].Name);
		Assert.AreEqual(0.30, a.Friends[0].PurrPower);
		Assert.AreEqual(1, a.Friends[0].Friends.Count);
		Assert.AreEqual(4, a.Friends[0].Friends[0].Id);
		Assert.AreEqual("D", a.Friends[0].Friends[0].Name);
		Assert.AreEqual(0.40, a.Friends[0].Friends[0].PurrPower);
		Assert.AreEqual(0, a.Friends[0].Friends[0].Friends.Count);
	}
}
