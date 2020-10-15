using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class StaticRotationalVelocityCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticRotationalVelocityInitializer);
        
        public string GenerateProperties(object obj)
        {
            var initializer = (StaticRotationalVelocityInitializer) obj;

            return $@"
        public int RotationSpeedInDegreesPerSecond {{ get; set; }} = {initializer.DegreesPerSecond};
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"
                        particle.RotationalVelocityInRadians = (float) (RotationSpeedInDegreesPerSecond * Math.PI / 180f);
";
        }

        public string GenerateCapacityEstimationCode(object obj)
        {
            return string.Empty;
        }
    }
}