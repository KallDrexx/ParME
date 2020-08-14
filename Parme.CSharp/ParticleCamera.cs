using System.Numerics;

namespace Parme.CSharp
{
    public class ParticleCamera
    {
        public Vector2 Origin { get; set; }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }
        public bool PositiveYAxisPointsUp { get; set; }

        public bool IsInView(ref Particle particle)
        {
            var halfCameraWidth = PixelWidth / 2;
            var halfCameraHeight = PixelHeight / 2;
            var halfParticleWidth = particle.Size.X / 2;
            var halfParticleHeight = particle.Size.Y / 2;

            var isInHorizontalArea = particle.Position.X + halfParticleWidth > Origin.X - halfCameraWidth &&
                                     particle.Position.X - halfParticleWidth < Origin.X + halfCameraWidth;

            var isInVerticalArea = particle.Position.Y + halfParticleHeight > Origin.Y - halfCameraHeight &&
                                   particle.Position.Y - halfParticleHeight < Origin.Y + halfCameraHeight;

            return isInHorizontalArea && isInVerticalArea;
        }
    }
}