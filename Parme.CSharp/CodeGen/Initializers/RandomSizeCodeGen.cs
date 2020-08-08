using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomSizeCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(RandomSizeInitializer);

        public string GenerateProperties(object obj)
        {
            var initializer = (RandomSizeInitializer) obj;
            
            return $@"
        public int RandomSizeMinWidth {{ get; set; }} = {initializer.MinWidth};
        public int RandomSizeMaxWidth {{ get; set; }} = {initializer.MaxWidth};
        public int RandomSizeMinHeight {{ get; set; }} = {initializer.MinHeight};
        public int RandomSizeMaxHeight {{ get; set; }} = {initializer.MaxHeight};
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return $@"
            var x = RandomSizeMaxWidth - _random.NextDouble() * (RandomSizeMaxWidth - RandomSizeMinWidth);
            var y = RandomSizeMaxHeight - _random.NextDouble() * (RandomSizeMaxHeight - RandomSizeMinHeight);
            particle.Size = new Vector2((float) x, (float) y);
";
        }
    }
}