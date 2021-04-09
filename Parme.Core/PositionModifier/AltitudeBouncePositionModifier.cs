namespace Parme.Core.PositionModifier
{
    public class AltitudeBouncePositionModifier : IParticlePositionModifier
    {
        public string EditorShortName => "Altitude Based Bounce";
        public string EditorShortValue => $"({MinBounceAcceleration}-{MaxBounceAcceleration}, -{Gravity}, {Elasticity})";

        /// <summary>
        /// When the particle's altitude on the Y axis reaches zero, this is the minimum amount of acceleration in the
        /// Y axis that will be applied to the particle's altitude velocity
        /// </summary>
        public float MinBounceAcceleration { get; set; }
        
        /// <summary>
        /// When the particle's altitude on the Y axis reaches zero, this is the minimum amount of acceleration in the
        /// Y axis that will be applied to the particle's altitude velocity
        /// </summary>
        public float MaxBounceAcceleration { get; set; }
        
        /// <summary>
        /// Constant downward velocity applied to the particle's altitude
        /// </summary>
        public float Gravity { get; set; }

        /// <summary>
        /// How much bounce acceleration is preserved on each bounce
        /// </summary>
        public float Elasticity { get; set; } = 0.9f;
    }
}