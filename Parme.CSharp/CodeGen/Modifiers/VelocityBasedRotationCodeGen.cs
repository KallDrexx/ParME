using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class VelocityBasedRotationCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(VelocityBasedRotationModifier);

        public FormattableString GenerateProperties(object obj)
        {
            return $"";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        if (particle.Velocity != Vector2.Zero)
                        {{
                            particle.RotationInRadians = (float) Math.Atan2(particle.Velocity.Y, particle.Velocity.X);
                        }}
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}