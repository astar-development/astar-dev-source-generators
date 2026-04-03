namespace AStar.Dev.Source.Generators.Abstractions;

/// <summary>
/// The DI lifetime to use when registering a service via <see cref="RegisterServiceAttribute"/>.
/// Mirrors <c>Microsoft.Extensions.DependencyInjection.ServiceLifetime</c> so consumers
/// don't need to import MEDI just to annotate a class.
/// </summary>
public enum ServiceLifetime
{
    Singleton,
    Scoped,
    Transient,
}
