using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class StaticColorCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticColorInitializer);

        public FormattableString GenerateProperties(object obj)
        {
            var initializer = (StaticColorInitializer) obj;
            
            return $@"
        public byte StaticColorStartingRed {{ get; set; }} = {initializer.Red};
        public byte StaticColorStartingGreen {{ get; set; }} = {initializer.Green};
        public byte StaticColorStartingBlue {{ get; set; }} = {initializer.Blue};
        public byte StaticColorStartingAlpha {{ get; set; }} = {(byte) (initializer.Alpha * 255)};
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        particle.CurrentRed = StaticColorStartingRed;
                        particle.CurrentGreen = StaticColorStartingGreen;
                        particle.CurrentBlue = StaticColorStartingBlue;
                        particle.CurrentAlpha = StaticColorStartingAlpha;
            ";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}