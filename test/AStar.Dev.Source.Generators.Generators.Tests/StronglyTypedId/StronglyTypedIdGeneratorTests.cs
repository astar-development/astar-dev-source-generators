using AStar.Dev.Source.Generators.Generators.StronglyTypedId;
using AStar.Dev.Source.Generators.Generators.Tests.Infrastructure;
using Microsoft.CodeAnalysis;

namespace AStar.Dev.Source.Generators.Generators.Tests.StronglyTypedId;

public sealed class StronglyTypedIdGeneratorTests
{
    // ── Happy path — backing type variants ───────────────────────────────────

    [Fact]
    public Task DefaultGuidBackingGeneratesFullImplementation()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Domain;

            [StronglyTypedId]
            public readonly partial record struct OrderId;
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<StronglyTypedIdGenerator>(source);
        return Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task ExplicitGuidBackingProducesIdenticalOutput()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;
            using System;

            namespace MyApp.Domain;

            [StronglyTypedId(typeof(Guid))]
            public readonly partial record struct CustomerId;
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<StronglyTypedIdGenerator>(source);
        return Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task IntBackingGeneratesIntImplementation()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Domain;

            [StronglyTypedId(typeof(int))]
            public readonly partial record struct CategoryId;
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<StronglyTypedIdGenerator>(source);
        return Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task LongBackingGeneratesLongImplementation()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Domain;

            [StronglyTypedId(typeof(long))]
            public readonly partial record struct SequenceId;
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<StronglyTypedIdGenerator>(source);
        return Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task StringBackingGeneratesStringImplementation()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Domain;

            [StronglyTypedId(typeof(string))]
            public readonly partial record struct SlugId;
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<StronglyTypedIdGenerator>(source);
        return Verify(result).UseDirectory("Snapshots");
    }

    [Fact]
    public Task MultipleIdsInSameNamespaceEachGetsOwnFile()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;
            using System;

            namespace MyApp.Domain;

            [StronglyTypedId]
            public readonly partial record struct OrderId;

            [StronglyTypedId(typeof(int))]
            public readonly partial record struct LineItemId;

            [StronglyTypedId(typeof(string))]
            public readonly partial record struct Sku;
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<StronglyTypedIdGenerator>(source);

        // Expect exactly 3 generated files — one per struct.
        Assert.Equal(3, result.GeneratedTrees.Length);
        return Verify(result).UseDirectory("Snapshots");
    }

    // ── Diagnostic cases ─────────────────────────────────────────────────────

    [Fact]
    public async Task NotARecordStructEmitsDiagnosticASG0020()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Domain;

            [StronglyTypedId]
            public partial class OrderId { }
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<StronglyTypedIdGenerator>(source);
        Assert.Empty(result.GeneratedTrees);
        await Task.CompletedTask;
    }

    [Fact]
    public async Task NotPartialEmitsDiagnosticASG0020()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Domain;

            [StronglyTypedId]
            public readonly record struct OrderId;
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<StronglyTypedIdGenerator>(source);
        Assert.Empty(result.GeneratedTrees);
        await Task.CompletedTask;
    }

    [Fact]
    public async Task UnsupportedBackingTypeEmitsDiagnosticASG0021()
    {
        const string source = """
            using AStar.Dev.Source.Generators.Abstractions;

            namespace MyApp.Domain;

            [StronglyTypedId(typeof(double))]
            public readonly partial record struct BadId;
            """;

        GeneratorDriverRunResult result = GeneratorTestBase.RunGenerator<StronglyTypedIdGenerator>(source);
        Assert.Empty(result.GeneratedTrees);
        await Task.CompletedTask;
    }
}
