namespace AStar.Dev.Source.Generators.Generators.ServiceRegistration;

/// <summary>
/// Immutable data extracted from a single <c>[RegisterService]</c> annotation.
/// Kept pure (no Roslyn symbols) so it is cheap to cache in the incremental pipeline.
/// </summary>
internal sealed class ServiceRegistrationModel(
    string Namespace,
    string ClassName,
    string[] InterfaceNames,
    string Lifetime,   // "Singleton" | "Scoped" | "Transient"
    bool RegisterAsInterfaces
);
