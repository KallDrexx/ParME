using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class RandomRangeVelocityCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(RandomRangeVelocityInitializer);

        public override FormattableString GenerateProperties(object obj)
        {
            var initializer = (RandomRangeVelocityInitializer) obj;
            
            return $@"
        public float RandomRangeVelocityMinX {{ get; set; }} = {initializer.MinXVelocity}f;
        public float RandomRangeVelocityMaxX {{ get; set; }} = {initializer.MaxXVelocity}f;
        public float RandomRangeVelocityMinY {{ get; set; }} = {initializer.MinYVelocity}f;
        public float RandomRangeVelocityMaxY {{ get; set; }} = {initializer.MaxYVelocity}f; 
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        var x = RandomRangeVelocityMaxX - parent.Random.NextDouble() * (RandomRangeVelocityMaxX - RandomRangeVelocityMinX);
                        var y = RandomRangeVelocityMaxY - parent.Random.NextDouble() * (RandomRangeVelocityMaxY - RandomRangeVelocityMinY);
                        particle.Velocity = new Vector2((float) x, (float) y);
";
        }
    }
}