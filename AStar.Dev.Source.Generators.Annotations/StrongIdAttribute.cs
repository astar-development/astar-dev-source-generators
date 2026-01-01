using System;

namespace AStar.Dev.Source.Generators.Annotations
{
    /// <summary>
    /// Indicates that the target is a strong ID type. Intended for use only on readonly record structs.
    /// This is not enforced by the compiler, but should be validated by source generators.
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class StrongIdAttribute : Attribute
    {
        /// <summary>
        /// The type of the ID property (e.g., typeof(Guid), typeof(int)).
        /// </summary>
        public Type IdType { get; }

        public StrongIdAttribute(Type idType) => IdType = idType ?? throw new ArgumentNullException(nameof(idType));
    }
}
