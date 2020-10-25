using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class RadialVelocityInitializerCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(RadialVelocityInitializer);

        public FormattableString GenerateProperties(object obj)
        {
            var initializer = (RadialVelocityInitializer) obj;
            
            return $@"
        public float RadialVelocityMinMagnitude {{ get; set; }} = {initializer.MinMagnitude}f;
        public float RadialVelocityMaxMagnitude {{ get; set; }} = {initializer.MaxMagnitude}f;
        public float RadialVelocityMinRadians {{ get; set; }} = {initializer.MinDegrees * (Math.PI / 180f)}f;
        public float RadialVelocityMaxRadians {{ get; set; }} = {initializer.MaxDegrees * (Math.PI / 180f)}f; 
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $@"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        var radians = RadialVelocityMaxRadians - _random.NextDouble() * (RadialVelocityMaxRadians - RadialVelocityMinRadians);
                        var magnitude = RadialVelocityMaxMagnitude - _random.NextDouble() * (RadialVelocityMaxMagnitude - RadialVelocityMinMagnitude);
                
                        // convert from polar coordinates to cartesian coordinates
                        var x = magnitude * Math.Cos(radians);
                        var y = magnitude * Math.Sin(radians);
                        particle.Velocity = new Vector2((float) x, (float) y);
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}