using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class StaticRotationalVelocityCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticRotationalVelocityInitializer);
        
        public FormattableString GenerateProperties(object obj)
        {
            var initializer = (StaticRotationalVelocityInitializer) obj;

            return $@"
        public int RotationSpeedInDegreesPerSecond {{ get; set; }} = {initializer.DegreesPerSecond};
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        particle.RotationalVelocityInRadians = (float) (RotationSpeedInDegreesPerSecond * Math.PI / 180f);
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}