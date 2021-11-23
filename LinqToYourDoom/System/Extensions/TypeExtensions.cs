using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LinqToYourDoom;

public static class TypeExtensions {
	/// <summary> Alias of <see cref="Type.IsAssignableTo(Type?)"/>. </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Inherits(this Type @this, Type other) => @this.IsAssignableTo(other);

	/// <inheritdoc cref="Inherits(Type, Type)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Implements(this Type @this, Type other) => @this.Inherits(other);

	/// <summary>
	/// For example: <c><see langword="typeof"/>(List&lt;A&gt;).Implements(<see langword="typeof"/>(IEnumerable&lt;&gt;)</c> returns <see langword="false"/>,
	/// while <c><see langword="typeof"/>(List&lt;A&gt;).ImplementsGeneric(<see langword="typeof"/>(IEnumerable&lt;&gt;)</c> returns <see langword="true"/>.
	/// </summary>
	///
	/// <param name="other">
	/// <b>Must</b> be the type of a generic interface.
	/// </param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ImplementsGeneric(this Type @this, Type other, [NotNullWhen(true)] out Type? @interface) =>
		@this
			.GetInterfaces()
			.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == other)
			.Tee(out @interface)
			is not null;

	/// <summary>
	/// Example:
	/// <c>typeof(Dictionary&lt;System.DateTime[], IReadOnlyList&lt;float&gt;&gt;).PrettyName();</c>
	/// returns <c>"Dictionary&lt;DateTime[], IReadOnlyList&lt;Single&gt;&gt;"</c>.
	/// </summary>
	public static string PrettyName(this Type @this) {
		// Stolen and adapted from https://stackoverflow.com/a/6402954
		if (@this.IsGenericType) {
			var generics = @this.GetGenericArguments();

			return new StringBuilder(@this.Name, 0, @this.Name.IndexOf('`'), @this.Name.Length + generics.Length * 12)
				.Append('<')
				.AppendJoin(", ", generics.Select(PrettyName))
				.Append('>')
				.ToString();
		}

		else return @this.Name;
	}
}
