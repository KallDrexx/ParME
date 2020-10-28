using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class SingleTextureCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(SingleTextureInitializer);
        
        public FormattableString GenerateProperties(object obj)
        {
            return $"";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        particle.TextureSectionIndex = 0;
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}