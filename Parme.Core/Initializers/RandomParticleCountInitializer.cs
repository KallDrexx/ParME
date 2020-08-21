namespace Parme.Core.Initializers
{
    public class RandomParticleCountInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.ParticleCount;
        public string EditorShortName => "Random";
        public string EditorShortValue => $"{MinimumToSpawn} - {MaximumToSpawn}";
        
        public int MinimumToSpawn { get; set; }
        public int MaximumToSpawn { get; set; }
    }
}