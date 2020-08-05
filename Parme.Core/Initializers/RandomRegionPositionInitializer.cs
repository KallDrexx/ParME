namespace Parme.Core.Initializers
{
    public class RandomRegionPositionInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.Position;
        
        public float MinXOffset { get; set; }
        public float MinYOffset { get; set; }
        public float MaxXOffset { get; set; }
        public float MaxYOffset { get; set; }
    }
}