using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class StaticPositionCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticPositionInitializer);

        public string GenerateProperties(object obj)
        {
            var initializer = (StaticPositionInitializer) obj;
            
            return $@"
        public float StaticPositionXOffset {{ get; set; }} = {initializer.XOffset}f;
        public float StaticPositionYOffset {{ get; set; }} = {initializer.YOffset};
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"
                        particle.Position = new Vector2(StaticPositionXOffset, StaticPositionYOffset);
";
        }
    }
}