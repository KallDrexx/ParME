﻿using Microsoft.Xna.Framework;
using Parme.Scripting;

namespace Parme.Modifiers
{
    public class ConstantSizeModifier : IParticleModifier
    {
        private readonly Vector2 _sizeAcceleration;

        public ConstantSizeModifier(Vector2 sizeAcceleration)
        {
            _sizeAcceleration = sizeAcceleration;
        }

        public void Update(float timeSinceLastFrame, ref Particle particle)
        {
            particle.Size += timeSinceLastFrame * _sizeAcceleration;
        }

        public string GetCSharpExecutionCode()
        {
            return $@"
                particle.Size += timeSinceLastFrame * {_sizeAcceleration.ToCSharpScriptString()};
";
        }
    }
}