using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomTextureCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(RandomTextureInitializer);

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        particle.TextureSectionIndex = (byte) parent.Random.Next(0, TextureSections.Length);
";
        }
    }
}