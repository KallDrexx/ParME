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
        
        public void Update(float timeSinceLastFrame, ref Particle particle)
        {
            particle.RotationInRadians += timeSinceLastFrame * _rotationChangeInRadians;
        }

        public string GetCSharpExecutionCode()
        {
            return $@"
                particle.RotationInRadians += timeSinceLastFrame * (float) {_rotationChangeInRadians};                
";
        }
    }
}