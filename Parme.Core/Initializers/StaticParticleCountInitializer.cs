namespace Parme.Core.Initializers
{
    public class StaticParticleCountInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.ParticleCount;
        public string EditorShortName => "Static";
        public string EditorShortValue => $"{ParticleSpawnCount}";
        
        public int ParticleSpawnCount { get; set; }
    }
}