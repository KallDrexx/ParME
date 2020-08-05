namespace Parme.Core.Initializers
{
    public class StaticParticleCountInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.ParticleCount;
        
        public int ParticleSpawnCount { get; set; }
    }
}