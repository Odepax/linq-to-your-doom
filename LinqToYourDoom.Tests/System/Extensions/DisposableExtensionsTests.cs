using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.System.Extensions;

sealed class DummyDisposable : IDisposable {
	public bool IsDisposed { get; private set; }
	public void Dispose() => IsDisposed = true;
}

static class DisposableExtensionsTests {
	[Test]
	public static void DisposeAll() {
		new List<DummyDisposable>(3) {
			new DummyDisposable().Tee(out var a),
			new DummyDisposable().Tee(out var b),
			new DummyDisposable().Tee(out var c)
		}
			.Tee(out var list)
			.DisposeAll()
			.Clear();

		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);

		Assert.IsEmpty(list);
	}

	[Test]
	public static void DisposeAll_IEnumerable_eager() {
		var a = new DummyDisposable();
		var b = new DummyDisposable();
		var c = new DummyDisposable();

		IEnumerable<(char, DummyDisposable)> list() {
			yield return ('A', a);
			yield return ('B', b);
			yield return ('C', c);
		}

		list().DisposeAll(it => it.Item2);

		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	[Test]
	public static void DisposeAll_IEnumerable_lazy() {
		var a = new DummyDisposable();
		var b = new DummyDisposable();
		var c = new DummyDisposable();

		IEnumerable<(char, DummyDisposable)> list() {
			yield return ('A', a);
			yield return ('B', b);
			yield return ('C', c);
		}

		var enumerable = list().Each(it => it.Item2.Dispose());

		Assert.IsFalse(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed);
		Assert.IsFalse(c.IsDisposed);

		enumerable.Enumerate();

		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	[Test]
	public static void DisposeAll_ICollection() {
		new List<(char, DummyDisposable)>(3) {
			('A', new DummyDisposable().Tee(out var a)),
			('B', new DummyDisposable().Tee(out var b)),
			('C', new DummyDisposable().Tee(out var c))
		}
			.Tee(out var list)
			.DisposeAll(it => it.Item2)
			.Clear();

		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);

		Assert.IsEmpty(list);
	}

	[Test]
	public static void DisposeAll_IDictionary() {
		new Dictionary<char, DummyDisposable>(3) {
			['A'] = new DummyDisposable().Tee(out var a),
			['B'] = new DummyDisposable().Tee(out var b),
			['C'] = new DummyDisposable().Tee(out var c)
		}
			.Tee(out var dictionary)
			.DisposeAll(it => it.Value)
			.Clear();

		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);

		Assert.IsEmpty(dictionary);
	}
}
