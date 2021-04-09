using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class TextureBasedSizeCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(TextureBasedSizeInitializer);
        
        public override FormattableString GenerateProperties(object obj)
        {
            var initializer = (TextureBasedSizeInitializer) obj;
            
            return $@"
        public int TextureBasedSizePercentage {{ get; set; }} = {initializer.Percentage};
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"if (TextureSections.Length > 0)
                        {{
                            var width = Math.Abs(TextureSections[particle.TextureSectionIndex].RightX - TextureSections[particle.TextureSectionIndex].LeftX)
                                        * (TextureBasedSizePercentage / 100f);

                            var height = Math.Abs(TextureSections[particle.TextureSectionIndex].BottomY - TextureSections[particle.TextureSectionIndex].TopY)
                                        * (TextureBasedSizePercentage / 100f);

                            particle.Size = new Vector2(width, height);
                        }}
";
        }
    }
}