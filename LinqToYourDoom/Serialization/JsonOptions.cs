using System.Text.Json;
using System.Text.Json.Serialization;
namespace LinqToYourDoom.Serialization {
	public static class JsonOptions {
		public static readonly JsonSerializerOptions Indented = new() {
			ReadCommentHandling = JsonCommentHandling.Skip,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals,
			AllowTrailingCommas = true,
			ReferenceHandler = ReferenceHandler.Preserve,
			WriteIndented = true
		};

		public static readonly JsonSerializerOptions Unindented = new() {
			ReadCommentHandling = JsonCommentHandling.Skip,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals,
			AllowTrailingCommas = true,
			ReferenceHandler = ReferenceHandler.Preserve,
			WriteIndented = false
		};

		public static readonly JsonDocumentOptions DefaultDocument = new() {
			CommentHandling = JsonCommentHandling.Skip,
			AllowTrailingCommas = true,
			MaxDepth = int.MaxValue
		};
	}
}
