using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomTextureCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(RandomTextureInitializer);
        
        public string GenerateProperties(object obj)
        {
            return string.Empty;
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"
                        particle.TextureSectionIndex = (byte) _random.Next(0, TextureSections.Length);
";
        }
    }
}