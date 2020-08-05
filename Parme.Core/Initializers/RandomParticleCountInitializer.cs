namespace Parme.Core.Initializers
{
    public class RandomParticleCountInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.ParticleCount;
        
        public int MinimumToSpawn { get; set; }
        public int MaximumToSpawn { get; set; }
    }
}