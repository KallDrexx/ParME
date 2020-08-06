using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class StaticSizeCodeGen : IGenerateCode<StaticSizeInitializer>
    {
        public string GenerateProperties(StaticSizeInitializer obj)
        {
            return $@"
        public int StaticSizeWidth {{ get; set; }} = {obj.Width};
        public int StaticSizeHeight {{ get; set; }} = {obj.Height};
";
        }

        public string GenerateFields(StaticSizeInitializer obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(StaticSizeInitializer obj)
        {
            return @"particle.Size = new Vector2(StaticSizeWidth, StaticSizeHeight);
";
        }
    }
}