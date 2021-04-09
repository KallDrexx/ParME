using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class AnimatingTextureCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(AnimatingTextureModifier);

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        particle.TextureSectionIndex = (byte) ((particle.TimeAlive / MaxParticleLifeTime) * 
                                                               TextureSections.Length);
";
        }
    }
}