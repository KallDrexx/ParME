using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class StaticPositionCodeGen : IGenerateCode<StaticPositionInitializer>
    {
        public string GenerateProperties(StaticPositionInitializer obj)
        {
            return $@"
        public float StaticPositionXOffset {{ get; set; }} = {obj.XOffset}f;
        public float StaticPositionYOffset {{ get; set; }} = {obj.YOffset};
";
        }

        public string GenerateFields(StaticPositionInitializer obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(StaticPositionInitializer obj)
        {
            return @"
            particle.Position = new Vector2(StaticPositionXOffset, StaticPositionYOffset);
";
        }
    }
}