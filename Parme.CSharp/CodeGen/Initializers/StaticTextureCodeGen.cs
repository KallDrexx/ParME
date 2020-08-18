using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class StaticTextureCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticTextureInitializer);
        
        public string GenerateProperties(object obj)
        {
            var initializer = (StaticTextureInitializer) obj;
            
            return $@"
        public byte StaticTextureSectionIndex {{ get; set; }} = {initializer.TextureSectionIndex}; 
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"
                        particle.TextureSectionIndex = StaticTextureSectionIndex;
";
        }
    }
}