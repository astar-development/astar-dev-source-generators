using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AStar.Dev.Source.Generators;

[Generator]
public class StrongIdGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<RecordDeclarationSyntax> recordStructs = context.SyntaxProvider
            .CreateSyntaxProvider(
                    predicate: static (s, _) => s is RecordDeclarationSyntax rec && rec.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword),
                transform: static (ctx, _) => (RecordDeclarationSyntax)ctx.Node)
            .Where(static rds => rds.Modifiers.Any(m => m.IsKind(SyntaxKind.ReadOnlyKeyword)) &&
                                 rds.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)) &&
                                 rds.AttributeLists.Count > 0);

        IncrementalValueProvider<(Compilation Left, ImmutableArray<RecordDeclarationSyntax> Right)> compilationAndStructs = context.CompilationProvider.Combine(recordStructs.Collect());

        context.RegisterSourceOutput(compilationAndStructs, (spc, source) =>
        {
            (Compilation? compilation, ImmutableArray<RecordDeclarationSyntax> structs) = source;
            INamedTypeSymbol? strongIdAttrSymbol = compilation.GetTypeByMetadataName("AStar.Dev.Source.Generators.Attributes.StrongIdAttribute");
            Debug.WriteLine($"StrongIdAttribute symbol: {strongIdAttrSymbol?.ToDisplayString()}");
            if(strongIdAttrSymbol == null)
            {
                Debug.WriteLine("StrongIdAttribute symbol not found.");
                return;
            }

            foreach(RecordDeclarationSyntax? recordStruct in structs)
            {
                SemanticModel model = compilation.GetSemanticModel(recordStruct.SyntaxTree);
                INamedTypeSymbol? symbol = model.GetDeclaredSymbol(recordStruct);
                if(symbol == null)
                {
                    Debug.WriteLine($"Symbol not found for struct: {recordStruct.Identifier.Text}");
                    continue;
                }

                Debug.WriteLine($"Processing struct: {symbol.Name}");
                foreach(INamedTypeSymbol? attribute in symbol.GetAttributes().Select(s => s.AttributeClass))
                {
                    Debug.WriteLine($"  Attribute: {attribute?.ToDisplayString()}");
                    Debug.WriteLine($"    SymbolEqualityComparer: {SymbolEqualityComparer.Default.Equals(attribute, strongIdAttrSymbol)}");
                }

                AttributeData? attr = symbol.GetAttributes().FirstOrDefault(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, strongIdAttrSymbol));
                if(attr == null)
                {
                    Debug.WriteLine($"  No matching StrongIdAttribute found for {symbol.Name}.");
                    continue;
                }

                if(attr.ConstructorArguments.Length > 1)
                {
                    Debug.WriteLine($"  StrongIdAttribute on {symbol.Name} does not have exactly one constructor argument.");
                    continue;
                }

                INamedTypeSymbol idTypeSymbol;
                if(attr.ConstructorArguments.Length == 0)
                {
                    // Default to Guid if no argument is provided
                    idTypeSymbol = compilation.GetTypeByMetadataName("System.Guid");
                    if(idTypeSymbol == null)
                    {
                        Debug.WriteLine("  Could not resolve System.Guid.");
                        continue;
                    }
                }
                else
                {
                    TypedConstant idTypeArg = attr.ConstructorArguments[0];
                    if(idTypeArg.Value is not INamedTypeSymbol idTypeArgSymbol)
                    {
                        Debug.WriteLine($"  StrongIdAttribute argument is not a type symbol on {symbol.Name}.");
                        continue;
                    }

                    idTypeSymbol = idTypeArgSymbol;
                }

                var ns = symbol.ContainingNamespace.IsGlobalNamespace ? null : symbol.ContainingNamespace.ToString();
                var structName = symbol.Name;
                var idTypeName = idTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                var sb = new StringBuilder();
                if(!string.IsNullOrEmpty(ns))
                {
                    _ = sb.AppendLine($"namespace {ns}");
                    _ = sb.AppendLine("{");
                }

                _ = sb.AppendLine($"    public readonly partial record struct {structName}({idTypeName} Id)");

                if(!string.IsNullOrEmpty(ns))
                    _ = sb.AppendLine("}");

                spc.AddSource($"{structName}_StrongId.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
                Debug.WriteLine($"  Generated code for {structName}.");
            }
        });
    }
}
