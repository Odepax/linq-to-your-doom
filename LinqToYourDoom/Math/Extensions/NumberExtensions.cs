using System.Runtime.CompilerServices;

namespace LinqToYourDoom;

public static partial class NumberExtensions {
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float CoerceToFloat(this double @this) => @this < float.MinValue ? float.MinValue : float.MaxValue < @this ? float.MaxValue : (float) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static decimal CoerceToDecimal(this double @this) => @this < (double) decimal.MinValue ? decimal.MinValue : (double) decimal.MaxValue < @this ? decimal.MaxValue : (decimal) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong CoerceToULong(this double @this) => @this < ulong.MinValue ? ulong.MinValue : ulong.MaxValue < @this ? ulong.MaxValue : (ulong) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long CoerceToLong(this double @this) => @this < long.MinValue ? long.MinValue : long.MaxValue < @this ? long.MaxValue : (long) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint CoerceToUInt(this double @this) => @this < uint.MinValue ? uint.MinValue : uint.MaxValue < @this ? uint.MaxValue : (uint) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CoerceToInt(this double @this) => @this < int.MinValue ? int.MinValue : int.MaxValue < @this ? int.MaxValue : (int) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort CoerceToUShort(this double @this) => @this < ushort.MinValue ? ushort.MinValue : ushort.MaxValue < @this ? ushort.MaxValue : (ushort) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short CoerceToShort(this double @this) => @this < short.MinValue ? short.MinValue : short.MaxValue < @this ? short.MaxValue : (short) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte CoerceToByte(this double @this) => @this < byte.MinValue ? byte.MinValue : byte.MaxValue < @this ? byte.MaxValue : (byte) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte CoerceToSByte(this double @this) => @this < sbyte.MinValue ? sbyte.MinValue : sbyte.MaxValue < @this ? sbyte.MaxValue : (sbyte) @this;

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double ToDouble(this float @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static decimal CoerceToDecimal(this float @this) => @this < (float) decimal.MinValue ? decimal.MinValue : (float) decimal.MaxValue < @this ? decimal.MaxValue : (decimal) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong CoerceToULong(this float @this) => @this < ulong.MinValue ? ulong.MinValue : ulong.MaxValue < @this ? ulong.MaxValue : (ulong) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long CoerceToLong(this float @this) => @this < long.MinValue ? long.MinValue : long.MaxValue < @this ? long.MaxValue : (long) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint CoerceToUInt(this float @this) => @this < uint.MinValue ? uint.MinValue : uint.MaxValue < @this ? uint.MaxValue : (uint) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CoerceToInt(this float @this) => @this < int.MinValue ? int.MinValue : int.MaxValue < @this ? int.MaxValue : (int) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort CoerceToUShort(this float @this) => @this < ushort.MinValue ? ushort.MinValue : ushort.MaxValue < @this ? ushort.MaxValue : (ushort) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short CoerceToShort(this float @this) => @this < short.MinValue ? short.MinValue : short.MaxValue < @this ? short.MaxValue : (short) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte CoerceToByte(this float @this) => @this < byte.MinValue ? byte.MinValue : byte.MaxValue < @this ? byte.MaxValue : (byte) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte CoerceToSByte(this float @this) => @this < sbyte.MinValue ? sbyte.MinValue : sbyte.MaxValue < @this ? sbyte.MaxValue : (sbyte) @this;

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double ToDouble(this decimal @this) => (double) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float ToFloat(this decimal @this) => (float) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong CoerceToULong(this decimal @this) => @this < ulong.MinValue ? ulong.MinValue : ulong.MaxValue < @this ? ulong.MaxValue : (ulong) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long CoerceToLong(this decimal @this) => @this < long.MinValue ? long.MinValue : long.MaxValue < @this ? long.MaxValue : (long) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint CoerceToUInt(this decimal @this) => @this < uint.MinValue ? uint.MinValue : uint.MaxValue < @this ? uint.MaxValue : (uint) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CoerceToInt(this decimal @this) => @this < int.MinValue ? int.MinValue : int.MaxValue < @this ? int.MaxValue : (int) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort CoerceToUShort(this decimal @this) => @this < ushort.MinValue ? ushort.MinValue : ushort.MaxValue < @this ? ushort.MaxValue : (ushort) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short CoerceToShort(this decimal @this) => @this < short.MinValue ? short.MinValue : short.MaxValue < @this ? short.MaxValue : (short) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte CoerceToByte(this decimal @this) => @this < byte.MinValue ? byte.MinValue : byte.MaxValue < @this ? byte.MaxValue : (byte) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte CoerceToSByte(this decimal @this) => @this < sbyte.MinValue ? sbyte.MinValue : sbyte.MaxValue < @this ? sbyte.MaxValue : (sbyte) @this;

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double ToDouble(this ulong @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float ToFloat(this ulong @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static decimal ToDecimal (this ulong @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long CoerceToLong(this ulong @this) => long.MaxValue < @this ? long.MaxValue : (long) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint CoerceToUInt(this ulong @this) => uint.MaxValue < @this ? uint.MaxValue : (uint) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CoerceToInt(this ulong @this) => int.MaxValue < @this ? int.MaxValue : (int) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort CoerceToUShort(this ulong @this) => ushort.MaxValue < @this ? ushort.MaxValue : (ushort) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short CoerceToShort(this ulong @this) => (ulong) short.MaxValue < @this ? short.MaxValue : (short) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte CoerceToByte(this ulong @this) => byte.MaxValue < @this ? byte.MaxValue : (byte) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte CoerceToSByte(this ulong @this) => (ulong) sbyte.MaxValue < @this ? sbyte.MaxValue : (sbyte) @this;

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double ToDouble(this long @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float ToFloat(this long @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static decimal ToDecimal(this long @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong CoerceToULong(this long @this) => @this < (long) ulong.MinValue ? ulong.MinValue : (ulong) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint CoerceToUInt(this long @this) => @this < uint.MinValue ? uint.MinValue : uint.MaxValue < @this ? uint.MaxValue : (uint) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CoerceToInt(this long @this) => @this < int.MinValue ? int.MinValue : int.MaxValue < @this ? int.MaxValue : (int) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort CoerceToUShort(this long @this) => @this < ushort.MinValue ? ushort.MinValue : ushort.MaxValue < @this ? ushort.MaxValue : (ushort) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short CoerceToShort(this long @this) => @this < short.MinValue ? short.MinValue : short.MaxValue < @this ? short.MaxValue : (short) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte CoerceToByte(this long @this) => @this < byte.MinValue ? byte.MinValue : byte.MaxValue < @this ? byte.MaxValue : (byte) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte CoerceToSByte(this long @this) => @this < sbyte.MinValue ? sbyte.MinValue : sbyte.MaxValue < @this ? sbyte.MaxValue : (sbyte) @this;

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double ToDouble(this uint @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float ToFloat(this uint @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static decimal ToDecimal(this uint @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong ToULong(this uint @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long ToLong(this uint @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CoerceToInt(this uint @this) => int.MaxValue < @this ? int.MaxValue : (int) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort CoerceToUShort(this uint @this) => ushort.MaxValue < @this ? ushort.MaxValue : (ushort) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short CoerceToShort(this uint @this) => short.MaxValue < @this ? short.MaxValue : (short) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte CoerceToByte(this uint @this) => byte.MaxValue < @this ? byte.MaxValue : (byte) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte CoerceToSByte(this uint @this) => sbyte.MaxValue < @this ? sbyte.MaxValue : (sbyte) @this;

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double ToDouble(this int @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float ToFloat(this int @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static decimal ToDecimal(this int @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong CoerceToULong(this int @this) => @this < 0 ? ulong.MinValue : (ulong) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long ToLong(this int @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint CoerceToUInt(this int @this) => @this < uint.MinValue ? uint.MinValue : (uint) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort CoerceToUShort(this int @this) => @this < ushort.MinValue ? ushort.MinValue : ushort.MaxValue < @this ? ushort.MaxValue : (ushort) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short CoerceToShort(this int @this) => @this < short.MinValue ? short.MinValue : short.MaxValue < @this ? short.MaxValue : (short) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte CoerceToByte(this int @this) => @this < byte.MinValue ? byte.MinValue : byte.MaxValue < @this ? byte.MaxValue : (byte) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte CoerceToSByte(this int @this) => @this < sbyte.MinValue ? sbyte.MinValue : sbyte.MaxValue < @this ? sbyte.MaxValue : (sbyte) @this;

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double ToDouble(this ushort @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float ToFloat(this ushort @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static decimal ToDecimal(this ushort @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong ToULong(this ushort @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long ToLong(this ushort @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint ToUInt(this ushort @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int ToInt(this ushort @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short CoerceToShort(this ushort @this) => short.MaxValue < @this ? short.MaxValue : (short) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte CoerceToByte(this ushort @this) => byte.MaxValue < @this ? byte.MaxValue : (byte) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte CoerceToSByte(this ushort @this) => sbyte.MaxValue < @this ? sbyte.MaxValue : (sbyte) @this;

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double ToDouble(this short @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float ToFloat(this short @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static decimal ToDecimal(this short @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong CoerceToULong(this short @this) => @this < 0 ? ulong.MinValue : (ulong) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long ToLong(this short @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint CoerceToUInt(this short @this) => @this < 0 ? uint.MinValue : (uint) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int ToInt(this short @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort CoerceToUShort(this short @this) => @this < ushort.MinValue ? ushort.MinValue : (ushort) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short ToShort(this short @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte CoerceToByte(this short @this) => @this < byte.MinValue ? byte.MinValue : byte.MaxValue < @this ? byte.MaxValue : (byte) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte CoerceToSByte(this short @this) => @this < sbyte.MinValue ? sbyte.MinValue : sbyte.MaxValue < @this ? sbyte.MaxValue : (sbyte) @this;

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double ToDouble(this byte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float ToFloat(this byte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static decimal ToDecimal(this byte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong ToULong(this byte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long ToLong(this byte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint ToUInt(this byte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int ToInt(this byte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort ToUShort(this byte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short ToShort(this byte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static sbyte CoerceToSByte(this byte @this) => sbyte.MaxValue < @this ? sbyte.MaxValue : (sbyte) @this;

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static double ToDouble(this sbyte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static float ToFloat(this sbyte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static decimal ToDecimal(this sbyte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ulong CoerceToULong(this sbyte @this) => @this < 0 ? ulong.MinValue : (ulong) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static long ToLong(this sbyte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint CoerceToUInt(this sbyte @this) => @this < 0 ? uint.MinValue : (uint) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static int ToInt(this sbyte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static ushort CoerceToUShort(this sbyte @this) => @this < 0 ? ushort.MinValue : (ushort) @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static short ToShort(this sbyte @this) => @this;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static byte CoerceToByte(this sbyte @this) => @this < byte.MinValue ? byte.MinValue : (byte) @this;
}
