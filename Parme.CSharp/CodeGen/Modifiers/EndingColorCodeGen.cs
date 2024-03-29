﻿using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class EndingColorCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(EndingColorModifier);
        
        public override FormattableString GenerateProperties(object obj)
        {
            var modifier = (EndingColorModifier) obj;
            
            return $@"
        public byte EndingColorRed {{ get; set; }} = {modifier.Red};
        public byte EndingColorGreen {{ get; set; }} = {modifier.Green};
        public byte EndingColorBlue {{ get; set; }} = {modifier.Blue};
        public byte EndingColorAlpha {{ get; set; }} = {(byte) (modifier.Alpha * 255)};
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"particle.CurrentRed -= (((particle.InitialRed - EndingColorRed) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentGreen -= (((particle.InitialGreen - EndingColorGreen) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentBlue -= (((particle.InitialBlue - EndingColorBlue) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentAlpha -= (((particle.InitialAlpha - EndingColorAlpha) / MaxParticleLifeTime) * timeSinceLastFrame);
";
        }
    }
}