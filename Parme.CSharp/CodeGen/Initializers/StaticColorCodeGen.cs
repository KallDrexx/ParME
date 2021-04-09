using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class StaticColorCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(StaticColorInitializer);

        public override FormattableString GenerateProperties(object obj)
        {
            var initializer = (StaticColorInitializer) obj;
            
            return $@"
        public byte StaticColorStartingRed {{ get; set; }} = {initializer.Red};
        public byte StaticColorStartingGreen {{ get; set; }} = {initializer.Green};
        public byte StaticColorStartingBlue {{ get; set; }} = {initializer.Blue};
        public byte StaticColorStartingAlpha {{ get; set; }} = {(byte) (initializer.Alpha * 255)};
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        particle.CurrentRed = (float) StaticColorStartingRed;
                        particle.CurrentGreen = (float) StaticColorStartingGreen;
                        particle.CurrentBlue = (float) StaticColorStartingBlue;
                        particle.CurrentAlpha = (float) StaticColorStartingAlpha;
            ";
        }
    }
}