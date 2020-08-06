using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomRegionPositionCodeGen : IGenerateCode<RandomRegionPositionInitializer>
    {
        public string GenerateProperties(RandomRegionPositionInitializer obj)
        {
            return $@"
        public float RandomRegionPositionMinXOffset {{ get; set; }} = {obj.MinXOffset}f;
        public float RandomRegionPositionMaxXOffset {{ get; set; }} = {obj.MaxXOffset}f;
        public float RandomRegionPositionMinYOffset {{ get; set; }} = {obj.MinYOffset}f;
        public float RandomRegionPositionMaxYOffset {{ get; set; }} = {obj.MaxYOffset}f;
";
        }

        public string GenerateFields(RandomRegionPositionInitializer obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(RandomRegionPositionInitializer obj)
        {
            return $@"
            var x = RandomRegionPositionMaxXOffset - _random.NextDouble() * (RandomRegionPositionMaxXOffset - RandomRegionPositionMinXOffset);
            var y = RandomRegionPositionMaxYOffset - _random.NextDouble() * (RandomRegionPositionMaxYOffset - RandomRegionPositionMinYOffset);
            particle.Position = new Vector2((float) x, (float) y);
";
        }
    }
}