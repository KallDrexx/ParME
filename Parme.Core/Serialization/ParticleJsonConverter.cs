using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;

namespace Parme.Core.Serialization
{
    internal class ParticleJsonConverter : JsonConverter
    {
        private const string TypeFieldName = "$ParmeType";
        private readonly SerializedTypeNameMap _typeNameMap;

        public ParticleJsonConverter(SerializedTypeNameMap typeNameMap)
        {
            _typeNameMap = typeNameMap;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IParticleTrigger) ||
                   objectType == typeof(IParticleInitializer) ||
                   objectType == typeof(IParticleModifier) ||
                   typeof(IParticleTrigger).IsAssignableFrom(objectType) ||
                   typeof(IParticleInitializer).IsAssignableFrom(objectType) ||
                   typeof(IParticleModifier).IsAssignableFrom(objectType);
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();
            var parmeTypeName = type.GetCustomAttributes<SerializedTypeNameAttribute>()
                .Where(x => x.UsedForSerialization)
                .Select(x => x.TypeName)
                .FirstOrDefault() ?? type.Name;

            var jObject = JObject.FromObject(value);
            jObject[TypeFieldName] = parmeTypeName;
            jObject.Remove("InitializerType"); // Get only property, not needed in serialization
            jObject.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            if (jObject[TypeFieldName] == null)
            {
                var message = $"Json did not have the expected expected field: '{TypeFieldName}'";
                throw new InvalidOperationException(message);
            }

            var typeName = jObject[TypeFieldName].ToString();
            if (!_typeNameMap.Map.TryGetValue(typeName, out var concreteType))
            {
                var message = $"No known association for particle type of '{typeName}'";
                throw new InvalidOperationException(message);
            }

            var instance = Activator.CreateInstance(concreteType);
            serializer.Populate(jObject.CreateReader(), instance);

            return instance;
        }
    }
}