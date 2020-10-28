using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class AnimatingTextureCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(AnimatingTextureModifier);
        
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
                        particle.TextureSectionIndex = (byte) ((particle.TimeAlive / MaxParticleLifeTime) * 
                                                               TextureSections.Length);
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}