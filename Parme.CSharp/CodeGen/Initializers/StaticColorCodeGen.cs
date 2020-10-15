using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class StaticColorCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticColorInitializer);

        public string GenerateProperties(object obj)
        {
            var initializer = (StaticColorInitializer) obj;
            
            return $@"
        public byte StaticColorStartingRed {{ get; set; }} = {initializer.Red};
        public byte StaticColorStartingGreen {{ get; set; }} = {initializer.Green};
        public byte StaticColorStartingBlue {{ get; set; }} = {initializer.Blue};
        public byte StaticColorStartingAlpha {{ get; set; }} = {(byte) (initializer.Alpha * 255)};
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"
                        particle.InitialRed = StaticColorStartingRed;
                        particle.CurrentRed = StaticColorStartingRed;
                        particle.InitialGreen = StaticColorStartingGreen;
                        particle.CurrentGreen = StaticColorStartingGreen;
                        particle.InitialBlue = StaticColorStartingBlue;
                        particle.CurrentBlue = StaticColorStartingBlue;
                        particle.InitialAlpha = StaticColorStartingAlpha;
                        particle.CurrentAlpha = StaticColorStartingAlpha;
            ";
        }

        public string GenerateCapacityEstimationCode(object obj)
        {
            return string.Empty;
        }
    }
}