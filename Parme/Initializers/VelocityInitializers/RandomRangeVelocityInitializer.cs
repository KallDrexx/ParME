using System;
using Microsoft.Xna.Framework;

namespace Parme.Initializers.VelocityInitializers
{
    public class RandomRangeVelocityInitializer : IVelocityInitializer
    {
        private readonly Random _random = new Random();
        private readonly Vector2 _min, _max;

        public RandomRangeVelocityInitializer(Vector2 min, Vector2 max)
        {
            if (min.X > max.X || min.Y > max.Y)
            {
                var message = $"Minimum bounds ({min.X}, {min.Y}) had values greater than " +
                              $"the maximum bounds ({max.X}, {max.Y})";
                throw new ArgumentException(message);
            }

            _min = min;
            _max = max;
        }
        
        public Vector2 GetNewParticleVelocity()
        {
            var x = _max.X - _random.NextDouble() * (_max.X - _min.X);
            var y = _max.Y - _random.NextDouble() * (_max.Y - _min.Y);
            return new Vector2((float) x, (float) y);
        }
    }
}