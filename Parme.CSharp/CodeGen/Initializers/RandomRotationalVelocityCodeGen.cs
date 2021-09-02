using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomRotationalVelocityCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(RandomRotationalVelocityInitializer);
        
        public override FormattableString GenerateProperties(object obj)
        {
            var initializer = (RandomRotationalVelocityInitializer) obj;

            return $@"
        public int RandomRotationalSpeedMinDegrees {{ get; set; }} = {initializer.MinRotationSpeedInDegreesPerSecond};
        public int RandomRotationalSpeedMaxDegrees {{ get; set; }} = {initializer.MaxRotationSpeedInDegreesPerSecond}; 
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        var speed = RandomRotationalSpeedMaxDegrees - parent.Random.NextDouble() * (RandomRotationalSpeedMaxDegrees - RandomRotationalSpeedMinDegrees);
                        particle.RotationalVelocityInRadians = (float) (speed * Math.PI / 180f);
";
        }
    }
}