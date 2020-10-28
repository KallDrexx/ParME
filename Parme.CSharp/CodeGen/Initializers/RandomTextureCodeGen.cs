using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomTextureCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(RandomTextureInitializer);
        
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
                        particle.TextureSectionIndex = (byte) _random.Next(0, TextureSections.Length);
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}