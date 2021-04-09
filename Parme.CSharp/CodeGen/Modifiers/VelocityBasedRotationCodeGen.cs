using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class VelocityBasedRotationCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(VelocityBasedRotationModifier);

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        if (particle.Velocity != Vector2.Zero)
                        {{
                            particle.RotationInRadians = (float) Math.Atan2(particle.Velocity.Y, particle.Velocity.X);
                        }}
";
        }
    }
}