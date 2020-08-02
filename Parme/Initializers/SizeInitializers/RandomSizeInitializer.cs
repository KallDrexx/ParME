using System;
using Microsoft.Xna.Framework;

namespace Parme.Initializers.SizeInitializers
{
    public class RandomSizeInitializer : ISizeInitializer
    {
        private readonly Random _random = new Random();
        private readonly Vector2 _minSize, _maxSize;

        public RandomSizeInitializer(Vector2 minSize, Vector2 maxSize)
        {
            if (minSize.X > maxSize.X || minSize.Y > maxSize.Y)
            {
                var message = $"Minimum size ({minSize.X}, {minSize.Y}) had values greater than " +
                              $"the maximum size ({maxSize.X}, {maxSize.Y})";
                throw new ArgumentException(message);
            }

            _minSize = minSize;
            _maxSize = maxSize;
        }

        public Vector2 GetNextParticleSize()
        {
            var x = _maxSize.X - _random.NextDouble() * (_maxSize.X - _minSize.X);
            var y = _maxSize.Y - _random.NextDouble() * (_maxSize.Y - _minSize.Y);
            
            return new Vector2((float) x, (float) y);
        }
    }
}