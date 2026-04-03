using Microsoft.CodeAnalysis;

namespace AStar.Dev.Source.Generators.Generators.OptionsConfiguration;

[Generator(LanguageNames.CSharp)]
public sealed class OptionsConfigurationGenerator : IIncrementalGenerator
{
    internal const string AttributeFullName =
        "AStar.Dev.Source.Generators.Abstractions.OptionsConfigurationAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // TODO: implement pipeline
        //
        // 1. ForAttributeWithMetadataName(AttributeFullName, ...)
        // 2. Select → OptionsConfigurationModel
        // 3. Collect → Emitter.Emit
    }
}
