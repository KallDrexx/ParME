using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class StaticColorCodeGen : IGenerateCode<StaticColorInitializer>
    {
        public string GenerateProperties(StaticColorInitializer obj)
        {
            return $@"
        public float StaticColorRedMultiplier {{ get; set; }} = {obj.RedMultiplier}f;
        public float StaticColorGreenMultiplier {{ get; set; }} = {obj.GreenMultiplier}f;
        public float StaticColorBlueMultiplier {{ get; set; }} = {obj.BlueMultiplier}f;
        public float StaticColorAlphaMultiplier {{ get; set; }} = {obj.AlphaMultiplier}f;
";
        }

        public string GenerateFields(StaticColorInitializer obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(StaticColorInitializer obj)
        {
            return @"
            particle.RedMultiplier = StaticColorRedMultiplier;
            particle.GreenMultiplier = StaticColorGreenMultiplier;
            particle.BlueMultiplier = StaticColorBlueMultiplier;
            particle.AlphaMultiplier = StaticColorAlphaMultiplier;
            ";
        }
    }
}