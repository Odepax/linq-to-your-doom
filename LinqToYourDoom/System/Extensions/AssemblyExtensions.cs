using System.IO;
using System.Reflection;
using System.Text;

namespace LinqToYourDoom;

public static class AssemblyExtensions {
	/// <summary>
	/// Reads an embedded (i.e. manifest) resource from <paramref name="this"/> assembly.
	/// </summary>
	///
	/// <param name="path">
	/// The resource <paramref name="path"/> will be prepended with <paramref name="this"/> assembly name.
	/// The <paramref name="path"/> <b>must</b> use <c>'/'</c> as the directory separator.
	/// </param>
	///
	/// <exception cref="FileNotFoundException">
	/// When no resource is found at the specified <paramref name="path"/>.
	/// </exception>
	public static Stream GetEmbeddedResourceStream(this Assembly @this, string path) => // TODO: use LTYD.FileSystem.FilePath instead of string here
			@this.GetManifestResourceStream(ResourcePath(@this, path)) ?? throw new FileNotFoundException("No assembly resource exists for the specified path.", path);

	/// <inheritdoc cref="GetEmbeddedResourceStream(Assembly, string)"/>
	public static byte[] GetEmbeddedResourceBytes(this Assembly @this, string path) { // TODO: use LTYD.FileSystem.FilePath instead of string here
		using var stream = GetEmbeddedResourceStream(@this, path);
		using var reader = new BinaryReader(stream);

		return reader.ReadBytes(stream.Length.CoerceToInt());
	}

	/// <inheritdoc cref="GetEmbeddedResourceStream(Assembly, string)"/> // TODO: use LTYD.FileSystem.FilePath instead of string here
	public static string GetEmbeddedResourceString(this Assembly @this, string path, Encoding encoding) {
		using var stream = GetEmbeddedResourceStream(@this, path);
		using var reader = new StreamReader(stream, encoding);

		return reader.ReadToEnd();
	}

	static string ResourcePath(Assembly assembly, string path) =>
		assembly.GetName().Name + '.' + path.Replace('/', '.');
}
