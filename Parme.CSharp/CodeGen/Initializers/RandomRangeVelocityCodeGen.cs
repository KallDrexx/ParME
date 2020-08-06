using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomRangeVelocityCodeGen : IGenerateCode<RandomRangeVelocityInitializer>
    {
        public string GenerateProperties(RandomRangeVelocityInitializer obj)
        {
            return $@"public float RandomRangeVelocityMinX {{ get; set; }} = {obj.MinXVelocity}f;
        public float RandomRangeVelocityMaxX {{ get; set; }} = {obj.MaxXVelocity}f;
        public float RandomRangeVelocityMinY {{ get; set; }} = {obj.MinYVelocity}f;
        public float RandomRangeVelocityMaxY {{ get; set; }} = {obj.MaxYVelocity}f; 
";
        }

        public string GenerateFields(RandomRangeVelocityInitializer obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(RandomRangeVelocityInitializer obj)
        {
            return @"
            var x = RandomRangeVelocityMaxX - _random.NextDouble() * (RandomRangeVelocityMaxX - RandomRangeVelocityMinX);
            var y = RandomRangeVelocityMaxY - _random.NextDouble() * (RandomRangeVelocityMaxY - RandomRangeVelocityMinY);
            particle.Velocity = new Vector2((float) x, (float) y);
";
        }
    }
}