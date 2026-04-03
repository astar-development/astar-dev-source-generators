using AStar.Dev.Source.Generators.Generators.OptionsConfiguration;
using AStar.Dev.Source.Generators.Generators.Tests.Infrastructure;
using Microsoft.CodeAnalysis;
using VerifyXunit;

namespace AStar.Dev.Source.Generators.Generators.Tests.OptionsConfiguration;

public sealed class OptionsConfigurationGeneratorTests
{
    // ── Happy path ───────────────────────────────────────────────────────────

    [Fact]
    public Task SingleOptionsClassGeneratesConfigureCall()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Configuration;

            [OptionsConfiguration("PaymentGateway")]
            public sealed class PaymentGatewayOptions
            {
                public string ApiKey { get; set; } = string.Empty;
                public string BaseUrl { get; set; } = string.Empty;
            }
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<OptionsConfigurationGenerator>(source);
        return Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task MultipleOptionsClassesGeneratesSingleExtensionMethod()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Configuration;

            [OptionsConfiguration("Database")]
            public sealed class DatabaseOptions
            {
                public string ConnectionString { get; set; } = string.Empty;
            }

            [OptionsConfiguration("Cache")]
            public sealed class CacheOptions
            {
                public int ExpirySeconds { get; set; } = 300;
            }

            [OptionsConfiguration("Email")]
            public sealed class EmailOptions
            {
                public string SmtpHost { get; set; } = string.Empty;
            }
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<OptionsConfigurationGenerator>(source);
        return Verify(result).UseDirectory("Snapshots");
    }

    // ── Diagnostic cases ─────────────────────────────────────────────────────

    [Fact]
    public async Task EmptySectionNameEmitsDiagnosticASG0010()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Configuration;

            [OptionsConfiguration("")]
            public sealed class EmptyOptions { }
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<OptionsConfigurationGenerator>(source);
        Assert.Empty(result.GeneratedTrees);
        await Task.CompletedTask;
    }
}
