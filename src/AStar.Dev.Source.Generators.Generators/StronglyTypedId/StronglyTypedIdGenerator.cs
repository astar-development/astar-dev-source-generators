using Microsoft.CodeAnalysis;

namespace AStar.Dev.Source.Generators.Generators.StronglyTypedId;

[Generator(LanguageNames.CSharp)]
public sealed class StronglyTypedIdGenerator : IIncrementalGenerator
{
    internal const string AttributeFullName =
        "AStar.Dev.Source.Generators.Abstractions.StronglyTypedIdAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // TODO: implement pipeline
        //
        // Unlike the other two generators this one emits one source file
        // *per annotated type* rather than one aggregated file, so we do NOT
        // call .Collect().  Instead:
        //
        // 1. ForAttributeWithMetadataName(AttributeFullName, ...)
        // 2. Select → StronglyTypedIdModel
        // 3. RegisterSourceOutput(model => Emitter.Emit(ctx, model))
        //    — one AddSource call per model
    }
}
