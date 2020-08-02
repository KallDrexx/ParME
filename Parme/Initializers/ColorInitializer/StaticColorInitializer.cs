using Microsoft.Xna.Framework;

namespace Parme.Initializers.ColorInitializer
{
    public class StaticColorInitializer : IColorInitializer
    {
        private readonly Color _color;

        public StaticColorInitializer(Color color)
        {
            _color = color;
        }

        public Color GetColorOperationForNextParticle()
        {
            return _color;
        }
    }
}