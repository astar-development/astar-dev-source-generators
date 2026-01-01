using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Shouldly;
using Xunit;

namespace AStar.Dev.Source.Generators.Tests;

public class StrongIdGeneratorTests
{
    private const string AttributeSource = @"using System;
namespace AStar.Dev.Source.Generators.Annotations {
    [AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class StrongIdAttribute : Attribute {
        public Type IdType { get; }
        public StrongIdAttribute(Type idType) => IdType = idType ?? throw new ArgumentNullException(nameof(idType));
    }
}";

    [Fact]
    public void GeneratesPartialStructWithIdProperty_ForValidReadonlyRecordStruct()
    {
        const string input = @"using AStar.Dev.Source.Generators.Annotations;
namespace TestNamespace
{
    [StrongId(typeof(int))]
    public readonly partial record struct MyId { }
}";

        var compilation = CSharpCompilation.Create("TestAssembly",
            new[]
            {
                CSharpSyntaxTree.ParseText(AttributeSource),
                CSharpSyntaxTree.ParseText(input)
            },
            new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location)
            },
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var generator = new StrongIdGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);
        GeneratorDriverRunResult result = driver.GetRunResult();
        var allGenerated = result.Results.SelectMany(r => r.GeneratedSources).ToList();
        GeneratedSourceResult generated = allGenerated.FirstOrDefault(x => x.HintName.Contains("MyId"));
        generated.Equals(default(GeneratedSourceResult)).ShouldBeFalse();
        var generatedText = generated.SourceText.ToString();
        generatedText.ShouldContain("public int Id");
        generatedText.ShouldContain("public readonly partial record struct MyId");
    }

    [Fact]
    public void DoesNotGenerate_ForNonReadonlyOrNonRecordStruct()
    {
        const string input = @"using AStar.Dev.Source.Generators.Annotations;
namespace TestNamespace
{
    [StrongId(typeof(int))]
    public partial struct MyId { }
}";

        var compilation = CSharpCompilation.Create("TestAssembly",
            new[]
            {
                CSharpSyntaxTree.ParseText(AttributeSource),
                CSharpSyntaxTree.ParseText(input)
            },
            new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location)
            },
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var generator = new StrongIdGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);
        GeneratorDriverRunResult result = driver.GetRunResult();
        var allGenerated = result.Results.SelectMany(r => r.GeneratedSources).ToList();
        GeneratedSourceResult generated = allGenerated.FirstOrDefault(x => x.HintName.Contains("MyId"));
        generated.Equals(default(GeneratedSourceResult)).ShouldBeTrue();
    }
}
