using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class StaticSizeCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticSizeInitializer);

        public FormattableString GenerateProperties(object obj)
        {
            var initializer = (StaticSizeInitializer) obj;
            
            return $@"
        public int StaticSizeWidth {{ get; set; }} = {initializer.Width};
        public int StaticSizeHeight {{ get; set; }} = {initializer.Height};
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"particle.Size = new Vector2(StaticSizeWidth, StaticSizeHeight);
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}