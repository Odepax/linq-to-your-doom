using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Math {
	static class RandomDTests {
		const int N = 10_000;
		const double M = 0.03d; // Accept 3% diversion from perfect uniformity.

		[Test]
		public static void In() {
			var values = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
			var tossedValues = new Dictionary<char, int>();

			for (var i = 0; i < values.Count * N; ++i)
				tossedValues[MathD.Random.In(values).ToVariable(out var value)] = tossedValues.GetValueOrDefault(value, 0) + 1;

			// Distribution is uniform.
			foreach (var (value, count) in tossedValues)
				Assert.AreEqual(count, N, N * M, value + " is out of uniformity.");

			// Original collection is untouched.
			Assert.AreEqual(new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' }, values);
		}
		
		[Test]
		public static void In_empty() =>
			Assert.Throws<ArgumentException>(() => MathD.Random.In(Array.Empty<char>()));

		[Test]
		public static void TryIn() {
			var values = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
			var tossedValues = new Dictionary<char, int>();

			for (var i = 0; i < values.Count * N; ++i) {
				Assert.IsTrue(MathD.Random.TryIn(values, out var value));
				tossedValues[value] = tossedValues.GetValueOrDefault(value, 0) + 1;
			}

			// Distribution is uniform.
			foreach (var (value, count) in tossedValues)
				Assert.AreEqual(count, N, N * M, value + " is out of uniformity.");

			// Original collection is untouched.
			Assert.AreEqual(new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' }, values);
		}
		
		[Test]
		public static void TryIn_empty() =>
			Assert.IsFalse(MathD.Random.TryIn(Array.Empty<char>(), out _));

		[Test]
		public static void Pop() {
			var values = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
			var value = MathD.Random.Pop(values);

			// Selected item is removed from original collection.
			Assert.AreEqual(6, values.Count);
			Assert.IsFalse(values.Contains(value));
		}

		[Test]
		public static void Pop_empty() =>
			Assert.Throws<ArgumentException>(() => MathD.Random.Pop(Array.Empty<char>()));
		
		[Test]
		public static void TryPop() {
			var values = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

			// Selected item is removed from original collection.
			Assert.IsTrue(MathD.Random.TryPop(values, out var value));
			Assert.AreEqual(6, values.Count);
			Assert.IsFalse(values.Contains(value));
		}
		
		[Test]
		public static void TryPop_empty() =>
			Assert.IsFalse(MathD.Random.TryPop(Array.Empty<char>(), out _));

		[Test]
		[TestCase(-120, +120, 0d)]
		[TestCase(0, 2, 0.5d)]
		[TestCase(0, 64, 31.5d)]
		public static void Between(sbyte min, sbyte max, double expectedMean) {
			var sum = 0d;

			for (var i = 0; i < N; ++i)
				sum += MathD.Random.Between(min, max);

			var actualMean = sum / (double) N;

			Assert.AreEqual(expectedMean, actualMean, ((double) max - min) * M);
		}

		[Test]
		[TestCase(1, 101, 50.5d)]
		[TestCase(10, 1000, 504.5d)]
		[TestCase(0, 2, 0.5d)]
		[TestCase(-1, +2, 0d)]
		public static void Between(int min, int max, double expectedMean) {
			var sum = 0d;

			for (var i = 0; i < N; ++i)
				sum += MathD.Random.Between(min, max);

			var actualMean = sum / (double) N;

			Assert.AreEqual(expectedMean, actualMean, ((double) max - min) * M);
		}

		[Test]
		[TestCase(0u, 2u, 0.5d)]
		[TestCase(1u, 101u, 50.5d)]
		public static void Between(uint min, uint max, double expectedMean) {
			var sum = 0d;

			for (var i = 0; i < N; ++i)
				sum += MathD.Random.Between(min, max);

			var actualMean = sum / (double) N;

			Assert.AreEqual(expectedMean, actualMean, ((double) max - min) * M);
		}

		[Test]
		[TestCase(-1f, +1f)]
		[TestCase(-10f, +10f)]
		[TestCase(-100f, +100f)]
		[TestCase(0f, 1f)]
		[TestCase(0f, 1000f)]
		public static void Between(float min, float max) {
			var expectedMean = ((double) min + max) / 2d;
			var sum = 0d;

			for (var i = 0; i < N; ++i)
				sum += MathD.Random.Between(min, max);

			var actualMean = sum / (double) N;

			Assert.AreEqual(expectedMean, actualMean, ((double) max - min) * M);
		}

		[Test]
		public static void Loose_methods() {
			Assert.Throws<ArgumentException>(() => MathD.Random.Between(10f, 1f, ArgumentValidation.Strict));
			Assert.IsTrue(MathD.Random.Between(10f, 1f, ArgumentValidation.Lenient) is < 10f and >= 1f);

			for (var i = 0; i < N; ++i)
				Assert.AreEqual(+1, MathD.Random.Sign(1f));

			for (var i = 0; i < N; ++i)
				Assert.AreEqual(-1, MathD.Random.Sign(0f));
		}
	}
}
