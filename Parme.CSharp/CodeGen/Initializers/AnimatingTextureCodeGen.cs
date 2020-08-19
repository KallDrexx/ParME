using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class AnimatingTextureCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(AnimatingTextureModifier);
        
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
                        particle.TextureSectionIndex = (byte) ((particle.TimeAlive / MaxParticleLifeTime) * 
                                                               (TextureSections.Length / MaxParticleLifeTime));
";
        }
    }
}