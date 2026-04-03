namespace AStar.Dev.Source.Generators.Generators.StronglyTypedId;

/// <summary>
/// The primitive type that backs the strongly-typed ID.
/// Resolved from the <c>typeof(...)</c> argument on <c>[StronglyTypedId]</c>.
/// </summary>
internal enum BackingType
{
    Guid,
    Int,
    Long,
    String,
}

/// <summary>
/// Immutable data extracted from a single <c>[StronglyTypedId]</c> annotation.
/// One instance → one generated source file.
/// </summary>
internal sealed class StronglyTypedIdModel(
    string Namespace,
    string StructName,
    BackingType BackingType,
    /// <summary>True when EF Core is detected in the compilation references.</summary>
    bool EmitEfCoreConverter
)
{
    /// <summary>The C# keyword / fully-qualified type name for the backing primitive.</summary>
    internal string BackingTypeName => BackingType switch
    {
        BackingType.Guid   => "global::System.Guid",
        BackingType.Int    => "int",
        BackingType.Long   => "long",
        BackingType.String => "string",
        _                  => throw new InvalidOperationException($"Unsupported backing type: {BackingType}"),
    };

    /// <summary>
    /// The factory expression used in <c>NewId()</c>.
    /// Guid uses <c>Guid.CreateVersion7()</c> (UUIDv7, time-ordered) available in .NET 9+.
    /// Numeric and string types have no auto-generate semantics — the method throws by default.
    /// </summary>
    internal string NewIdExpression => BackingType switch
    {
        BackingType.Guid   => "global::System.Guid.CreateVersion7()",
        BackingType.Int    => "throw new global::System.NotSupportedException(\"NewId() is not supported for int-backed IDs.\")",
        BackingType.Long   => "throw new global::System.NotSupportedException(\"NewId() is not supported for long-backed IDs.\")",
        BackingType.String => "throw new global::System.NotSupportedException(\"NewId() is not supported for string-backed IDs.\")",
        _                  => throw new InvalidOperationException($"Unsupported backing type: {BackingType}"),
    };
}
