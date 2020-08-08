using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class ConstantRotationCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(ConstantRotationModifier);

        public string GenerateProperties(object obj)
        {
            var modifier = (ConstantRotationModifier) obj;
            
            return $@"
        public float ConstantRotationRadiansPerSecond {{ get; set; }} = {modifier.DegreesPerSecond * (Math.PI / 180f)}f;
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"particle.RotationInRadians += timeSinceLastFrame * ConstantRotationRadiansPerSecond;
";
        }
    }
}