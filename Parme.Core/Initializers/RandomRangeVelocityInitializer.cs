namespace Parme.Core.Initializers
{
    public class RandomRangeVelocityInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.Velocity;
        public string EditorShortName => "Random Range";
        public string EditorShortValue => $"({MinXVelocity}, {MinYVelocity}) - ({MaxXVelocity}, {MaxYVelocity})";
        
        public float MinXVelocity { get; set; }
        public float MaxXVelocity { get; set; }
        public float MinYVelocity { get; set; }
        public float MaxYVelocity { get; set; }
    }
}