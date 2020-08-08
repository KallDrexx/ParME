using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomRegionPositionCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(RandomRegionPositionInitializer);

        public string GenerateProperties(object obj)
        {
            var initializer = (RandomRegionPositionInitializer) obj;
            
            return $@"
        public float RandomRegionPositionMinXOffset {{ get; set; }} = {initializer.MinXOffset}f;
        public float RandomRegionPositionMaxXOffset {{ get; set; }} = {initializer.MaxXOffset}f;
        public float RandomRegionPositionMinYOffset {{ get; set; }} = {initializer.MinYOffset}f;
        public float RandomRegionPositionMaxYOffset {{ get; set; }} = {initializer.MaxYOffset}f;
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return $@"
                        var x = RandomRegionPositionMaxXOffset - _random.NextDouble() * (RandomRegionPositionMaxXOffset - RandomRegionPositionMinXOffset);
                        var y = RandomRegionPositionMaxYOffset - _random.NextDouble() * (RandomRegionPositionMaxYOffset - RandomRegionPositionMinYOffset);
                        particle.Position = new Vector2((float) x, (float) y);
";
        }
    }
}