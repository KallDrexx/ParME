using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class EndingColorCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(EndingColorModifier);
        
        public FormattableString GenerateProperties(object obj)
        {
            var modifier = (EndingColorModifier) obj;
            
            return $@"
        public byte EndingColorRed {{ get; set; }} = {modifier.Red};
        public byte EndingColorGreen {{ get; set; }} = {modifier.Green};
        public byte EndingColorBlue {{ get; set; }} = {modifier.Blue};
        public byte EndingColorAlpha {{ get; set; }} = {(byte) modifier.Alpha};
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"particle.CurrentRed -= (byte) (((particle.InitialRed - EndingColorRed) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentGreen -= (byte) (((particle.InitialGreen - EndingColorGreen) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentBlue -= (byte) (((particle.InitialBlue - EndingColorBlue) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentAlpha -= (byte) (((particle.InitialAlpha - EndingColorAlpha) / MaxParticleLifeTime) * timeSinceLastFrame);
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}