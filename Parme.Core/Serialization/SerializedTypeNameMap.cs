using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Parme.Core.Serialization
{
    /// <summary>
    /// Provides a lookup table for what type should be serialized by a specific type name.  One type can be referred
    /// to by multiple type names (to allow for backward compatibility of name changes) but each type name can only
    /// reference a single type
    /// </summary>
    internal class SerializedTypeNameMap
    {
        private readonly Dictionary<string, Type> _map = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyDictionary<string, Type> Map => _map;
        
        public void AddType(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                var message = $"{type.FullName} is an abstract or non-concrete type, and therefore cannot be mapped";
                throw new ArgumentException(message);
            }

            var aliases = type.GetCustomAttributes<SerializedTypeNameAttribute>()
                .Where(x => !string.IsNullOrWhiteSpace(x?.TypeName))
                .Select(x => x.TypeName.Trim())
                .Union(new[] {type.Name})
                .ToArray();

            foreach (var typeName in aliases)
            {
                if (_map.TryGetValue(typeName, out var existingType))
                {
                    if (existingType == type)
                    {
                        // This alias is already defined for the existing type, so ignore it.
                        continue;
                    }

                    var message = $"Existing type name defined of '{typeName}' for type {type.FullName} and {existingType.FullName}";
                    throw new InvalidOperationException(message);
                }
                
                _map.Add(typeName, type);
            }
        }
    }
}