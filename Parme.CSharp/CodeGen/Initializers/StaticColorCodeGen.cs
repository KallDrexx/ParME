using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class StaticColorCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticColorInitializer);

        public string GenerateProperties(object obj)
        {
            var initializer = (StaticColorInitializer) obj;
            
            return $@"
        public float StaticColorRedMultiplier {{ get; set; }} = {initializer.RedMultiplier}f;
        public float StaticColorGreenMultiplier {{ get; set; }} = {initializer.GreenMultiplier}f;
        public float StaticColorBlueMultiplier {{ get; set; }} = {initializer.BlueMultiplier}f;
        public float StaticColorAlphaMultiplier {{ get; set; }} = {initializer.AlphaMultiplier}f;
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
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