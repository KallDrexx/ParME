﻿using System.Numerics;

namespace Parme.CSharp
{
    public struct Particle
    {
        public bool IsAlive;
        public byte TextureSectionIndex;
        
        /// <summary>
        /// The standard (non-zoomed) width and height of the particle, in pixels
        /// </summary>
        public Vector2 Size;

        public Vector2 InitialSize;
        
        /// <summary>
        /// The center position of the particle in world space
        /// </summary>
        public Vector2 Position;
        public Vector2 Velocity;
        public float TimeAlive;
        public float RotationInRadians;
        public float RotationalVelocityInRadians;
        public byte InitialRed;
        public byte InitialGreen;
        public byte InitialBlue;
        public byte InitialAlpha;
        public byte CurrentRed;
        public byte CurrentGreen;
        public byte CurrentBlue;
        public byte CurrentAlpha;
    }
}