using System.Runtime.CompilerServices;

namespace AStar.Dev.Source.Generators.Generators.Tests.Infrastructure;

/// <summary>
/// Configures Verify snapshot settings once per test assembly load.
/// </summary>
public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        // Store snapshots under Snapshots/ next to the test source files.
        VerifierSettings.InitializePlugins();
        VerifySourceGenerators.Initialize();
    }
}
