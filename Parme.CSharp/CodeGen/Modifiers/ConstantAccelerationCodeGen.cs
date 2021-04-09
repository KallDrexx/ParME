using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    internal class ConstantAccelerationCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(ConstantAccelerationModifier);

        public override FormattableString GenerateProperties(object obj)
        {
            var modifier = (ConstantAccelerationModifier) obj;
            
            return $@"
        public float ConstantAccelerationX {{ get; set; }} = {modifier.XAcceleration}f;
        public float ConstantAccelerationY {{ get; set; }} = {modifier.YAcceleration}f;
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"particle.Velocity += timeSinceLastFrame * new Vector2(ConstantAccelerationX, ConstantAccelerationY);
";
        }
    }
}