using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class ConstantColorMultiplierCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(ConstantColorMultiplierChangeModifier);

        public string GenerateProperties(object obj)
        {
            var modifier = (ConstantColorMultiplierChangeModifier) obj;
            
            return $@"
        public float ConstantColorRedMultiplierChangePerSecond {{ get; set; }} = {modifier.RedMultiplierChangePerSecond}f;
        public float ConstantColorGreenMultiplierChangePerSecond {{ get; set; }} = {modifier.GreenMultiplierChangePerSecond}f;
        public float ConstantColorBlueMultiplierChangePerSecond {{ get; set; }} = {modifier.BlueMultiplierChangePerSecond}f;
        public float ConstantColorAlphaMultiplierChangePerSecond {{ get; set; }} = {modifier.AlphaMultiplierChangePerSecond}f;
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"particle.RedMultiplier += timeSinceLastFrame + ConstantColorRedMultiplierChangePerSecond;
                        particle.GreenMultiplier += timeSinceLastFrame + ConstantColorGreenMultiplierChangePerSecond;
                        particle.BlueMultiplier += timeSinceLastFrame + ConstantColorBlueMultiplierChangePerSecond;
                        particle.AlphaMultiplier += timeSinceLastFrame + ConstantColorAlphaMultiplierChangePerSecond;
";
        }
    }
}