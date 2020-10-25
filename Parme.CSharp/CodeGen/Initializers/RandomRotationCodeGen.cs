using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomRotationCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(RandomRotationInitializer);
        
        public FormattableString GenerateProperties(object obj)
        {
            var initializer = (RandomRotationInitializer) obj;

            return $@"
        public int RandomRotationMinDegrees {{ get; set; }} = {initializer.MinDegrees};
        public int RandomRotationMaxDegrees {{ get; set; }} = {initializer.MaxDegrees}; 
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"var degrees = RandomRotationMaxDegrees - _random.NextDouble() * (RandomRotationMaxDegrees - RandomRotationMinDegrees);
                        particle.RotationInRadians = (float) (degrees * (Math.PI / 180f)); 
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}