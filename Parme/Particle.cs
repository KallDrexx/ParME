using Microsoft.Xna.Framework;

namespace Parme
{
    public struct Particle
    {
        public bool IsAlive;
        public Vector2 Size;
        public Vector2 Position;
        public Vector2 Velocity;
        public float TimeAlive;
        public float RotationInRadians;
        public Color ColorModifier;
    }
}