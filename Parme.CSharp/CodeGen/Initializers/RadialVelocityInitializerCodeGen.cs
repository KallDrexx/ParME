using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RadialVelocityInitializerCodeGen : IGenerateCode<RadialVelocityInitializer>
    {
        public string GenerateProperties(RadialVelocityInitializer obj)
        {
            return $@"
        public float RadialVelocityMagnitude {{ get; set; }} = {obj.Magnitude}f;
        public float RadialVelocityMinRadians {{ get; set; }} = {obj.MinDegrees * (Math.PI / 180f)}f;
        public float RadialVelocityMaxRadians {{ get; set; }} = {obj.MaxDegrees * (Math.PI / 180f)}f; 
";
        }

        public string GenerateFields(RadialVelocityInitializer obj)
        {
            return @"";
        }

        public string GenerateExecutionCode(RadialVelocityInitializer obj)
        {
            return @"
            var radians = RadialVelocityMagnitude - _random.NextDouble() * (RadialVelocityMaxRadians - RadialVelocityMinRadians);
                
            // convert from polar coordinates to cartesian coordinates
            var x = RadialVelocityMagnitude * Math.Cos(radians);
            var y = RadialVelocityMagnitude * Math.Sin(radians);
            particle.Velocity = new Particle((float) x, (float) y);
";
        }
    }
}