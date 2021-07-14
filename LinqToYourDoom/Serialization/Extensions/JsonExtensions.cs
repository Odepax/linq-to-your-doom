using System.Runtime.CompilerServices;
using System.Text.Json;

namespace LinqToYourDoom.Serialization.Extensions {
	public static class JsonExtensions {
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string ToJsonString<T>(this T @this, bool writeIndented = true) => JsonSerializer.Serialize(@this, writeIndented ? JsonOptions.Indented: JsonOptions.Unindented);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static string ToJsonString<T>(this T @this, JsonSerializerOptions options) => JsonSerializer.Serialize(@this, options);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static T? ToJsonObject<T>(this string @this, JsonSerializerOptions? options = default) => JsonSerializer.Deserialize<T>(@this, options ?? JsonOptions.Unindented);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static T ToJsonObjectOrDefault<T>(this string @this, JsonSerializerOptions? options = default) where T : new() => JsonSerializer.Deserialize<T>(@this, options ?? JsonOptions.Unindented) ?? new();

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static JsonDocument ToJsonDocument(this string @this) => JsonDocument.Parse(@this, JsonOptions.DefaultDocument);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static JsonDocument ToJsonDocument(this string @this, JsonDocumentOptions options) => JsonDocument.Parse(@this, options);
	}
}
