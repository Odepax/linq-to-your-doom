using System;
using System.Text;

namespace LinqToYourDoom;

/// <summary> Konflikt! </summary>
public sealed class AssignConflictException : Exception {
	/// <summary>
	/// Represents the path to the property that triggered the conflict,
	/// e.g. <c>"Users[42].Name"</c>.
	/// </summary>
	///
	/// <remarks>
	/// <see cref="Path"/> is provided for <b>debug purposes</b>.
	/// Breaking changes here may not be reflected in the library's semantic versioning!
	/// </remarks>
	public string Path => PathBuilder.ToString();

	readonly StringBuilder PathBuilder;

	/// <inheritdoc cref="AssignConflictException"/>
	public AssignConflictException() => PathBuilder = new();

	/// <inheritdoc cref="AssignConflictException"/>
	///
	/// <param name="path"> A base value for <see cref="Path"/>. </param>
	public AssignConflictException(string? path) => PathBuilder = new(path);

	/// <summary>
	/// Prepends a property name to the <see cref="Path"/>.
	/// </summary>
	public void PrependProperty(string? propertyName) {
		if (PathBuilder.Length != 0 && PathBuilder[0] != '[')
			PathBuilder.Insert(0, '.');

		PathBuilder.Insert(0, propertyName);
	}

	/// <summary>
	/// Prepends a key and a property name to the <see cref="Path"/>.
	/// </summary>
	public void PrependPropertyAndIndexer(string? propertyName, string? indexName) {
		if (PathBuilder.Length != 0 && PathBuilder[0] != '[')
			PathBuilder.Insert(0, '.');

		// Insertions are in reverse order, i.e. the last insert will be the new start of the string!
		PathBuilder.Insert(0, ']');
		PathBuilder.Insert(0, indexName);
		PathBuilder.Insert(0, '[');
		PathBuilder.Insert(0, propertyName);
	}
}
