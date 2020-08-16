using System;

namespace Parme.Core.Serialization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SerializedTypeNameAttribute : Attribute
    {
        public string TypeName { get; }
        public bool UsedForSerialization { get; }

        public SerializedTypeNameAttribute(string typeName, bool usedForSerialization = false)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentNullException(nameof(typeName));
            }
            
            TypeName = typeName.Trim();
            UsedForSerialization = usedForSerialization;
        }
    }
}