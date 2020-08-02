using Microsoft.Xna.Framework;

namespace Parme.Initializers.SizeInitializers
{
    public class StaticSizeInitializer : ISizeInitializer
    {
        private readonly Vector2 _size;

        public StaticSizeInitializer(Vector2 size)
        {
            _size = size;
        }

        public Vector2 GetNextParticleSize()
        {
            return _size;
        }
    }
}