using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomRotationCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(RandomRotationInitializer);
        
        public override FormattableString GenerateProperties(object obj)
        {
            var initializer = (RandomRotationInitializer) obj;

            return $@"
        public int RandomRotationMinDegrees {{ get; set; }} = {initializer.MinDegrees};
        public int RandomRotationMaxDegrees {{ get; set; }} = {initializer.MaxDegrees}; 
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"var degrees = RandomRotationMaxDegrees - parent.Random.NextDouble() * (RandomRotationMaxDegrees - RandomRotationMinDegrees);
                        particle.RotationInRadians = (float) (degrees * (Math.PI / 180f)); 
";
        }

        public override FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}