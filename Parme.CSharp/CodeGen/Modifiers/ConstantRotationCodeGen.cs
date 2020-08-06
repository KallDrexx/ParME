using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class ConstantRotationCodeGen : IGenerateCode<ConstantRotationModifier>
    {
        public string GenerateProperties(ConstantRotationModifier obj)
        {
            return $@"
        public float ConstantRotationRadiansPerSecond {{ get; set; }} = {obj.DegreesPerSecond * (Math.PI / 180f)}f;
";
        }

        public string GenerateFields(ConstantRotationModifier obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(ConstantRotationModifier obj)
        {
            return @"particle.RotationInRadians += timeSinceLastFrame * ConstantRotationRadiansPerSecond
";
        }
    }
}