using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class StaticPositionCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(StaticPositionInitializer);

        public override FormattableString GenerateProperties(object obj)
        {
            var initializer = (StaticPositionInitializer) obj;
            
            return $@"
        public float StaticPositionXOffset {{ get; set; }} = {initializer.XOffset}f;
        public float StaticPositionYOffset {{ get; set; }} = {initializer.YOffset};
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        particle.Position = new Vector2(StaticPositionXOffset, StaticPositionYOffset);
";
        }

        public override FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}