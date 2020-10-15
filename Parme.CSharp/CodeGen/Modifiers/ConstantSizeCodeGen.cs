using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    internal class ConstantSizeCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(ConstantSizeModifier);

        public string GenerateProperties(object obj)
        {
            var modifier = (ConstantSizeModifier) obj;
            
            return $@"
        public float ConstantSizeWidthChangePerSecond {{ get; set; }} = {modifier.WidthChangePerSecond}f;
        public float ConstantSizeHeightChangePerSecond {{ get; set; }} = {modifier.HeightChangePerSecond}f;
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"particle.Size += timeSinceLastFrame * new Vector2(ConstantSizeWidthChangePerSecond, ConstantSizeHeightChangePerSecond);
";
        }

        public string GenerateCapacityEstimationCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}