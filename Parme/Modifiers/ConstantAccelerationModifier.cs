using Microsoft.Xna.Framework;

namespace Parme.Modifiers
{
    public class ConstantAccelerationModifier : IParticleModifier
    {
        private readonly Vector2 _acceleration;

        public ConstantAccelerationModifier(Vector2 acceleration)
        {
            _acceleration = acceleration;
        }

        public void Update(float timeSinceLastFrame, ref Particle particle)
        {
            particle.Velocity += timeSinceLastFrame * _acceleration;
        }
    }
}