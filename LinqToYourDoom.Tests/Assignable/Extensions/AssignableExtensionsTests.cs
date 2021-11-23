using NUnit.Framework;

namespace LinqToYourDoom.Tests.Assignable.Extensions;

static class AssignableExtensionsTests {
	sealed class Level1 : IAssignable<Level1, Level1> {
		public Level2 Sub { get; }
		public Level1(Level2 sub) => Sub = sub;

		public Level1 Assign(Level1 other, ConflictHandling conflictHandling = default) {
			Sub.Assign(other.Sub, conflictHandling, nameof(Sub));

			return this;
		}
	}

	sealed class Level2 : IAssignable<Level2, Level2> {
		public int Id { get; set; }
		public Level2(int id) => Id = id;

		public Level2 Assign(Level2 other, ConflictHandling conflictHandling = default) {
			Id = Id.Assign(other.Id, conflictHandling, MathD.Avg, nameof(Id));

			return this;
		}
	}

	[Test]
	public static void Assign() {
		var a = new Level1(new Level2(1));
		var b = new Level1(new Level2(2));

		var exception = Assert.Throws<AssignConflictException>(() => a.Assign(b, ConflictHandling.Throw));
		Assert.AreEqual("Sub.Id", exception.Path);

		exception = Assert.Throws<AssignConflictException>(() => a.Assign(b, ConflictHandling.Throw, "@var"));
		Assert.AreEqual("@var.Sub.Id", exception.Path);
	}
}
