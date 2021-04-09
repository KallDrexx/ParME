using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.PositionModifier;
using Parme.Core.Serialization;
using Parme.Core.Triggers;

namespace Parme.Core
{
    public class EmitterSettings
    {
        public float MaxParticleLifeTime { get; set; }
        public string TextureFileName { get; set; }
        public IReadOnlyList<TextureSectionCoords> TextureSections { get; set; } = Array.Empty<TextureSectionCoords>();
        public IParticleTrigger Trigger { get; set; }
        public IReadOnlyList<IParticleInitializer> Initializers { get; set; } = Array.Empty<IParticleInitializer>();
        public IReadOnlyList<IParticleModifier> Modifiers { get; set; } = Array.Empty<IParticleModifier>();
        public IParticlePositionModifier PositionModifier { get; set; }

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
            var allParticleTypes =
                typeof(IParticleTrigger).Assembly
                    .GetTypes()
                    .Where(x => !x.IsAbstract)
                    .Where(x => !x.IsInterface)
                    .Where(x => typeof(IParticleTrigger).IsAssignableFrom(x) ||
                                typeof(IParticleInitializer).IsAssignableFrom(x) ||
                                typeof(IParticleModifier).IsAssignableFrom(x) ||
                                typeof(IParticlePositionModifier).IsAssignableFrom(x));
            
            var typeNameMap = new SerializedTypeNameMap();
            foreach (var type in allParticleTypes)
            {
                typeNameMap.AddType(type);
            }
            
            return new ParticleJsonConverter(typeNameMap);
        }
    }
}