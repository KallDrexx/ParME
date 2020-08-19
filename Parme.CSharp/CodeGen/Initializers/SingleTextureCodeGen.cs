using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class SingleTextureCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(SingleTextureInitializer);
        
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
                        particle.TextureSectionIndex = 0;
";
        }
    }
}