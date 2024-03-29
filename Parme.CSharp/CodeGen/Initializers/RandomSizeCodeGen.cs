using System;
using System.Text;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class RandomSizeCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(RandomSizeInitializer);

        public override FormattableString GenerateProperties(object obj)
        {
            var initializer = (RandomSizeInitializer) obj;

            if (initializer.PreserveAspectRatio)
            {
                return $@"
        public int RandomSizeMin {{ get; set; }} = {(initializer.RandomizedAxis == RandomSizeInitializer.Axis.Y ? initializer.MinHeight : initializer.MinWidth)};
        public int RandomSizeMax {{ get; set; }} = {(initializer.RandomizedAxis == RandomSizeInitializer.Axis.Y ? initializer.MaxHeight : initializer.MaxWidth)};
";
            }

            return $@"
        public int RandomSizeMinWidth {{ get; set; }} = {initializer.MinWidth};
        public int RandomSizeMaxWidth {{ get; set; }} = {initializer.MaxWidth};
        public int RandomSizeMinHeight {{ get; set; }} = {initializer.MinHeight};
        public int RandomSizeMaxHeight {{ get; set; }} = {initializer.MaxHeight};
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            var initializer = (RandomSizeInitializer) obj;

            if (initializer.PreserveAspectRatio)
            {
                var result = new StringBuilder();
                result.Append(@"
                        float aspectRatio;
                        if (TextureSections.Length == 0)
                        {
                            aspectRatio = parent.FullTextureSize.X / parent.FullTextureSize.Y;
                        }
                        else
                        {
                            var textureWidth = Math.Abs(TextureSections[particle.TextureSectionIndex].RightX - TextureSections[particle.TextureSectionIndex].LeftX);
                            var textureHeight = Math.Abs(TextureSections[particle.TextureSectionIndex].BottomY - TextureSections[particle.TextureSectionIndex].TopY);
                            aspectRatio = (float) textureWidth / textureHeight;
                        }
");

                if (initializer.RandomizedAxis == RandomSizeInitializer.Axis.Y)
                {
                    result.Append(@"
                        var height = RandomSizeMax - parent.Random.NextDouble() * (RandomSizeMax - RandomSizeMin);
                        var width = height * aspectRatio;");
                }
                else
                {
                    result.Append(@"
                        var width = RandomSizeMax - parent.Random.NextDouble() * (RandomSizeMax - RandomSizeMin);
                        var height = width / aspectRatio;");
                }

                result.AppendLine("                        particle.Size = new Vector2((float) width, (float) height);");

                return $"{result}";
            }
            
            return $@"
                        var x = RandomSizeMaxWidth - parent.Random.NextDouble() * (RandomSizeMaxWidth - RandomSizeMinWidth);
                        var y = RandomSizeMaxHeight - parent.Random.NextDouble() * (RandomSizeMaxHeight - RandomSizeMinHeight);
                        particle.Size = new Vector2((float) x, (float) y);
";
        }
    }
}