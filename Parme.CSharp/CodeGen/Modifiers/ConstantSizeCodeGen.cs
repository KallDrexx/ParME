using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class ConstantSizeCodeGen : IGenerateCode<ConstantSizeModifier>
    {
        public string GenerateProperties(ConstantSizeModifier obj)
        {
            return $@"
        public float ConstantSizeWidthChangePerSecond {{ get; set; }} = {obj.WidthChangePerSecond}f;
        public float ConstantSizeHeightChangePerSecond {{ get; set; }} = {obj.HeightChangePerSecond}f;
";
        }

        public string GenerateFields(ConstantSizeModifier obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(ConstantSizeModifier obj)
        {
            return @"particle.Size += timeSinceLastFrame * new Vector2(ConstantSizeWidthChangePerSecond, ConstantSizeHeightChangePerSecond
";
        }
    }
}