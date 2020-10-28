using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class RandomRangeVelocityCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(RandomRangeVelocityInitializer);

        public FormattableString GenerateProperties(object obj)
        {
            var initializer = (RandomRangeVelocityInitializer) obj;
            
            return $@"
        public float RandomRangeVelocityMinX {{ get; set; }} = {initializer.MinXVelocity}f;
        public float RandomRangeVelocityMaxX {{ get; set; }} = {initializer.MaxXVelocity}f;
        public float RandomRangeVelocityMinY {{ get; set; }} = {initializer.MinYVelocity}f;
        public float RandomRangeVelocityMaxY {{ get; set; }} = {initializer.MaxYVelocity}f; 
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        var x = RandomRangeVelocityMaxX - _random.NextDouble() * (RandomRangeVelocityMaxX - RandomRangeVelocityMinX);
                        var y = RandomRangeVelocityMaxY - _random.NextDouble() * (RandomRangeVelocityMaxY - RandomRangeVelocityMinY);
                        particle.Velocity = new Vector2((float) x, (float) y);
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}