using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class EndingSizeCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(EndingSizeModifier);
        
        public override FormattableString GenerateProperties(object obj)
        {
            var modifier = (EndingSizeModifier) obj;

            return $@"
        public int EndingSizeWidth {{ get; set; }} = {modifier.Width};
        public int EndingSizeHeight {{ get; set; }} = {modifier.Height};
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        var width = (((particle.InitialSize.X - EndingSizeWidth) / MaxParticleLifeTime) * timeSinceLastFrame);
                        var height = (((particle.InitialSize.Y - EndingSizeHeight) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.Size.X -= width;
                        particle.Size.Y -= height;
";
        }
    }
}