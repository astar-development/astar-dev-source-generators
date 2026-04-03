using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace AStar.Dev.Source.Generators.Generators.OptionsConfiguration;

internal static class OptionsConfigurationEmitter
{
    internal static void Emit(
        SourceProductionContext context,
        ImmutableArray<OptionsConfigurationModel> models)
    {
        if (models.IsDefaultOrEmpty)
            return;

        var source = "null";
        context.AddSource("OptionsConfigurationExtensions.g.cs", source);
    }
}
