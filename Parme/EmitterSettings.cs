using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Parme.Initializers.ColorInitializer;
using Parme.Initializers.ParticleCountInitializer;
using Parme.Initializers.PositionalInitializers;
using Parme.Initializers.SizeInitializers;
using Parme.Initializers.VelocityInitializers;
using Parme.Modifiers;
using Parme.Triggers;

namespace Parme
{
    public class EmitterSettings
    {
        public Texture2D ParticleTexture { get; set; }
        public float MaxParticleLifeTime { get; set; }
        public ITrigger Trigger { get; set; }
        public IParticleCountInitializer ParticleCountInitializer { get; set; }
        public IColorInitializer ColorInitializer { get; set; }
        public IPositionalInitializer PositionalInitializer { get; set; }
        public ISizeInitializer SizeInitializer { get; set; }
        public IVelocityInitializer VelocityInitializer { get; set; }
        public List<IParticleModifier> Modifiers { get; } = new List<IParticleModifier>();
    }
}