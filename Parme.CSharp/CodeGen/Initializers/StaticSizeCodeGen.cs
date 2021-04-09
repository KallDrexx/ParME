using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class StaticSizeCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(StaticSizeInitializer);

        public override FormattableString GenerateProperties(object obj)
        {
            var initializer = (StaticSizeInitializer) obj;
            
            return $@"
        public int StaticSizeWidth {{ get; set; }} = {initializer.Width};
        public int StaticSizeHeight {{ get; set; }} = {initializer.Height};
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"particle.Size = new Vector2(StaticSizeWidth, StaticSizeHeight);
";
        }
    }
}