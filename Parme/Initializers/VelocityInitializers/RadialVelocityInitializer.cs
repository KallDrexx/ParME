using System;
using Microsoft.Xna.Framework;

namespace Parme.Initializers.VelocityInitializers
{
    public class RadialVelocityInitializer : IVelocityInitializer
    {
        private readonly Random _random = new Random();
        private readonly float _magnitude, _minRadians, _maxRadians;

        public RadialVelocityInitializer(float magnitude, float minDegrees, float maxDegrees)
        {
            _magnitude = magnitude;

            if (maxDegrees < minDegrees)
            {
                // This means we are crossing the 0 degree boundary, so we want to adjust to make
                // sure max is always greater than min
                maxDegrees += 360;
            }
            
            _minRadians = (float)(minDegrees * (Math.PI / 180f));
            _maxRadians = (float) (maxDegrees * (Math.PI / 180f));
        }
        
        public Vector2 GetNewParticleVelocity()
        {
            var radians = _maxRadians - _random.NextDouble() * (_maxRadians - _minRadians);
            
            // convert from polar coordinates to cartesian coordinates
            var x = _magnitude * Math.Cos(radians);
            var y = _magnitude * Math.Sin(radians);
            
            return new Vector2((float) x, (float) y);
        }
    }
}