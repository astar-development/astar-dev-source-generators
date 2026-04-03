using System;

namespace AStar.Dev.Source.Generators.Abstractions;

/// <summary>
/// Marks a class for automatic DI registration.
/// The source generator will emit a <c>ServiceCollectionExtensions.AddGeneratedServices()</c>
/// extension method that registers every annotated type in the assembly.
/// </summary>
/// <example>
/// <code>
/// [RegisterService(ServiceLifetime.Scoped)]
/// public class OrderService : IOrderService { }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class RegisterServiceAttribute : Attribute
{
    /// <summary>The DI lifetime to use for this registration.</summary>
    public ServiceLifetime Lifetime { get; }

    /// <summary>
    /// When <c>true</c>, the service is registered against each of its implemented
    /// interfaces rather than its concrete type.  Defaults to <c>true</c>.
    /// </summary>
    public bool RegisterAsInterfaces { get; set; } = true;

    public RegisterServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        => Lifetime = lifetime;
}
