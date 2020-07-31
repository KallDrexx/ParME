using Microsoft.Xna.Framework;

namespace Parme
{
    public class Particle
    {
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float TimeAlive { get; set; }
        public float RotationInRadians { get; set; }
    }
}