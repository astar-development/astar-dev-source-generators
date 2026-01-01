using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Shouldly;
using Xunit;

namespace AStar.Dev.Source.Generators.Tests;

public class ServiceRegistrationGeneratorShould
{
    private const string AttributeSource = @"using System;\nnamespace AStar.Dev.Source.Generators.Attributes {\n    public enum ServiceLifetime { Singleton, Scoped, Transient }\n    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]\n    public sealed class ServiceAttribute : Attribute {\n        public ServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped) { Lifetime = lifetime; }\n        public ServiceLifetime Lifetime { get; }\n        public Type? As { get; set; }\n        public bool AsSelf { get; set; } = false;\n    }\n}";

    private static CSharpCompilation CreateCompilation(string input)
        => CSharpCompilation.Create("TestAssembly",
            [
                CSharpSyntaxTree.ParseText(AttributeSource),
                CSharpSyntaxTree.ParseText(input)
            ],
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location)
            ],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

    [Fact]
    public void RegisterClassWithSingleInterface_DefaultScoped()
    {
        const string input = @"using AStar.Dev.Source.Generators.Attributes;\npublic interface IFoo {}\n[Service]\npublic class Foo : IFoo {}";
        CSharpCompilation compilation = CreateCompilation(input);
        var generator = new ServiceRegistrationGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);
        GeneratorDriverRunResult result = driver.GetRunResult();
        GeneratedSourceResult generated = result.Results.SelectMany(r => r.GeneratedSources).FirstOrDefault(x => x.HintName.Contains("ServiceCollectionExtensions"));
        generated.Equals(default(GeneratedSourceResult)).ShouldBeFalse();
        var text = generated.SourceText.ToString();
        text.ShouldContain("s.AddScoped<IFoo, Foo>();");
    }

    [Fact]
    public void RegisterClassWithSingleInterface_SingletonLifetime()
    {
        const string input = @"using AStar.Dev.Source.Generators.Attributes;\npublic interface IFoo {}\n[Service(ServiceLifetime.Singleton)]\npublic class Foo : IFoo {}";
        CSharpCompilation compilation = CreateCompilation(input);
        var generator = new ServiceRegistrationGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);
        GeneratorDriverRunResult result = driver.GetRunResult();
        GeneratedSourceResult generated = result.Results.SelectMany(r => r.GeneratedSources).FirstOrDefault(x => x.HintName.Contains("ServiceCollectionExtensions"));
        generated.Equals(default(GeneratedSourceResult)).ShouldBeFalse();
        var text = generated.SourceText.ToString();
        text.ShouldContain("s.AddSingleton<IFoo, Foo>();");
    }

    [Fact]
    public void RegisterClassWithSingleInterface_AsSelfTrue()
    {
        const string input = @"using AStar.Dev.Source.Generators.Attributes;\npublic interface IFoo {}\n[Service(AsSelf = true)]\npublic class Foo : IFoo {}";
        CSharpCompilation compilation = CreateCompilation(input);
        var generator = new ServiceRegistrationGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);
        GeneratorDriverRunResult result = driver.GetRunResult();
        GeneratedSourceResult generated = result.Results.SelectMany(r => r.GeneratedSources).FirstOrDefault(x => x.HintName.Contains("ServiceCollectionExtensions"));
        generated.Equals(default(GeneratedSourceResult)).ShouldBeFalse();
        var text = generated.SourceText.ToString();
        text.ShouldContain("s.AddScoped<IFoo, Foo>();");
        text.ShouldContain("s.AddScoped<Foo>();");
    }

    [Fact]
    public void RegisterClassWithSingleInterface_AsOverride()
    {
        const string input = @"using AStar.Dev.Source.Generators.Attributes;\npublic interface IFoo {}\npublic interface IBar {}\n[Service(As = typeof(IBar))]\npublic class Foo : IFoo, IBar {}";
        CSharpCompilation compilation = CreateCompilation(input);
        var generator = new ServiceRegistrationGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);
        GeneratorDriverRunResult result = driver.GetRunResult();
        GeneratedSourceResult generated = result.Results.SelectMany(r => r.GeneratedSources).FirstOrDefault(x => x.HintName.Contains("ServiceCollectionExtensions"));
        generated.Equals(default(GeneratedSourceResult)).ShouldBeFalse();
        var text = generated.SourceText.ToString();
        text.ShouldContain("s.AddScoped<IBar, Foo>();");
    }

    [Fact]
    public void RegisterClassWithNoInterface_AsSelfTrue()
    {
        const string input = @"using AStar.Dev.Source.Generators.Attributes;\n[Service(AsSelf = true)]\npublic class Foo {}";
        CSharpCompilation compilation = CreateCompilation(input);
        var generator = new ServiceRegistrationGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);
        GeneratorDriverRunResult result = driver.GetRunResult();
        GeneratedSourceResult generated = result.Results.SelectMany(r => r.GeneratedSources).FirstOrDefault(x => x.HintName.Contains("ServiceCollectionExtensions"));
        generated.Equals(default(GeneratedSourceResult)).ShouldBeFalse();
        var text = generated.SourceText.ToString();
        text.ShouldContain("s.AddScoped<Foo>();");
    }

    [Fact]
    public void DoesNotRegisterAbstractOrNonPublicOrGenericClasses()
    {
        const string input = @"using AStar.Dev.Source.Generators.Attributes;\npublic interface IFoo {}\n[Service] public abstract class AbstractFoo : IFoo {}\n[Service] internal class InternalFoo : IFoo {}\n[Service] public class GenericFoo<T> : IFoo {}";
        CSharpCompilation compilation = CreateCompilation(input);
        var generator = new ServiceRegistrationGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);
        GeneratorDriverRunResult result = driver.GetRunResult();
        GeneratedSourceResult generated = result.Results.SelectMany(r => r.GeneratedSources).FirstOrDefault(x => x.HintName.Contains("ServiceCollectionExtensions"));
        generated.Equals(default(GeneratedSourceResult)).ShouldBeFalse();
        var text = generated.SourceText.ToString();
        text.ShouldNotContain("AbstractFoo");
        text.ShouldNotContain("InternalFoo");
        text.ShouldNotContain("GenericFoo");
    }

    [Fact]
    public void DoesNotRegisterClassWithoutServiceAttribute()
    {
        const string input = @"using AStar.Dev.Source.Generators.Attributes;\npublic interface IFoo {}\npublic class Foo : IFoo {}";
        CSharpCompilation compilation = CreateCompilation(input);
        var generator = new ServiceRegistrationGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);
        GeneratorDriverRunResult result = driver.GetRunResult();
        GeneratedSourceResult generated = result.Results.SelectMany(r => r.GeneratedSources).FirstOrDefault(x => x.HintName.Contains("ServiceCollectionExtensions"));
        // Should not generate any registration for Foo
        if(!generated.Equals(default(GeneratedSourceResult)))
        {
            var text = generated.SourceText.ToString();
            text.ShouldNotContain("Foo");
        }
    }

    [Fact]
    public void DoesNotRegisterClassWithMultipleInterfacesAndNoAsSpecified()
    {
        const string input = @"using AStar.Dev.Source.Generators.Attributes;\npublic interface IFoo {}\npublic interface IBar {}\n[Service]\npublic class Foo : IFoo, IBar {}";
        CSharpCompilation compilation = CreateCompilation(input);
        var generator = new ServiceRegistrationGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        driver = (CSharpGeneratorDriver)driver.RunGenerators(compilation);
        GeneratorDriverRunResult result = driver.GetRunResult();
        GeneratedSourceResult generated = result.Results.SelectMany(r => r.GeneratedSources).FirstOrDefault(x => x.HintName.Contains("ServiceCollectionExtensions"));
        // Should not generate any registration for Foo
        if(!generated.Equals(default(GeneratedSourceResult)))
        {
            var text = generated.SourceText.ToString();
            text.ShouldNotContain("Foo");
        }
    }
}
