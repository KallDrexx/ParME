using System;
using Parme.Core.PositionModifier;

namespace Parme.CSharp.CodeGen.PositionModifiers
{
    public class AltitudeBouncePositionModifierCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(AltitudeBouncePositionModifier);

        public override FormattableString GenerateProperties(object obj)
        {
            var modifier = (AltitudeBouncePositionModifier) obj;
            return $@"
        public float AltitudeMinBounceAcceleration {{ get; set; }} = {modifier.MinBounceAcceleration}f;
        public float AltitudeMaxBounceAcceleration {{ get; set; }} = {modifier.MaxBounceAcceleration}f;
        public float AltitudeBounceGravity {{ get; set; }} = {modifier.Gravity}f;
        public float AltitudeElasticity {{ get; set; }} = {modifier.Elasticity}f;
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        particle.AltitudeVelocity -= AltitudeBounceGravity * timeSinceLastFrame;
                        particle.Altitude += particle.AltitudeVelocity;
                        if (particle.Altitude <= 0)
                        {{
                            particle.Altitude = 0;
                            var accelerationModifier = (float) Math.Pow(AltitudeElasticity, particle.AltitudeBounceCount);
                            var acceleration = AltitudeMinBounceAcceleration + (float) _random.NextDouble() * (AltitudeMaxBounceAcceleration - AltitudeMinBounceAcceleration);
                            particle.AltitudeVelocity = acceleration * accelerationModifier;
                            particle.AltitudeBounceCount++;
                        }}
";
        }

        public override FormattableString GeneratePositionExecutionCode(object obj)
        {
            return $@"
                particle.ReferencePosition += particle.Velocity * timeSinceLastFrame;
                particle.Position.X = particle.ReferencePosition.X;
                particle.Position.Y = particle.ReferencePosition.Y + particle.Altitude;
";
        }
    }
}
