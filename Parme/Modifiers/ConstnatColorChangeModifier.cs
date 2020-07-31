using Microsoft.Xna.Framework;

namespace Parme.Modifiers
{
    public class ConstnatColorChangeModifier : IParticleModifier
    {
        private readonly float _redChangePerSecond, _blueChangePerSecond, _greenChangePerSecond;

        public ConstnatColorChangeModifier(float redChangePerSecond, float blueChangePerSecond, float greenChangePerSecond)
        {
            _redChangePerSecond = redChangePerSecond;
            _blueChangePerSecond = blueChangePerSecond;
            _greenChangePerSecond = greenChangePerSecond;
        }

        public void Update(float timeSinceLastFrame, Particle particle)
        {
            particle.Color = new Color(particle.Color.R + (int)(_redChangePerSecond * timeSinceLastFrame),
                particle.Color.G + (int)(_greenChangePerSecond * timeSinceLastFrame),
                particle.Color.B + (int)(_blueChangePerSecond * timeSinceLastFrame));
        }
    }
}