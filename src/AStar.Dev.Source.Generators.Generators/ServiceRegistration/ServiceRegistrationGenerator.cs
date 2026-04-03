using Microsoft.CodeAnalysis;

namespace AStar.Dev.Source.Generators.Generators.ServiceRegistration;

/// <summary>
/// Incremental source generator that discovers all types annotated with
/// <c>[RegisterService]</c> and emits a
/// <c>ServiceCollectionExtensions.AddGeneratedServices()</c> extension method.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class ServiceRegistrationGenerator : IIncrementalGenerator
{
    internal const string AttributeFullName =
        "AStar.Dev.Source.Generators.Abstractions.RegisterServiceAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // TODO: implement pipeline
        //
        // 1. context.SyntaxProvider.ForAttributeWithMetadataName(AttributeFullName, ...)
        // 2. .Where(static m => m is not null)
        // 3. .Select(static (ctx, _) => ServiceRegistrationModel.From(ctx))
        // 4. .Collect()
        // 5. context.RegisterSourceOutput(models, Emitter.Emit)
    }
}
