namespace Parme.Core.Initializers
{
    public class StaticPositionInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.Position;
        
        public float XOffset { get; set; }
        public float YOffset { get; set; }
    }
}