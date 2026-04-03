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

        // TODO: build source and call:
        // context.AddSource("OptionsConfigurationExtensions.g.cs", source)
        //
        // Emitted shape:
        //
        // public static class OptionsConfigurationExtensions
        // {
        //     public static IServiceCollection AddGeneratedOptions(
        //         this IServiceCollection services,
        //         IConfiguration configuration)
        //     {
        //         services.Configure<FooOptions>(configuration.GetSection("Foo"));
        //         ...
        //         return services;
        //     }
        // }
    }
}
