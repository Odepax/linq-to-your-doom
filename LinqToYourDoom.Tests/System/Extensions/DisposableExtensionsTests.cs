using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.System.Extensions;

sealed class DummyDisposable : IDisposable {
	public bool IsDisposed { get; private set; }
	public void Dispose() => IsDisposed = true;
}

static class DisposableExtensionsTests {
	// IEnumerable<T> where T : IDisposable
	// ----

	[Test]
	public static void IEnumerable_TDisposable_DisposeAll() {
		new[] {
			new DummyDisposable().Tee(out var a),
			new DummyDisposable().Tee(out var b),
			new DummyDisposable().Tee(out var c)
		}
			.DisposeAll();

		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	[Test]
	public static void IEnumerable_TDisposable_DisposeAll_predicate() {
		new[] {
			new DummyDisposable().Tee(out var a),
			new DummyDisposable().Tee(out var b),
			new DummyDisposable().Tee(out var c)
		}
			.DisposeAll(it => it != b);

		Assert.IsTrue(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	// IReadOnlyDictionary<TKey, TValue> where TValue : IDisposable
	// ----

	[Test]
	public static void IReadOnlyDictionary_TDisposable_DisposeAll() {
		new Dictionary<char, DummyDisposable> {
			['A'] = new DummyDisposable().Tee(out var a),
			['B'] = new DummyDisposable().Tee(out var b),
			['C'] = new DummyDisposable().Tee(out var c)
		}
			.DisposeAll();

		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	[Test]
	public static void IReadOnlyDictionary_TDisposable_DisposeAll_predicate_1() {
		new Dictionary<char, DummyDisposable> {
			['A'] = new DummyDisposable().Tee(out var a),
			['B'] = new DummyDisposable().Tee(out var b),
			['C'] = new DummyDisposable().Tee(out var c)
		}
			.DisposeAll(value => value != b);

		Assert.IsTrue(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	[Test]
	public static void IReadOnlyDictionary_TDisposable_DisposeAll_predicate_2() {
		new Dictionary<char, DummyDisposable> {
			['A'] = new DummyDisposable().Tee(out var a),
			['B'] = new DummyDisposable().Tee(out var b),
			['C'] = new DummyDisposable().Tee(out var c)
		}
			.DisposeAll((key, value) => key != 'B');

		Assert.IsTrue(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	// IEnumerable<T>
	// ----

	[Test]
	public static void IEnumerable_T_DisposeAll_selector() {
		new[] {
			('A', new DummyDisposable().Tee(out var a)),
			('B', new DummyDisposable().Tee(out var b)),
			('C', new DummyDisposable().Tee(out var c))
		}
			.DisposeAll(it => it.Item2);

		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	[Test]
	public static void IEnumerable_T_DisposeAll_trySelector_1() {
		new[] {
			('A', new DummyDisposable().Tee(out var a)),
			('B', new DummyDisposable().Tee(out var b)),
			('C', new DummyDisposable().Tee(out var c))
		}
			.DisposeAll(TryFilter);

		static bool TryFilter((char, DummyDisposable) it, out IDisposable @out) {
			@out = it.Item2;
			return it.Item1 != 'B';
		}

		Assert.IsTrue(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	[Test]
	public static void IEnumerable_T_DisposeAll_trySelector_2() {
		new[] {
			('A', new DummyDisposable().Tee(out var a)),
			('B', new DummyDisposable().Tee(out var b)),
			('C', new DummyDisposable().Tee(out var c))
		}
			.DisposeAll(it => (it.Item1 != 'B', it.Item2));

		Assert.IsTrue(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	// IReadOnlyDictionary<TKey, TValue>
	// ----

	[Test]
	public static void IReadOnlyDictionary_T_DisposeAll_selector() {
		new Dictionary<char, (int, DummyDisposable)> {
			['A'] = (1, new DummyDisposable().Tee(out var a)),
			['B'] = (2, new DummyDisposable().Tee(out var b)),
			['C'] = (3, new DummyDisposable().Tee(out var c))
		}
			.DisposeAll((key, value) => value.Item2);

		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	[Test]
	public static void IReadOnlyDictionary_T_DisposeAll_trySelector_1() {
		new Dictionary<char, (int, DummyDisposable)> {
			['A'] = (1, new DummyDisposable().Tee(out var a)),
			['B'] = (2, new DummyDisposable().Tee(out var b)),
			['C'] = (3, new DummyDisposable().Tee(out var c))
		}
			.DisposeAll(TryFilter);

		static bool TryFilter(char key, (int, DummyDisposable) value, out IDisposable @out) {
			@out = value.Item2;
			return key!= 'B';
		}

		Assert.IsTrue(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	[Test]
	public static void IReadOnlyDictionary_T_DisposeAll_trySelector_2() {
		new Dictionary<char, (int, DummyDisposable)> {
			['A'] = (1, new DummyDisposable().Tee(out var a)),
			['B'] = (2, new DummyDisposable().Tee(out var b)),
			['C'] = (3, new DummyDisposable().Tee(out var c))
		}
			.DisposeAll((key, value) => (key != 'B', value.Item2));

		Assert.IsTrue(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
	}

	// ICollection<T> where T : IDisposable
	// ----

	[Test]
	public static void ICollection_TDisposable_RemoveAndDispose() {
		new List<DummyDisposable> {
			new DummyDisposable().Tee(out var a),
			new DummyDisposable().Tee(out var b),
			new DummyDisposable().Tee(out var c)
		}
			.Tee(out var list)
			.RemoveAndDispose(b);

		Assert.IsFalse(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsFalse(c.IsDisposed);

		Assert.AreEqual(new[] { a, c }, list);
	}
	
	// IList<T> where T : IDisposable
	// ----

	[Test]
	public static void IList_TDisposable_RemoveAtAndDispose() {
		new List<DummyDisposable> {
			new DummyDisposable().Tee(out var a),
			new DummyDisposable().Tee(out var b),
			new DummyDisposable().Tee(out var c)
		}
			.Tee(out var list)
			.RemoveAtAndDispose(1);

		Assert.IsFalse(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsFalse(c.IsDisposed);

		Assert.AreEqual(new[] { a, c }, list);
	}

	[Test]
	public static void IList_TDisposable_SetAndDispose() {
		new List<DummyDisposable> {
			new DummyDisposable().Tee(out var a),
			new DummyDisposable().Tee(out var b),
			new DummyDisposable().Tee(out var c)
		}
			.Tee(out var list)
			.SetAndDispose(1, new DummyDisposable().Tee(out var d));

		Assert.IsFalse(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsFalse(c.IsDisposed);
		Assert.IsFalse(d.IsDisposed);

		Assert.AreEqual(new[] { a, d, c }, list);
	}

	// IDictionary<TKey, TValue> where TValue : IDisposable
	// ----

	[Test]
	public static void IDictionary_TDisposable_RemoveAndDispose() {
		new Dictionary<char, DummyDisposable> {
			['A'] = new DummyDisposable().Tee(out var a),
			['B'] = new DummyDisposable().Tee(out var b),
			['C'] = new DummyDisposable().Tee(out var c)
		}
			.Tee(out var dictionary)
			.RemoveAndDispose('B');

		Assert.IsFalse(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsFalse(c.IsDisposed);

		Assert.AreEqual(new[] { KeyValuePair.Create('A', a), KeyValuePair.Create('C', c) }, dictionary);
	}

	[Test]
	public static void IDictionary_TDisposable_RemoveAndDispose_non_existent() {
		new Dictionary<char, DummyDisposable>()
			.Tee(out var dictionary)
			.RemoveAndDispose('X');

		Assert.IsEmpty(dictionary);
	}

	[Test]
	public static void IDictionary_TDisposable_SetAndDispose() {
		new Dictionary<char, DummyDisposable> {
			['A'] = new DummyDisposable().Tee(out var a),
			['B'] = new DummyDisposable().Tee(out var b),
			['C'] = new DummyDisposable().Tee(out var c)
		}
			.Tee(out var dictionary)
			.SetAndDispose('B', new DummyDisposable().Tee(out var d));

		Assert.IsFalse(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsFalse(c.IsDisposed);
		Assert.IsFalse(d.IsDisposed);

		Assert.AreEqual(new[] { KeyValuePair.Create('A', a), KeyValuePair.Create('B', d), KeyValuePair.Create('C', c) }, dictionary);
	}

	[Test]
	public static void IDictionary_TDisposable_SetAndDispose_non_existent() {
		new Dictionary<char, DummyDisposable>()
			.Tee(out var dictionary)
			.SetAndDispose('X', new DummyDisposable().Tee(out var x));

		Assert.IsFalse(x.IsDisposed);

		Assert.AreEqual(new[] { KeyValuePair.Create('X', x) }, dictionary);
	}

	// ICollection<T>
	// ----

	[Test]
	public static void ICollection_T_RemoveAndTryDispose() {
		new List<(int, DummyDisposable)> {
			(1, new DummyDisposable().Tee(out var a)),
			(2, new DummyDisposable().Tee(out var b)),
			(3, new DummyDisposable().Tee(out var c))
		}
			.Tee(out var list)
			.RemoveAndTryDispose((2, b));

		Assert.IsFalse(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed); // b is disposable, but the tuple is not!
		Assert.IsFalse(c.IsDisposed);

		Assert.AreEqual(new[] { (1, a), (3, c) }, list);
	}

	// IList<T>
	// ----

	[Test]
	public static void IList_T_RemoveAtAndTryDispose() {
		new List<(int, DummyDisposable)> {
			(1, new DummyDisposable().Tee(out var a)),
			(2, new DummyDisposable().Tee(out var b)),
			(3, new DummyDisposable().Tee(out var c))
		}
			.Tee(out var list)
			.RemoveAtAndTryDispose(1);

		Assert.IsFalse(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed); // b is disposable, but the tuple is not!
		Assert.IsFalse(c.IsDisposed);

		Assert.AreEqual(new[] { (1, a), (3, c) }, list);
	}

	[Test]
	public static void IList_T_SetAndTryDispose() {
		new List<(int, DummyDisposable)> {
			(1, new DummyDisposable().Tee(out var a)),
			(2, new DummyDisposable().Tee(out var b)),
			(3, new DummyDisposable().Tee(out var c))
		}
			.Tee(out var list)
			.SetAndTryDispose(1, (4, new DummyDisposable().Tee(out var d)));

		Assert.IsFalse(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed); // b is disposable, but the tuple is not!
		Assert.IsFalse(c.IsDisposed);
		Assert.IsFalse(d.IsDisposed);

		Assert.AreEqual(new[] { (1, a), (4, d), (3, c) }, list);
	}

	// IDictionary<TKey, TValue>
	// ----

	[Test]
	public static void IDictionary_T_RemoveAndTryDispose() {
		new Dictionary<char, (int, DummyDisposable)> {
			['A'] = (1, new DummyDisposable().Tee(out var a)),
			['B'] = (2, new DummyDisposable().Tee(out var b)),
			['C'] = (3, new DummyDisposable().Tee(out var c))
		}
			.Tee(out var dictionary)
			.RemoveAndTryDispose('B');

		Assert.IsFalse(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed); // b is disposable, but the tuple is not!
		Assert.IsFalse(c.IsDisposed);

		Assert.AreEqual(new[] { KeyValuePair.Create('A', (1, a)), KeyValuePair.Create('C', (3, c)) }, dictionary);
	}

	[Test]
	public static void IDictionary_T_RemoveAndTryDispose_non_existent() {
		new Dictionary<char, (int, DummyDisposable)>()
			.Tee(out var dictionary)
			.RemoveAndTryDispose('X');

		Assert.IsEmpty(dictionary);
	}

	[Test]
	public static void IDictionary_T_SetAndTryDispose() {
		new Dictionary<char, (int, DummyDisposable)> {
			['A'] = (1, new DummyDisposable().Tee(out var a)),
			['B'] = (2, new DummyDisposable().Tee(out var b)),
			['C'] = (3, new DummyDisposable().Tee(out var c))
		}
			.Tee(out var dictionary)
			.SetAndTryDispose('B', (4, new DummyDisposable().Tee(out var d)));

		Assert.IsFalse(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed); // b is disposable, but the tuple is not!
		Assert.IsFalse(c.IsDisposed);
		Assert.IsFalse(d.IsDisposed);

		Assert.AreEqual(new[] { KeyValuePair.Create('A', (1, a)), KeyValuePair.Create('B', (4, d)), KeyValuePair.Create('C', (3, c)) }, dictionary);
	}

	[Test]
	public static void IDictionary_T_SetAndDisTrypose_non_existent() {
		new Dictionary<char, (int, DummyDisposable)>()
			.Tee(out var dictionary)
			.SetAndTryDispose('X', (0, new DummyDisposable().Tee(out var x)));

		Assert.IsFalse(x.IsDisposed);

		Assert.AreEqual(new[] { KeyValuePair.Create('X', (0, x)) }, dictionary);
	}

	// Legacy
	// ----

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
			.Tee(out var list);

		list.DisposeAll(it => it.Item2);
		list.Clear();

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
			.Tee(out var dictionary);

		dictionary.DisposeAll();
		dictionary.Clear();

		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);

		Assert.IsEmpty(dictionary);
	}
	
	[Test]
	public static void DisposeAll_IDictionary_selector() {
		new Dictionary<char, DummyDisposable>(3) {
			['A'] = new DummyDisposable().Tee(out var a),
			['B'] = new DummyDisposable().Tee(out var b),
			['C'] = new DummyDisposable().Tee(out var c)
		}
			.Tee(out var dictionary);

		dictionary.DisposeAll(it => it.Value);
		dictionary.Clear();

		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);

		Assert.IsEmpty(dictionary);
	}

	[Test]
	public static void Set_Remove_Clear_AndDispose_IDictionary() {
		var a = new DummyDisposable();
		var b = new DummyDisposable();
		var c = new DummyDisposable();
		var d = new DummyDisposable();

		var dictionary = new Dictionary<int, object> {
			[1] = a,
			[2] = b,
			[3] = c
		};

		dictionary.SetAndTryDispose(1, d);

		Assert.AreEqual(new[] { d, b, c }, dictionary.Values);
		Assert.IsTrue(a.IsDisposed);
		Assert.IsFalse(b.IsDisposed);
		Assert.IsFalse(c.IsDisposed);
		Assert.IsFalse(d.IsDisposed);

		dictionary.RemoveAndTryDispose(2);

		Assert.AreEqual(new[] { d, c }, dictionary.Values);
		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsFalse(c.IsDisposed);
		Assert.IsFalse(d.IsDisposed);

		dictionary.TryDisposeAll();
		dictionary.Clear();

		Assert.IsEmpty(dictionary);
		Assert.IsTrue(a.IsDisposed);
		Assert.IsTrue(b.IsDisposed);
		Assert.IsTrue(c.IsDisposed);
		Assert.IsTrue(d.IsDisposed);
	}
}
