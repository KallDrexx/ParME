using System.Numerics;

namespace Parme.CSharp
{
    public struct Particle
    {
        public bool IsAlive;
        public byte TextureSlotId;
        public Vector2 Size;
        public Vector2 Position;
        public Vector2 Velocity;
        public float TimeAlive;
        public float RotationInRadians;
        public float RedMultiplier;
        public float GreenMultiplier;
        public float BlueMultiplier;
        public float AlphaMultiplier;
    }
}