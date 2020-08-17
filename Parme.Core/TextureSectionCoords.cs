using System;

namespace Parme.Core
{
    public readonly struct TextureSectionCoords : IEquatable<TextureSectionCoords>
    {
        /// <summary>
        /// How many pixels from the left of the atlas is the left of the texture section
        /// </summary>
        public readonly int LeftX;
        
        /// <summary>
        /// How many pixels from the top of the atlas is the top of the texture section
        /// </summary>
        public readonly int TopY;
        
        /// <summary>
        /// How many pixels from the left of the atlas is the right of the texture section
        /// </summary>
        public readonly int RightX;
        
        /// <summary>
        /// How many pixels from the top of the atlas is the bottom of the texture section
        /// </summary>
        public readonly int BottomY;

        public TextureSectionCoords(int leftX, int topY, int rightX, int bottomY)
        {
            LeftX = leftX;
            TopY = topY;
            RightX = rightX;
            BottomY = bottomY;
        }

        public bool Equals(TextureSectionCoords other)
        {
            return LeftX == other.LeftX && 
                   TopY == other.TopY && 
                   RightX == other.RightX && 
                   BottomY == other.BottomY;
        }

        public override bool Equals(object obj)
        {
            return obj is TextureSectionCoords other && 
                   Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = LeftX;
                hashCode = (hashCode * 397) ^ TopY;
                hashCode = (hashCode * 397) ^ RightX;
                hashCode = (hashCode * 397) ^ BottomY;
                return hashCode;
            }
        }
    }
}