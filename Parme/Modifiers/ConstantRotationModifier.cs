using System;

namespace Parme.Modifiers
{
    public class ConstantRotationModifier : IParticleModifier
    {
        private readonly float _rotationChangeInRadians;

        public ConstantRotationModifier(float degreesPerSecond)
        {
            _rotationChangeInRadians = (float)(degreesPerSecond * Math.PI / 180);
        }
        
        public void Update(float timeSinceLastFrame, Particle particle)
        {
            particle.RotationInRadians += timeSinceLastFrame * _rotationChangeInRadians;
        }
    }
}