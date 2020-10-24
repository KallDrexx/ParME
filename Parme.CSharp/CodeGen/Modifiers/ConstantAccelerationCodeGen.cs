using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    internal class ConstantAccelerationCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(ConstantAccelerationModifier);

        public FormattableString GenerateProperties(object obj)
        {
            var modifier = (ConstantAccelerationModifier) obj;
            
            return $@"
        public float ConstantAccelerationX {{ get; set; }} = {modifier.XAcceleration}f;
        public float ConstantAccelerationY {{ get; set; }} = {modifier.YAcceleration}f;
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"particle.Velocity += timeSinceLastFrame * new Vector2(ConstantAccelerationX, ConstantAccelerationY);
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}