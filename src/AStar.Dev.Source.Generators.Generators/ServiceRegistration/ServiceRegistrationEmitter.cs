using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace AStar.Dev.Source.Generators.Generators.ServiceRegistration;

internal static class ServiceRegistrationEmitter
{
    internal static void Emit(
        SourceProductionContext context,
        ImmutableArray<ServiceRegistrationModel> models)
    {
        if (models.IsDefaultOrEmpty)
            return;

        // TODO: build source via StringBuilder / IndentedTextWriter
        // and call context.AddSource("ServiceCollectionExtensions.g.cs", source)
        context.AddSource(
            hintName: $"ServiceCollectionExtensions.g.cs",
            sourceText: SourceText.From(BuildSource(models), Encoding.UTF8));
    }

    private static string BuildSource(ImmutableArray<ServiceRegistrationModel> models) => throw new NotImplementedException();
}
