using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AStar.Dev.Source.Generators.Generators.OptionsConfiguration;

public static class OptionsConfigurationExtensions
{
    public static IServiceCollection AddGeneratedOptions(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<FooOptions>(configuration.GetSection("Foo"));
        return services;
    }
}
