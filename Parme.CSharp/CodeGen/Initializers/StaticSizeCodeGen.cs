using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class StaticSizeCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticSizeInitializer);

        public string GenerateProperties(object obj)
        {
            var initializer = (StaticSizeInitializer) obj;
            
            return $@"
        public int StaticSizeWidth {{ get; set; }} = {initializer.Width};
        public int StaticSizeHeight {{ get; set; }} = {initializer.Height};
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"particle.Size = new Vector2(StaticSizeWidth, StaticSizeHeight);
";
        }
    }
}