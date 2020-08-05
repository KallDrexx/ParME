namespace Parme.Core.Initializers
{
    public class RadialVelocityInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.Velocity;
        
        public float Magnitude { get; set; }
        public float MinDegrees { get; set; }
        public float MaxDegrees { get; set; }
    }
}