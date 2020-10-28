using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomRotationalVelocityCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(RandomRotationalVelocityInitializer);
        
        public FormattableString GenerateProperties(object obj)
        {
            var initializer = (RandomRotationalVelocityInitializer) obj;

            return $@"
        public int RandomRotationalSpeedMinDegrees {{ get; set; }} = {initializer.MinRotationSpeedInDegreesPerSecond};
        public int RandomRotationalSpeedMaxDegrees {{ get; set; }} = {initializer.MaxRotationSpeedInDegreesPerSecond}; 
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        var speed = RandomRotationalSpeedMaxDegrees - _random.NextDouble() * (RandomRotationalSpeedMaxDegrees - RandomRotationalSpeedMinDegrees);
                        particle.RotationalVelocityInRadians = (float) (speed * Math.PI / 180f);
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}