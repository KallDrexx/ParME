using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Serialization;
using Parme.Core.Triggers;

namespace Parme.Core
{
    public class EmitterSettings
    {
        public float MaxParticleLifeTime { get; set; }
        public string TextureFileName { get; set; }
        public Dictionary<int, TextureSectionCoords> TextureSections { get; set; } = new Dictionary<int, TextureSectionCoords>();
        public IParticleTrigger Trigger { get; set; }
        public IReadOnlyList<IParticleInitializer> Initializers { get; set; }
        public IReadOnlyList<IParticleModifier> Modifiers { get; set; }

        public static EmitterSettings FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException(nameof(json));
            }

            return JsonConvert.DeserializeObject<EmitterSettings>(json, GetParticleJsonConverter());
        }
        
        /// <summary>
        /// Serializes the emitter into json
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this,
                Formatting.Indented,
                GetParticleJsonConverter());
        }

        private static ParticleJsonConverter GetParticleJsonConverter()
        {
            var triggerTypes = typeof(IParticleTrigger).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(IParticleTrigger).IsAssignableFrom(x));

            var initializerTypes = typeof(IParticleInitializer).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(IParticleInitializer).IsAssignableFrom(x));
            
            var modifierTypes = typeof(IParticleModifier).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(IParticleModifier).IsAssignableFrom(x));
            
            var typeNameMap = new SerializedTypeNameMap();
            foreach (var type in triggerTypes.Union(initializerTypes).Union(modifierTypes))
            {
                typeNameMap.AddType(type);
            }
            
            return new ParticleJsonConverter(typeNameMap);
        }
    }
}