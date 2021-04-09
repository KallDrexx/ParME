using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class SingleTextureCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(SingleTextureInitializer);

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        particle.TextureSectionIndex = 0;
";
        }
    }
}