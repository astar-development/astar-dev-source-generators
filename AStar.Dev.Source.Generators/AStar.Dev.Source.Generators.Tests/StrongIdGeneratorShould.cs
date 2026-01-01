
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using Shouldly;

namespace AStar.Dev.Source.Generators.Tests;

public class StrongIdGeneratorShould
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
    public async Task GenerateIdPropertyForValidReadonlyRecordStruct()
    {
        var source = @"using AStar.Dev.Source.Generators.Annotations;
namespace TestNamespace
{
    [StrongId(typeof(int))]
    public readonly partial record struct MyId { }
}";
        var expectedGenerated = @"namespace TestNamespace
{
    public readonly partial record struct MyId
    {
        public int Id { get; }
    }
}";

        var test = new CSharpSourceGeneratorTest<StrongIdGeneratorWrapper, XUnitVerifier>
        {
            TestState =
            {
                Sources = { AttributeSource, source },
                ReferenceAssemblies = ReferenceAssemblies.Net.Net60
            }
        };

        test.TestState.GeneratedSources.Add((typeof(StrongIdGeneratorWrapper), "MyId_StrongId.g.cs", expectedGenerated));

        await test.RunAsync();
    }

    [Fact]
    public async Task NotGenerateForNonReadonlyOrNonRecordStruct()
    {
        var source = @"using AStar.Dev.Source.Generators.Annotations;
namespace TestNamespace
{
    [StrongId(typeof(int))]
    public partial struct MyId { }
}";
        var test = new CSharpSourceGeneratorTest<StrongIdGeneratorWrapper, XUnitVerifier>
        {
            TestState =
            {
                Sources = { AttributeSource, source },
                ReferenceAssemblies = ReferenceAssemblies.Net.Net60
            }
        };

        await test.RunAsync();
    }
}
