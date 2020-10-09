using System;

namespace Parme.Core.Serialization
{
    public class MissingParmeTypeException : Exception
    {
        public string TypeName { get; }

        public MissingParmeTypeException(string typeName)
            : base($"Emitter logic definition requires the parme type '{typeName}' for deseralization, " +
                   $"but that type is not known")
        {
            TypeName = typeName;
        }
    }
}