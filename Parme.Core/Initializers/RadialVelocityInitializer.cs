using System;

namespace Parme.Core.Initializers
{
    public class RadialVelocityInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.Velocity;
        public string EditorShortName => "Wedge";
        public string EditorShortValue => $"({MaxMagnitude} - {MinMagnitude}) angle {MinDegrees}° - {MaxDegrees}°";

        [Obsolete("Replaced with min and max magnitude")]
        public float Magnitude
        {
            set
            {
                MinMagnitude = value;
                MaxMagnitude = value;
            }
        }
        
        public float MinMagnitude { get; set; }
        public float MaxMagnitude { get; set; }
        public float MinDegrees { get; set; }
        public float MaxDegrees { get; set; }
    }
}