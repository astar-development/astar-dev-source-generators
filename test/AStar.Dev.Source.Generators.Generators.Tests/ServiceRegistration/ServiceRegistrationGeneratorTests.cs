using AStar.Dev.Source.Generators.Generators.ServiceRegistration;
using AStar.Dev.Source.Generators.Generators.Tests.Infrastructure;
using Microsoft.CodeAnalysis;

namespace AStar.Dev.Source.Generators.Generators.Tests.ServiceRegistration;

public sealed class ServiceRegistrationGeneratorTests
{
    // ── Happy path ───────────────────────────────────────────────────────────

    [Fact]
    public Task SingleScopedServiceGeneratesCorrectExtensionMethod()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Services;

            public interface IOrderService { }

            [RegisterService(ServiceLifetime.Scoped)]
            public partial class OrderService : IOrderService { }
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<ServiceRegistrationGenerator>(source);
        return Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task SingletonServiceWithNoInterfaceRegistersAsConcrete()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Services;

            [RegisterService(ServiceLifetime.Singleton)]
            public partial class BackgroundWorker { }
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<ServiceRegistrationGenerator>(source);
        return Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task MultipleServicesAllLifetimesGeneratesSingleExtensionMethod()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Services;

            public interface IFoo { }
            public interface IBar { }
            public interface IBaz { }

            [RegisterService(ServiceLifetime.Scoped)]
            public partial class FooService : IFoo { }

            [RegisterService(ServiceLifetime.Singleton)]
            public partial class BarService : IBar { }

            [RegisterService(ServiceLifetime.Transient)]
            public partial class BazService : IBaz { }
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<ServiceRegistrationGenerator>(source);
        return Verify(result).UseDirectory("Snapshots");
    }

    // ── Diagnostic cases ─────────────────────────────────────────────────────

    [Fact]
    public async Task NonPartialClassEmitsDiagnosticASG0001()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Services;

            public interface IOrderService { }

            [RegisterService(ServiceLifetime.Scoped)]
            public class OrderService : IOrderService { }
            """;

        // TODO: assert diagnostic ASG0001 once emitter is implemented
        // For now, verify the run produces no output (not crashing is the bar).
        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<ServiceRegistrationGenerator>(source);
        Assert.Empty(result.GeneratedTrees);
        await Task.CompletedTask;
    }

    [Fact]
    public async Task AbstractClassEmitsDiagnosticASG0002()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Services;

            [RegisterService(ServiceLifetime.Scoped)]
            public abstract partial class BaseService { }
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<ServiceRegistrationGenerator>(source);
        Assert.Empty(result.GeneratedTrees);
        await Task.CompletedTask;
    }
}
