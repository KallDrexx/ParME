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

        public string GetCSharpExecutionCode()
        {
            return $@"
            particle.ColorModifier = new Color({_color.R}, {_color.G}, {_color.B}, {_color.A});
";
        }
    }
}