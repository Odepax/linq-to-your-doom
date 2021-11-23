using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Linq.Extensions;

static class DictionaryExtensionsTests {
	[Test]
	public static void GetValueOrDefault_factory() {
		var dictionary = new Dictionary<int, int>() { [42] = 2048 };
		var invokeCount = 0;

		Assert.AreEqual(2048, dictionary.GetValueOrDefault(42, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(0, invokeCount);

		Assert.AreEqual(100, dictionary.GetValueOrDefault(404, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(1, invokeCount);
	}

	[Test]
	public static void GetValueOrSet_value() {
		var dictionary = new Dictionary<int, int>() { [42] = 2048 };

		Assert.AreEqual(2048, dictionary.GetValueOrSet(42, 100));
		Assert.AreEqual(2048, dictionary[42]);

		Assert.AreEqual(100, dictionary.GetValueOrSet(404, 100));
		Assert.AreEqual(100, dictionary[404]);
	}

	[Test]
	public static void GetValueOrSet_factory() {
		var dictionary = new Dictionary<int, int>() { [42] = 2048 };
		var invokeCount = 0;

		Assert.AreEqual(2048, dictionary.GetValueOrSet(42, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(0, invokeCount);
		Assert.AreEqual(2048, dictionary[42]);

		Assert.AreEqual(100, dictionary.GetValueOrSet(404, () => { ++invokeCount; return 100; }));
		Assert.AreEqual(1, invokeCount);
		Assert.AreEqual(100, dictionary[404]);
	}

	class Sample {
		public int Value;
	}

	[Test]
	public static void GetValueOrSet_action() {
		var dictionary = new Dictionary<int, Sample>() { [42] = new Sample { Value = 2048 } };
		var invokeCount = 0;

		Assert.AreEqual(2048, dictionary.GetValueOrSet(42, sample => { ++invokeCount; sample.Value = 100; }).Value);
		Assert.AreEqual(0, invokeCount);
		Assert.AreEqual(2048, dictionary[42].Value);

		Assert.AreEqual(100, dictionary.GetValueOrSet(404, sample => { ++invokeCount; sample.Value = 100; }).Value);
		Assert.AreEqual(1, invokeCount);
		Assert.AreEqual(100, dictionary[404].Value);
	}

	[Test]
	public static void OrderBy() {
		var dictionary = new Dictionary<int, char>() {
			[1] = 'B',
			[4] = 'D',
			[2] = 'C',
			[3] = 'A'
		};

		Assert.AreEqual(new Dictionary<int, char>() {
			[1] = 'B',
			[2] = 'C',
			[3] = 'A',
			[4] = 'D'
		}, dictionary.OrderByKey());

		Assert.AreEqual(new Dictionary<int, char>() {
			[4] = 'D',
			[3] = 'A',
			[2] = 'C',
			[1] = 'B'
		}, dictionary.OrderByKeyDescending());

		Assert.AreEqual(new Dictionary<int, char>() {
			[3] = 'A',
			[1] = 'B',
			[2] = 'C',
			[4] = 'D'
		}, dictionary.OrderByValue());

		Assert.AreEqual(new Dictionary<int, char>() {
			[4] = 'D',
			[2] = 'C',
			[1] = 'B',
			[3] = 'A'
		}, dictionary.OrderByValueDescending());

		Assert.AreEqual(new Dictionary<int, char>() {
			[4] = 'D',
			[3] = 'A',
			[1] = 'B',
			[2] = 'C'
		}, dictionary.OrderByKey(k => global::System.Math.Sin(k)));

		Assert.AreEqual(new Dictionary<int, char>() {
			[2] = 'C',
			[1] = 'B',
			[3] = 'A',
			[4] = 'D'
		}, dictionary.OrderByKeyDescending(k => global::System.Math.Sin(k)));

		Assert.AreEqual(new Dictionary<int, char>() {
			[4] = 'D',
			[2] = 'C',
			[1] = 'B',
			[3] = 'A'
		}, dictionary.OrderByValue(v => v * -1));

		Assert.AreEqual(new Dictionary<int, char>() {
			[3] = 'A',
			[1] = 'B',
			[2] = 'C',
			[4] = 'D'
		}, dictionary.OrderByValueDescending(v => v * -1));
	}
}
