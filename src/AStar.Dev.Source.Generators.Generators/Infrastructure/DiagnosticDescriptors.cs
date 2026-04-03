using Microsoft.CodeAnalysis;

namespace AStar.Dev.Source.Generators.Generators.Infrastructure;

/// <summary>
/// All diagnostic IDs and descriptors emitted by the generator suite.
/// Convention: ASGXXXX where ASG = AStar Generators.
/// </summary>
internal static class DiagnosticDescriptors
{
    private const string Category = "AStar.Dev.Source.Generators";

    // ── [RegisterService] ────────────────────────────────────────────────────

    public static readonly DiagnosticDescriptor ServiceMustBePartial = new(
        id:             "ASG0001",
        title:          "[RegisterService] class must be partial",
        messageFormat:  "Class '{0}' is annotated with [RegisterService] but is not declared partial. The generated registration code requires the class to be partial.",
        category:       Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor ServiceIsAbstract = new(
        id:             "ASG0002",
        title:          "[RegisterService] cannot be applied to an abstract class",
        messageFormat:  "Class '{0}' is abstract and cannot be registered as a service. Remove [RegisterService] or make the class concrete.",
        category:       Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    // ── [OptionsConfiguration] ───────────────────────────────────────────────

    public static readonly DiagnosticDescriptor OptionsSectionNameEmpty = new(
        id:             "ASG0010",
        title:          "[OptionsConfiguration] section name must not be empty",
        messageFormat:  "Class '{0}' has an empty SectionName in [OptionsConfiguration]. Provide a non-empty configuration section key.",
        category:       Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    // ── [StronglyTypedId] ────────────────────────────────────────────────────

    public static readonly DiagnosticDescriptor IdMustBeReadonlyPartialRecordStruct = new(
        id:             "ASG0020",
        title:          "[StronglyTypedId] target must be a readonly partial record struct",
        messageFormat:  "'{0}' is annotated with [StronglyTypedId] but is not declared as a 'readonly partial record struct'. Change the declaration or remove the attribute.",
        category:       Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor IdUnsupportedBackingType = new(
        id:             "ASG0021",
        title:          "[StronglyTypedId] unsupported backing type",
        messageFormat:  "'{0}' specifies an unsupported backing type '{1}'. Supported types are: Guid, int, long, string.",
        category:       Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}
