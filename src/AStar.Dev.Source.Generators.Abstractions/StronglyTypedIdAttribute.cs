using System;

namespace AStar.Dev.Source.Generators.Abstractions;

/// <summary>
/// Generates a strongly-typed ID wrapper around a primitive backing type.
/// Apply to a <c>readonly partial record struct</c>.
/// </summary>
/// <remarks>
/// The generator emits:
/// <list type="bullet">
///   <item>A constructor accepting the backing value.</item>
///   <item>Implicit/explicit conversions to and from the primitive.</item>
///   <item><c>IComparable&lt;T&gt;</c> and <c>IParsable&lt;T&gt;</c> implementations.</item>
///   <item>A <c>NewId()</c> factory method (Guid types use <c>Guid.CreateVersion7()</c>).</item>
///   <item>A nested <c>System.Text.Json</c> <c>JsonConverter&lt;T&gt;</c>.</item>
///   <item>An EF Core <c>ValueConverter&lt;T, TPrimitive&gt;</c> (when EF Core is referenced).</item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// [StronglyTypedId]                    // defaults to Guid
/// public readonly partial record struct OrderId;
///
/// [StronglyTypedId(typeof(long))]      // long-backed
/// public readonly partial record struct ProductId;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class StronglyTypedIdAttribute : Attribute
{
    /// <summary>
    /// The primitive backing type.  Supported: <c>Guid</c>, <c>int</c>, <c>long</c>, <c>string</c>.
    /// Defaults to <c>Guid</c>.
    /// </summary>
    public Type BackingType { get; }

    public StronglyTypedIdAttribute(Type? backingType = null)
        => BackingType = backingType ?? typeof(Guid);
}
