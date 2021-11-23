using System.Diagnostics.CodeAnalysis;

namespace LinqToYourDoom;

/// <summary>
/// Encapsulates a method that takes three input parameters,
/// and tries to output a value to its <paramref name="out"/> output parameter,
/// returning <see langword="true"/> if it succeeds,
/// or <see langword="false"/> if <paramref name="out"/> was not set.
/// </summary>
public delegate bool TryFunc<TResult>([MaybeNullWhen(false)] out TResult @out);
