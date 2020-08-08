using System;
using System.Collections.Generic;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;

namespace Parme.CSharp
{
    public class EmitterSettings
    {
        public float MaxParticleLifeTime { get; }
        public IParticleTrigger Trigger { get; }
        public IReadOnlyList<IParticleInitializer> Initializers { get; }
        public IReadOnlyList<IParticleModifier> Modifiers { get; }

        public EmitterSettings(IParticleTrigger trigger,
            IEnumerable<IParticleInitializer> initializers,
            IEnumerable<IParticleModifier> modifiers,
            float maxParticleLifeTime)
        {
            MaxParticleLifeTime = maxParticleLifeTime;
            Trigger = trigger ?? throw new ArgumentNullException(nameof(trigger));
            Initializers = new List<IParticleInitializer>(initializers);
            Modifiers = new List<IParticleModifier>(modifiers);
        }
    }
}