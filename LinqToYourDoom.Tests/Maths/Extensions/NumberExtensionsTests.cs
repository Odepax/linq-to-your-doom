using System;
using LinqToYourDoom.Maths.Extensions;
using LinqToYourDoom.System;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.Maths.Extensions {
	static class NumberExtensionsTests {
		[Test]
		public static void CoerceTo_() {
			Assert.AreEqual(float.MinValue, double.MinValue.CoerceToFloat());
			Assert.AreEqual(float.MaxValue, double.MaxValue.CoerceToFloat());
			Assert.AreEqual(decimal.MinValue, double.MinValue.CoerceToDecimal());
			Assert.AreEqual(decimal.MaxValue, double.MaxValue.CoerceToDecimal());
			Assert.AreEqual(ulong.MinValue, double.MinValue.CoerceToULong());
			Assert.AreEqual(ulong.MaxValue, double.MaxValue.CoerceToULong());
			Assert.AreEqual(long.MinValue, double.MinValue.CoerceToLong());
			Assert.AreEqual(long.MaxValue, double.MaxValue.CoerceToLong());
			Assert.AreEqual(uint.MinValue, double.MinValue.CoerceToUInt());
			Assert.AreEqual(uint.MaxValue, double.MaxValue.CoerceToUInt());
			Assert.AreEqual(int.MinValue, double.MinValue.CoerceToInt());
			Assert.AreEqual(int.MaxValue, double.MaxValue.CoerceToInt());
			Assert.AreEqual(ushort.MinValue, double.MinValue.CoerceToUShort());
			Assert.AreEqual(ushort.MaxValue, double.MaxValue.CoerceToUShort());
			Assert.AreEqual(short.MinValue, double.MinValue.CoerceToShort());
			Assert.AreEqual(short.MaxValue, double.MaxValue.CoerceToShort());
			Assert.AreEqual(byte.MinValue, double.MinValue.CoerceToByte());
			Assert.AreEqual(byte.MaxValue, double.MaxValue.CoerceToByte());
			Assert.AreEqual(sbyte.MinValue, double.MinValue.CoerceToSByte());
			Assert.AreEqual(sbyte.MaxValue, double.MaxValue.CoerceToSByte());

			Assert.AreEqual(decimal.MinValue, float.MinValue.CoerceToDecimal());
			Assert.AreEqual(decimal.MaxValue, float.MaxValue.CoerceToDecimal());
			Assert.AreEqual(ulong.MinValue, float.MinValue.CoerceToULong());
			Assert.AreEqual(ulong.MaxValue, float.MaxValue.CoerceToULong());
			Assert.AreEqual(long.MinValue, float.MinValue.CoerceToLong());
			Assert.AreEqual(long.MaxValue, float.MaxValue.CoerceToLong());
			Assert.AreEqual(uint.MinValue, float.MinValue.CoerceToUInt());
			Assert.AreEqual(uint.MaxValue, float.MaxValue.CoerceToUInt());
			Assert.AreEqual(int.MinValue, float.MinValue.CoerceToInt());
			Assert.AreEqual(int.MaxValue, float.MaxValue.CoerceToInt());
			Assert.AreEqual(ushort.MinValue, float.MinValue.CoerceToUShort());
			Assert.AreEqual(ushort.MaxValue, float.MaxValue.CoerceToUShort());
			Assert.AreEqual(short.MinValue, float.MinValue.CoerceToShort());
			Assert.AreEqual(short.MaxValue, float.MaxValue.CoerceToShort());
			Assert.AreEqual(byte.MinValue, float.MinValue.CoerceToByte());
			Assert.AreEqual(byte.MaxValue, float.MaxValue.CoerceToByte());
			Assert.AreEqual(sbyte.MinValue, float.MinValue.CoerceToSByte());
			Assert.AreEqual(sbyte.MaxValue, float.MaxValue.CoerceToSByte());

			Assert.AreEqual(ulong.MinValue, decimal.MinValue.CoerceToULong());
			Assert.AreEqual(ulong.MaxValue, decimal.MaxValue.CoerceToULong());
			Assert.AreEqual(long.MinValue, decimal.MinValue.CoerceToLong());
			Assert.AreEqual(long.MaxValue, decimal.MaxValue.CoerceToLong());
			Assert.AreEqual(uint.MinValue, decimal.MinValue.CoerceToUInt());
			Assert.AreEqual(uint.MaxValue, decimal.MaxValue.CoerceToUInt());
			Assert.AreEqual(int.MinValue, decimal.MinValue.CoerceToInt());
			Assert.AreEqual(int.MaxValue, decimal.MaxValue.CoerceToInt());
			Assert.AreEqual(ushort.MinValue, decimal.MinValue.CoerceToUShort());
			Assert.AreEqual(ushort.MaxValue, decimal.MaxValue.CoerceToUShort());
			Assert.AreEqual(short.MinValue, decimal.MinValue.CoerceToShort());
			Assert.AreEqual(short.MaxValue, decimal.MaxValue.CoerceToShort());
			Assert.AreEqual(byte.MinValue, decimal.MinValue.CoerceToByte());
			Assert.AreEqual(byte.MaxValue, decimal.MaxValue.CoerceToByte());
			Assert.AreEqual(sbyte.MinValue, decimal.MinValue.CoerceToSByte());
			Assert.AreEqual(sbyte.MaxValue, decimal.MaxValue.CoerceToSByte());

			Assert.AreEqual((long) 0, ulong.MinValue.CoerceToLong());
			Assert.AreEqual(long.MaxValue, ulong.MaxValue.CoerceToLong());
			Assert.AreEqual(uint.MinValue, ulong.MinValue.CoerceToUInt());
			Assert.AreEqual(uint.MaxValue, ulong.MaxValue.CoerceToUInt());
			Assert.AreEqual((int) 0, ulong.MinValue.CoerceToInt());
			Assert.AreEqual(int.MaxValue, ulong.MaxValue.CoerceToInt());
			Assert.AreEqual(ushort.MinValue, ulong.MinValue.CoerceToUShort());
			Assert.AreEqual(ushort.MaxValue, ulong.MaxValue.CoerceToUShort());
			Assert.AreEqual((short) 0, ulong.MinValue.CoerceToShort());
			Assert.AreEqual(short.MaxValue, ulong.MaxValue.CoerceToShort());
			Assert.AreEqual(byte.MinValue, ulong.MinValue.CoerceToByte());
			Assert.AreEqual(byte.MaxValue, ulong.MaxValue.CoerceToByte());
			Assert.AreEqual((sbyte) 0, ulong.MinValue.CoerceToSByte());
			Assert.AreEqual(sbyte.MaxValue, ulong.MaxValue.CoerceToSByte());

			Assert.AreEqual(ulong.MinValue, long.MinValue.CoerceToULong());
			Assert.AreEqual(long.MaxValue, long.MaxValue.CoerceToULong());
			Assert.AreEqual(uint.MinValue, long.MinValue.CoerceToUInt());
			Assert.AreEqual(uint.MaxValue, long.MaxValue.CoerceToUInt());
			Assert.AreEqual(int.MinValue, long.MinValue.CoerceToInt());
			Assert.AreEqual(int.MaxValue, long.MaxValue.CoerceToInt());
			Assert.AreEqual(ushort.MinValue, long.MinValue.CoerceToUShort());
			Assert.AreEqual(ushort.MaxValue, long.MaxValue.CoerceToUShort());
			Assert.AreEqual(short.MinValue, long.MinValue.CoerceToShort());
			Assert.AreEqual(short.MaxValue, long.MaxValue.CoerceToShort());
			Assert.AreEqual(byte.MinValue, long.MinValue.CoerceToByte());
			Assert.AreEqual(byte.MaxValue, long.MaxValue.CoerceToByte());
			Assert.AreEqual(sbyte.MinValue, long.MinValue.CoerceToSByte());
			Assert.AreEqual(sbyte.MaxValue, long.MaxValue.CoerceToSByte());

			Assert.AreEqual((int) 0, uint.MinValue.CoerceToInt());
			Assert.AreEqual(int.MaxValue, uint.MaxValue.CoerceToInt());
			Assert.AreEqual(ushort.MinValue, uint.MinValue.CoerceToUShort());
			Assert.AreEqual(ushort.MaxValue, uint.MaxValue.CoerceToUShort());
			Assert.AreEqual((short) 0, uint.MinValue.CoerceToShort());
			Assert.AreEqual(short.MaxValue, uint.MaxValue.CoerceToShort());
			Assert.AreEqual(byte.MinValue, uint.MinValue.CoerceToByte());
			Assert.AreEqual(byte.MaxValue, uint.MaxValue.CoerceToByte());
			Assert.AreEqual((sbyte) 0, uint.MinValue.CoerceToSByte());
			Assert.AreEqual(sbyte.MaxValue, uint.MaxValue.CoerceToSByte());

			Assert.AreEqual(uint.MinValue, int.MinValue.CoerceToUInt());
			Assert.AreEqual(int.MaxValue, int.MaxValue.CoerceToUInt());
			Assert.AreEqual(ushort.MinValue, int.MinValue.CoerceToUShort());
			Assert.AreEqual(ushort.MaxValue, int.MaxValue.CoerceToUShort());
			Assert.AreEqual(short.MinValue, int.MinValue.CoerceToShort());
			Assert.AreEqual(short.MaxValue, int.MaxValue.CoerceToShort());
			Assert.AreEqual(byte.MinValue, int.MinValue.CoerceToByte());
			Assert.AreEqual(byte.MaxValue, int.MaxValue.CoerceToByte());
			Assert.AreEqual(sbyte.MinValue, int.MinValue.CoerceToSByte());
			Assert.AreEqual(sbyte.MaxValue, int.MaxValue.CoerceToSByte());

			Assert.AreEqual((short) 0, ushort.MinValue.CoerceToShort());
			Assert.AreEqual(short.MaxValue, ushort.MaxValue.CoerceToShort());
			Assert.AreEqual(byte.MinValue, ushort.MinValue.CoerceToByte());
			Assert.AreEqual(byte.MaxValue, ushort.MaxValue.CoerceToByte());
			Assert.AreEqual((sbyte) 0, ushort.MinValue.CoerceToSByte());
			Assert.AreEqual(sbyte.MaxValue, ushort.MaxValue.CoerceToSByte());

			Assert.AreEqual(ushort.MinValue, short.MinValue.CoerceToUShort());
			Assert.AreEqual(short.MaxValue, short.MaxValue.CoerceToUShort());
			Assert.AreEqual(byte.MinValue, short.MinValue.CoerceToByte());
			Assert.AreEqual(byte.MaxValue, short.MaxValue.CoerceToByte());
			Assert.AreEqual(sbyte.MinValue, short.MinValue.CoerceToSByte());
			Assert.AreEqual(sbyte.MaxValue, short.MaxValue.CoerceToSByte());

			Assert.AreEqual((sbyte) 0, byte.MinValue.CoerceToSByte());
			Assert.AreEqual(sbyte.MaxValue, byte.MaxValue.CoerceToSByte());

			Assert.AreEqual(byte.MinValue, sbyte.MinValue.CoerceToByte());
			Assert.AreEqual(sbyte.MaxValue, sbyte.MaxValue.CoerceToByte());
		}

		[Test]
		public static void DivRem() {
			Assert.AreEqual((21, 0), 42.DivRem(2));
			Assert.AreEqual((4, 5), 33.DivRem(7));

			Assert.Throws<DivideByZeroException>(() => 12.DivRem(0, ArgumentValidation.Strict));
			Assert.AreEqual((12, 0), 12.DivRem(0, ArgumentValidation.Lenient));
		}
		
		[Test]
		[TestCase(-12)]
		[TestCase(0)]
		[TestCase(-1)]
		[TestCase(1)]
		[TestCase(42)]
		public static void Pattern_matching_learning_test(int x) {
			var a = x != -1 && x != 0 && x != +1;
			var b = x is not (-1) and not 0 and not (+1);
			var c = x is not -1 and not 0 and not +1;
			var d = x is not (-1 or 0 or +1);
			// NOPE x is not -1 or 0 or +1;

			Assert.AreEqual(a, b);
			Assert.AreEqual(a, c);
			Assert.AreEqual(a, d);
		}

		[Test]
		public static void Sign() {
			Assert.AreEqual(-1.0, -12.0.Sign());
			Assert.AreEqual(0.0, 0.0.Sign());
			Assert.AreEqual(+1.0, 0.0.Sign(+1.0));
			Assert.AreEqual(+1.0, 42.0.Sign());

			Assert.Throws<ArithmeticException>(() => double.NaN.Sign(0, ArgumentValidation.Strict));
			Assert.AreEqual(0.0, double.NaN.Sign(0, ArgumentValidation.Lenient));

			Assert.Throws<ArgumentOutOfRangeException>(() => 0.0.Sign(+12.0, ArgumentValidation.Strict));
			Assert.AreEqual(+1.0, 0.0.Sign(+12.0, ArgumentValidation.Lenient));
			Assert.AreEqual(-1.0, 0.0.Sign(-42.0, ArgumentValidation.Lenient));

			Assert.AreEqual(-1, ((long) -12).Sign());
			Assert.AreEqual(+1, ((long) 0).Sign(+1));
			Assert.AreEqual(+1, ((long) 42).Sign());

			Assert.AreEqual(-1, ((int) -12).Sign());
			Assert.AreEqual(+1, ((int) 0).Sign(+1));
			Assert.AreEqual(+1, ((int) 42).Sign());

			Assert.AreEqual(-1, ((short) -12).Sign());
			Assert.AreEqual(+1, ((short) 0).Sign(+1));
			Assert.AreEqual(+1, ((short) 42).Sign());

			Assert.AreEqual(-1, ((sbyte) -12).Sign());
			Assert.AreEqual(+1, ((sbyte) 0).Sign(+1));
			Assert.AreEqual(+1, ((sbyte) 42).Sign());
		}

		[Test]
		public static void Ceiling() {
			Assert.Throws<ArgumentOutOfRangeException>(() => 3.1415.Ceiling(-1, ArgumentValidation.Strict));
			Assert.AreEqual(4, 3.1415.Ceiling(0, ArgumentValidation.Strict));
			Assert.AreEqual(3.15, 3.1415.Ceiling(2, ArgumentValidation.Strict));
			Assert.Throws<ArgumentOutOfRangeException>(() => 3.1415.Ceiling(17, ArgumentValidation.Strict));

			Assert.AreEqual(4, 3.1415.Ceiling(-1, ArgumentValidation.Lenient));
			Assert.AreEqual(4, 3.1415.Ceiling(0, ArgumentValidation.Lenient));
			Assert.AreEqual(3.15, 3.1415.Ceiling(2, ArgumentValidation.Lenient));
			Assert.AreEqual(3.1415, 3.1415.Ceiling(17, ArgumentValidation.Lenient));

			Assert.AreEqual(3, 3.0.Ceiling(0));
			Assert.AreEqual(4, 4.0.Ceiling(0));

			Assert.AreEqual(-4, -3.1415.Ceiling(0));
			Assert.AreEqual(-3.15, -3.1415.Ceiling(2));
			Assert.AreEqual(-3, -3.0.Ceiling(0));
			Assert.AreEqual(-4, -4.0.Ceiling(0));
		}

		[Test]
		public static void Floor() {
			Assert.Throws<ArgumentOutOfRangeException>(() => 3.1415.Floor(-1, ArgumentValidation.Strict));
			Assert.AreEqual(3, 3.1415.Floor(0, ArgumentValidation.Strict));
			Assert.AreEqual(3.14, 3.1415.Floor(2, ArgumentValidation.Strict));
			Assert.Throws<ArgumentOutOfRangeException>(() => 3.1415.Floor(17, ArgumentValidation.Strict));

			Assert.AreEqual(3, 3.1415.Floor(-1, ArgumentValidation.Lenient));
			Assert.AreEqual(3, 3.1415.Floor(0, ArgumentValidation.Lenient));
			Assert.AreEqual(3.14, 3.1415.Floor(2, ArgumentValidation.Lenient));
			Assert.AreEqual(3.1415, 3.1415.Floor(17, ArgumentValidation.Lenient));

			Assert.AreEqual(3, 3.0.Floor(0));
			Assert.AreEqual(4, 4.0.Floor(0));

			Assert.AreEqual(-3, -3.1415.Floor(0));
			Assert.AreEqual(-3.14, -3.1415.Floor(2));
			Assert.AreEqual(-3, -3.0.Floor(0));
			Assert.AreEqual(-4, -4.0.Floor(0));
		}

		[Test]
		public static void Round() {
			Assert.Throws<ArgumentOutOfRangeException>(() => 3.1415.Round(-1, ArgumentValidation.Strict));
			Assert.AreEqual(3, 3.1415.Round(0, ArgumentValidation.Strict));
			Assert.AreEqual(3.14, 3.1415.Round(2, ArgumentValidation.Strict));
			Assert.Throws<ArgumentOutOfRangeException>(() => 3.1415.Round(17, ArgumentValidation.Strict));

			Assert.AreEqual(3, 3.1415.Round(-1, ArgumentValidation.Lenient));
			Assert.AreEqual(3, 3.1415.Round(0, ArgumentValidation.Lenient));
			Assert.AreEqual(3.14, 3.1415.Round(2, ArgumentValidation.Lenient));
			Assert.AreEqual(3.1415, 3.1415.Round(17, ArgumentValidation.Lenient));

			Assert.AreEqual(1, 1.2.Round(0));
			Assert.AreEqual(2, 1.5.Round(0));
			Assert.AreEqual(2, 1.7.Round(0));
			Assert.AreEqual(2, 2.2.Round(0));
			Assert.AreEqual(3, 2.5.Round(0));
			Assert.AreEqual(3, 2.7.Round(0));
			Assert.AreEqual(3, 3.0.Round(0));
			Assert.AreEqual(4, 4.0.Round(0));

			Assert.AreEqual(-3, -3.1415.Round(0));
			Assert.AreEqual(-3.14, -3.1415.Round(2));
			Assert.AreEqual(-1, -1.2.Round(0));
			Assert.AreEqual(-2, -1.5.Round(0));
			Assert.AreEqual(-2, -1.7.Round(0));
			Assert.AreEqual(-2, -2.2.Round(0));
			Assert.AreEqual(-3, -2.5.Round(0));
			Assert.AreEqual(-3, -2.7.Round(0));
			Assert.AreEqual(-3, -3.0.Round(0));
			Assert.AreEqual(-4, -4.0.Round(0));
		}

		[Test]
		public static void Lerp() {
			Assert.AreEqual(100.0, 10.0.Lerp(0, 100, 0, 1000));
			Assert.AreEqual(110.0, 10.0.Lerp(0, 100, 100, 200));
		}
	}
}
