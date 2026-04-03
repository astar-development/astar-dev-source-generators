using System;

namespace AStar.Dev.Source.Generators.Abstractions;

/// <summary>
/// Marks a class as an <c>IOptions&lt;T&gt;</c> configuration type.
/// The source generator will emit a <c>services.Configure&lt;T&gt;(config.GetSection(...))</c>
/// call inside the generated <c>AddGeneratedOptions()</c> extension method.
/// </summary>
/// <example>
/// <code>
/// [OptionsConfiguration("PaymentGateway")]
/// public sealed class PaymentGatewayOptions
/// {
///     public string ApiKey { get; set; } = string.Empty;
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class OptionsConfigurationAttribute : Attribute
{
    /// <summary>
    /// The configuration section key passed to <c>IConfiguration.GetSection()</c>.
    /// </summary>
    public string SectionName { get; }

    public OptionsConfigurationAttribute(string sectionName)
        => SectionName = sectionName;
}
