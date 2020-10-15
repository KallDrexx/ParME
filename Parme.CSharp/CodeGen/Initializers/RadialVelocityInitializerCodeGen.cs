using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class RadialVelocityInitializerCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(RadialVelocityInitializer);

        public string GenerateProperties(object obj)
        {
            var initializer = (RadialVelocityInitializer) obj;
            
            return $@"
        public float RadialVelocityMagnitude {{ get; set; }} = {initializer.Magnitude}f;
        public float RadialVelocityMinRadians {{ get; set; }} = {initializer.MinDegrees * (Math.PI / 180f)}f;
        public float RadialVelocityMaxRadians {{ get; set; }} = {initializer.MaxDegrees * (Math.PI / 180f)}f; 
";
        }

        public string GenerateFields(object obj)
        {
            return @"";
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"
                        var radians = RadialVelocityMaxRadians - _random.NextDouble() * (RadialVelocityMaxRadians - RadialVelocityMinRadians);
                
                        // convert from polar coordinates to cartesian coordinates
                        var x = RadialVelocityMagnitude * Math.Cos(radians);
                        var y = RadialVelocityMagnitude * Math.Sin(radians);
                        particle.Velocity = new Vector2((float) x, (float) y);
";
        }

        public string GenerateCapacityEstimationCode(object obj)
        {
            return string.Empty;
        }
    }
}