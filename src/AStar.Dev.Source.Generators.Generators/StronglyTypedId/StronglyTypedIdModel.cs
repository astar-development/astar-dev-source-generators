namespace AStar.Dev.Source.Generators.Generators.StronglyTypedId;

/// <summary>
/// Immutable data extracted from a single <c>[StronglyTypedId]</c> annotation.
/// One instance → one generated source file.
/// </summary>
internal sealed class StronglyTypedIdModel
{
    public StronglyTypedIdModel(string @namespace, string structName, BackingType backingType, bool emitEfCoreConverter)
    {
        Namespace = @namespace;
        StructName = structName;
        BackingType = backingType;
        EmitEfCoreConverter = emitEfCoreConverter;

    }

    public string Namespace { get; set; }
    public string StructName { get;  set; }
    public BackingType BackingType { get;  set; }
    public bool EmitEfCoreConverter { get;  set; }

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
