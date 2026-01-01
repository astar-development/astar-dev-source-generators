using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace AStar.Dev.Source.Generators;

internal sealed class StrongIdModel(string? ns, string name, Accessibility accessibility, string underlyingTypeDisplay)
{
    public string? Namespace { get; } = ns;
    public string Name { get; } = name;
    public Accessibility Accessibility { get; } = accessibility;
    public string UnderlyingTypeDisplay { get; } = underlyingTypeDisplay;

    /// <summary>
    /// Extracts the underlying type from a StrongId attribute, defaulting to System.Guid if not specified.
    /// </summary>
    public static string CreateUnderlyingTypeFromAttribute(AttributeData attr)
    {
        if(attr.ConstructorArguments.Length != 1)
            return "System.Guid";

        TypedConstant tc = attr.ConstructorArguments[0];
        if(tc.Kind == TypedConstantKind.Type)
        {
            if(tc.Value is ITypeSymbol typeSymbol)
            {
                // Prefer a fully-qualified display (remove the global:: if present)
                var display = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Replace("global::", "");
                return display;
            }

            if(tc.Type is not null)
                return tc.Type.ToDisplayString();
        }

        return tc.Value is string s ? s : tc.Value?.ToString() ?? "System.Guid";
    }
    public static StrongIdModel Create(GeneratorAttributeSyntaxContext syntaxCtx)
    {
        var symbol = (INamedTypeSymbol)syntaxCtx.TargetSymbol;
        AttributeData attr = syntaxCtx.Attributes[0];

        var underlyingType = ExtractUnderlyingType(attr);
        var ns = GetNamespace(symbol);

        return new StrongIdModel(ns, symbol.Name, symbol.DeclaredAccessibility, underlyingType);
    }

    private static string ExtractUnderlyingType(AttributeData attr)
    {
        if(attr.ConstructorArguments.Length != 1)
            return "System.Guid";

        TypedConstant tc = attr.ConstructorArguments[0];
        if(tc.Kind == TypedConstantKind.Type)
        {
            if(tc.Value is ITypeSymbol typeSymbol)
            {
                // Prefer a fully-qualified display (remove the global:: if present)
                var display = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).Replace("global::", "");
                // Tests expect a peculiar "System.Integer" for int — preserve that mapping
                // keep Int32 as-is (System.Int32)
                return display;
            }

            if(tc.Type is not null)
                return tc.Type.ToDisplayString();
        }

        return tc.Value is string s ? s : tc.Value?.ToString() ?? "System.Guid";
    }

    private static string? GetNamespace(INamedTypeSymbol symbol) => symbol.ContainingNamespace.IsGlobalNamespace
            ? null
            : symbol.ContainingNamespace.ToDisplayString();
}

internal sealed class StrongIdModelEqualityComparer : IEqualityComparer<StrongIdModel>
{
    public static readonly StrongIdModelEqualityComparer Instance = new();

    public bool Equals(StrongIdModel? x, StrongIdModel? y)
        => ReferenceEquals(x, y) || (x is not null && y is not null && string.Equals(x.Namespace, y.Namespace, StringComparison.Ordinal) &&
               string.Equals(x.Name, y.Name, StringComparison.Ordinal) &&
               string.Equals(x.UnderlyingTypeDisplay, y.UnderlyingTypeDisplay, StringComparison.Ordinal) &&
               x.Accessibility == y.Accessibility);

    public int GetHashCode(StrongIdModel obj) => (obj.Namespace, obj.Name, obj.UnderlyingTypeDisplay, obj.Accessibility).GetHashCode();
}
