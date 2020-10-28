using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class EndingSizeCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(EndingSizeModifier);
        
        public FormattableString GenerateProperties(object obj)
        {
            var modifier = (EndingSizeModifier) obj;

            return $@"
        public int EndingSizeWidth {{ get; set; }} = {modifier.Width};
        public int EndingSizeHeight {{ get; set; }} = {modifier.Height};
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        var width = (((particle.InitialSize.X - EndingSizeWidth) / MaxParticleLifeTime) * timeSinceLastFrame);
                        var height = (((particle.InitialSize.Y - EndingSizeHeight) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.Size.X -= width;
                        particle.Size.Y -= height;
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}