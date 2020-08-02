using System;
using Microsoft.Xna.Framework;

namespace Parme.Initializers.PositionalInitializers
{
    public class RandomRegionPositionInitializer : IPositionalInitializer
    {
        private readonly Random _random = new Random();
        private readonly Vector2 _minBounds, _maxBounds;

        public RandomRegionPositionInitializer(Vector2 minBounds, Vector2 maxBounds)
        {
            _minBounds = minBounds;
            _maxBounds = maxBounds;

            if (minBounds.X > maxBounds.X || minBounds.Y > maxBounds.Y)
            {
                var message = $"Minimum bounds ({minBounds.X}, {minBounds.Y}) had values greater than " +
                              $"the maximum bounds ({maxBounds.X}, {maxBounds.Y})";
                throw new ArgumentException(message);
            }
        }

        public Vector2 GetNewParticlePosition()
        {
            var x = _maxBounds.X - _random.NextDouble() * (_maxBounds.X - _minBounds.X);
            var y = _maxBounds.Y - _random.NextDouble() * (_maxBounds.Y - _minBounds.Y);
            return new Vector2((float) x, (float) y);
        }
    }
}