using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AStar.Dev.Source.Generators.Generators.Infrastructure;

internal static class RoslynExtensions
{
    /// <summary>
    /// Returns <c>true</c> when the compilation references EF Core
    /// (detected by the presence of <c>Microsoft.EntityFrameworkCore.DbContext</c>).
    /// </summary>
    internal static bool ReferencesEfCore(this Compilation compilation)
        => compilation.GetTypeByMetadataName("Microsoft.EntityFrameworkCore.DbContext") is not null;

    /// <summary>
    /// Returns the fully-qualified namespace of a symbol, or an empty string
    /// for the global namespace.
    /// </summary>
    internal static string GetFullNamespace(this INamespaceSymbol ns)
        => ns.IsGlobalNamespace ? string.Empty : ns.ToDisplayString();

    /// <summary>
    /// Returns all non-object interfaces directly or transitively implemented
    /// by a named type, as fully-qualified display strings.
    /// </summary>
    internal static IEnumerable<string> GetAllInterfaceNames(this INamedTypeSymbol type)
        => type.AllInterfaces.Select(static i => i.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

    /// <summary>
    /// Returns <c>true</c> if <paramref name="syntax"/> carries the <c>partial</c> modifier.
    /// </summary>
    internal static bool IsPartial(this TypeDeclarationSyntax syntax)
        => syntax.Modifiers.Any(static m => m.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword));

    /// <summary>
    /// Returns <c>true</c> if <paramref name="syntax"/> carries the <c>readonly</c> modifier.
    /// </summary>
    internal static bool IsReadOnly(this TypeDeclarationSyntax syntax)
        => syntax.Modifiers.Any(static m => m.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.ReadOnlyKeyword));
}
