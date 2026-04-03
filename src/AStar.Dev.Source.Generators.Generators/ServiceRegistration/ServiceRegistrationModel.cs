namespace AStar.Dev.Source.Generators.Generators.ServiceRegistration;

/// <summary>
/// Immutable data extracted from a single <c>[RegisterService]</c> annotation.
/// Kept pure (no Roslyn symbols) so it is cheap to cache in the incremental pipeline.
/// </summary>
internal sealed class ServiceRegistrationModel
    {
        public string Namespace { get; }
        public string ClassName { get; }
        public string[] InterfaceNames { get; }
        public string Lifetime { get; }   // "Singleton" | "Scoped" | "Transient"
        public bool RegisterAsInterfaces { get; }

        public ServiceRegistrationModel(
            string @namespace,
            string className,
            string[] interfaceNames,
            string lifetime,
            bool registerAsInterfaces)
        {
            Namespace = @namespace;
            ClassName = className;
            InterfaceNames = interfaceNames;
            Lifetime = lifetime;
            RegisterAsInterfaces = registerAsInterfaces;
        }
    }
