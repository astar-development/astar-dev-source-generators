namespace AStar.Dev.Source.Generators.Generators.OptionsConfiguration;

/// <summary>
/// Immutable data extracted from a single <c>[OptionsConfiguration]</c> annotation.
/// Kept pure (no Roslyn symbols) so it is cheap to cache in the incremental pipeline.
/// </summary>
internal sealed class OptionsConfigurationModel(
    string Namespace,
    string ClassName,
    string SectionName
);
