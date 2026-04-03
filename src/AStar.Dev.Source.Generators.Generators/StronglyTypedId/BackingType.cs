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
