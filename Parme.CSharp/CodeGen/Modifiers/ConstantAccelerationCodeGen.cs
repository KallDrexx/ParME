using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class ConstantAccelerationCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(ConstantAccelerationModifier);

        public string GenerateProperties(object obj)
        {
            var modifier = (ConstantAccelerationModifier) obj;
            
            return $@"
        public float ConstantAccelerationX {{ get; set; }} = {modifier.XAcceleration}f;
        public float ConstantAccelerationY {{ get; set; }} = {modifier.YAcceleration}f;
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"particle.Velocity += timeSinceLastFrame * new Vector2(ConstantAccelerationX, ConstantAccelerationY);
";
        }
    }
}