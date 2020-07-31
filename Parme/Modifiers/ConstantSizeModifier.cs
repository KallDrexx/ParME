using Microsoft.Xna.Framework;

namespace Parme.Modifiers
{
    public class ConstantSizeModifier : IParticleModifier
    {
        private readonly Vector2 _sizeAcceleration;

        public ConstantSizeModifier(Vector2 sizeAcceleration)
        {
            _sizeAcceleration = sizeAcceleration;
        }

        public void Update(float timeSinceLastFrame, Particle particle)
        {
            particle.Size += timeSinceLastFrame * _sizeAcceleration;
        }
    }
}