using System.Collections.Generic;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;

namespace Parme.CSharp
{
    public class EmitterSettings
    {
        public float MaxParticleLifeTime { get; set; }
        public IParticleTrigger Trigger { get; set; }
        public List<IParticleInitializer> Initializers { get; } = new List<IParticleInitializer>();
        public List<IParticleModifier> Modifiers { get; } = new List<IParticleModifier>();
    }
}