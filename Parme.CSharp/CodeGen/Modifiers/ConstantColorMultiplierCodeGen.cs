using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class ConstantColorMultiplierCodeGen : IGenerateCode<ConstantColorMultiplierChangeModifier>
    {
        public string GenerateProperties(ConstantColorMultiplierChangeModifier obj)
        {
            return $@"
        public float ConstantColorRedMultiplierChangePerSecond {{ get; set; }} = {obj.RedMultiplierChangePerSecond}f;
        public float ConstantColorGreenMultiplierChangePerSecond {{ get; set; }} = {obj.GreenMultiplierChangePerSecond}f;
        public float ConstantColorBlueMultiplierChangePerSecond {{ get; set; }} = {obj.BlueMultiplierChangePerSecond}f;
        public float ConstantColorAlphaMultiplierChangePerSecond {{ get; set; }} = {obj.AlphaMultiplierChangePerSecond}f;
";
        }

        public string GenerateFields(ConstantColorMultiplierChangeModifier obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(ConstantColorMultiplierChangeModifier obj)
        {
            return @"particle.RedMultiplier += timeSinceLastFrame + ConstantColorRedMultiplierChangePerSecond
            particle.GreenMultiplier += timeSinceLastFrame + ConstantColorGreenMultiplierChangePerSecond
            particle.BlueMultiplier += timeSinceLastFrame + ConstantColorBlueMultiplierChangePerSecond
            particle.AlphaMultiplier += timeSinceLastFrame + ConstantColorAlphaMultiplierChangePerSecond
";
        }
    }
}