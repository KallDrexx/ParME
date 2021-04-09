using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class StaticRotationalVelocityCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(StaticRotationalVelocityInitializer);
        
        public override FormattableString GenerateProperties(object obj)
        {
            var initializer = (StaticRotationalVelocityInitializer) obj;

            return $@"
        public int RotationSpeedInDegreesPerSecond {{ get; set; }} = {initializer.DegreesPerSecond};
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        particle.RotationalVelocityInRadians = (float) (RotationSpeedInDegreesPerSecond * Math.PI / 180f);
";
        }
    }
}