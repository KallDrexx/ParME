using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class ConstantAccelerationCodeGen : IGenerateCode<ConstantAccelerationModifier>
    {
        public string GenerateProperties(ConstantAccelerationModifier obj)
        {
            return $@"
        public float ConstantAccelerationX {{ get; set; }} = {obj.XAcceleration}f;
        public float ConstantAccelerationY {{ get; set; }} = {obj.YAcceleration}f;
";
        }

        public string GenerateFields(ConstantAccelerationModifier obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(ConstantAccelerationModifier obj)
        {
            return @"particle.Velocity += timeSinceLastFrame * new Vector2(ConstantAccelerationX, ConstantAccelerationY);
";
        }
    }
}