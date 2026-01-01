using Microsoft.CodeAnalysis;

namespace AStar.Dev.Source.Generators.Tests
{
    public class StrongIdGeneratorWrapper : ISourceGenerator
    {
        private readonly StrongIdGenerator _incremental = new();

        public void Initialize(GeneratorInitializationContext context)
        {
            // This is a no-op for incremental generators in classic test harnesses
            // The test harness will call Execute, which is also a no-op for incremental generators
            ((IIncrementalGenerator)_incremental).Initialize(context);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // No-op for incremental generators
        }
    }
}
