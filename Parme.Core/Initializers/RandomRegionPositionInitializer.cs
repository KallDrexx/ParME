namespace Parme.Core.Initializers
{
    public class RandomRegionPositionInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.Position;
        public string EditorShortName => "Region";
        public string EditorShortValue => $"({MinXOffset}, {MinYOffset}) - ({MaxXOffset}, {MaxYOffset})";
        
        public float MinXOffset { get; set; }
        public float MinYOffset { get; set; }
        public float MaxXOffset { get; set; }
        public float MaxYOffset { get; set; }
    }
}