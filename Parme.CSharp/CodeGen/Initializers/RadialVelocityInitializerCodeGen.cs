using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class RadialVelocityInitializerCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(RadialVelocityInitializer);

        public override FormattableString GenerateProperties(object obj)
        {
            var initializer = (RadialVelocityInitializer) obj;
            
            return $@"
        public float RadialVelocityMinMagnitude {{ get; set; }} = {initializer.MinMagnitude}f;
        public float RadialVelocityMaxMagnitude {{ get; set; }} = {initializer.MaxMagnitude}f;
        public float RadialVelocityMinRadians {{ get; set; }} = {initializer.MinDegrees * (Math.PI / 180f)}f;
        public float RadialVelocityMaxRadians {{ get; set; }} = {initializer.MaxDegrees * (Math.PI / 180f)}f;
        public float RadialVelocityXAxisScale {{ get; set; }} = {initializer.XAxisScale}f;
        public float RadialVelocityYAxisScale {{ get; set; }} = {initializer.YAxisScale}f; 
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        var radians = RadialVelocityMaxRadians - _random.NextDouble() * (RadialVelocityMaxRadians - RadialVelocityMinRadians);
                        var magnitude = RadialVelocityMaxMagnitude - _random.NextDouble() * (RadialVelocityMaxMagnitude - RadialVelocityMinMagnitude);
                
                        // convert from polar coordinates to cartesian coordinates
                        var x = magnitude * Math.Cos(radians) * RadialVelocityXAxisScale;
                        var y = magnitude * Math.Sin(radians) * RadialVelocityYAxisScale;
                        particle.Velocity = new Vector2((float) x, (float) y);
";
        }
    }
}