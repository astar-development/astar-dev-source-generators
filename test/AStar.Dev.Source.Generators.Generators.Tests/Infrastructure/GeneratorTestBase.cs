using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace AStar.Dev.Source.Generators.Generators.Tests.Infrastructure;

/// <summary>
/// Provides convenience factory methods for setting up
/// <see cref="CSharpSourceGeneratorTest{TGenerator,TVerifier}"/> instances
/// with the solution's standard references pre-loaded.
/// </summary>
internal static class GeneratorTestBase
{
    /// <summary>
    /// The C# parse options that match the generator's target language version.
    /// </summary>
    internal static readonly CSharpParseOptions ParseOptions =
        new(LanguageVersion.Latest);

    /// <summary>
    /// Creates a fully-configured test instance for <typeparamref name="TGenerator"/>.
    /// </summary>
    internal static CSharpSourceGeneratorTest<TGenerator, DefaultVerifier> Create<TGenerator>(
        string testSource,
        params (Type generatorType, string hintName, string source)[] generatedSources)
        where TGenerator : IIncrementalGenerator, new()
    {
        var test = new CSharpSourceGeneratorTest<TGenerator, DefaultVerifier>
        {
            TestCode = testSource,
            TestState =
            {
                // Ensure our Abstractions attributes are resolvable inside the test compilation.
                AdditionalReferences =
                {
                    MetadataReference.CreateFromFile(
                        typeof(Abstractions.RegisterServiceAttribute).Assembly.Location),
                },
            },
        };

        foreach ((Type _, var hintName, var source) in generatedSources)
            test.TestState.GeneratedSources.Add((typeof(TGenerator), hintName, source));

        return test;
    }

    /// <summary>
    /// Runs the generator against <paramref name="source"/> and returns the
    /// full <see cref="GeneratorDriverRunResult"/> for snapshot / assertion use.
    /// </summary>
    internal static GeneratorDriverRunResult RunGenerator<TGenerator>(string source)
        where TGenerator : IIncrementalGenerator, new()
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source, ParseOptions);

        var compilation = CSharpCompilation.Create(
            assemblyName: "TestAssembly",
            syntaxTrees: [syntaxTree],
            references:
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(
                    typeof(Abstractions.RegisterServiceAttribute).Assembly.Location),
            ],
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var generator = new TGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver
            .Create(generator)
            .WithUpdatedParseOptions(ParseOptions)
            .RunGeneratorsAndUpdateCompilation(compilation, out _, out _);

        return driver.GetRunResult();
    }
}
