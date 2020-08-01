using Microsoft.Xna.Framework;

namespace Parme.Modifiers
{
    public class ConstantColorChangeModifier : IParticleModifier
    {
        private readonly float _redChangePerSecond, _blueChangePerSecond, _greenChangePerSecond;

        public ConstantColorChangeModifier(float redChangePerSecond, float blueChangePerSecond, float greenChangePerSecond)
        {
            _redChangePerSecond = redChangePerSecond;
            _blueChangePerSecond = blueChangePerSecond;
            _greenChangePerSecond = greenChangePerSecond;
        }

        public void Update(float timeSinceLastFrame, ref Particle particle)
        {
            particle.ColorModifier = new Color(particle.ColorModifier.R + (int)(_redChangePerSecond * timeSinceLastFrame),
                particle.ColorModifier.G + (int)(_greenChangePerSecond * timeSinceLastFrame),
                particle.ColorModifier.B + (int)(_blueChangePerSecond * timeSinceLastFrame));
        }
    }
}