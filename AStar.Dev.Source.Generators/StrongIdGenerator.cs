using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace AStar.Dev.Source.Generators
{
    [Generator]
    public class StrongIdGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var structDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => s is StructDeclarationSyntax,
                    transform: static (ctx, _) => (StructDeclarationSyntax)ctx.Node)
                .Where(static s => s.AttributeLists.Count > 0);

            var compilationAndStructs = context.CompilationProvider.Combine(structDeclarations.Collect());

            context.RegisterSourceOutput(compilationAndStructs, (spc, source) =>
            {
                var (compilation, structs) = source;
                var strongIdAttrSymbol = compilation.GetTypeByMetadataName("AStar.Dev.Source.Generators.Annotations.StrongIdAttribute");
                if (strongIdAttrSymbol == null) return;

                foreach (var structDecl in structs)
                {
                    var model = compilation.GetSemanticModel(structDecl.SyntaxTree);
                    var symbol = model.GetDeclaredSymbol(structDecl);
                    if (symbol == null)
                        continue;

                    // Only process readonly record structs
                    if (!structDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.ReadOnlyKeyword)) ||
                        !structDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)) ||
                        !structDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.RecordKeyword)))
                        continue;

                    // Check for the correct StrongIdAttribute
                    var attr = symbol.GetAttributes().FirstOrDefault(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, strongIdAttrSymbol));
                    if (attr == null)
                        continue;

                    // Get the IdType argument
                    if (attr.ConstructorArguments.Length != 1)
                        continue;
                    var idTypeArg = attr.ConstructorArguments[0];
                    if (idTypeArg.Value is not INamedTypeSymbol idTypeSymbol)
                        continue;

                    var ns = symbol.ContainingNamespace.IsGlobalNamespace ? "" : symbol.ContainingNamespace.ToString();
                    var structName = symbol.Name;
                    var idTypeName = idTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                    var sb = new StringBuilder();
                    if (!string.IsNullOrEmpty(ns))
                    {
                        sb.AppendLine($"namespace {ns}");
                        sb.AppendLine("{");
                    }
                    sb.AppendLine($"    public readonly partial record struct {structName}");
                    sb.AppendLine("    {");
                    sb.AppendLine($"        public {idTypeName} Id {{ get; }}");
                    sb.AppendLine("    }");
                    if (!string.IsNullOrEmpty(ns))
                        sb.AppendLine("}");

                    spc.AddSource($"{structName}_StrongId.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
                }
            });
        }
    }
}
