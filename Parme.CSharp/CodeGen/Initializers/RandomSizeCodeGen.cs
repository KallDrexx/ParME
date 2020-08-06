using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomSizeCodeGen : IGenerateCode<RandomSizeInitializer>
    {
        public string GenerateProperties(RandomSizeInitializer obj)
        {
            return $@"
        public int RandomSizeMinWidth {{ get; set; }} = {obj.MinWidth};
        public int RandomSizeMaxWidth {{ get; set; }} = {obj.MaxWidth};
        public int RandomSizeMinHeight {{ get; set; }} = {obj.MinHeight};
        public int RandomSizeMaxHeight {{ get; set; }} = {obj.MaxHeight};
";
        }

        public string GenerateFields(RandomSizeInitializer obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(RandomSizeInitializer obj)
        {
            return $@"
            var x = RandomSizeMaxWidth - _random.NextDouble() * (RandomSizeMaxWidth - RandomSizeMinWidth);
            var y = RandomSizeMaxHeight - _random.NextDouble() * (RandomSizeMaxHeight - RandomSizeMinHeight);
            particle.Size = new Vector2((float) x, (float) y);
";
        }
    }
}