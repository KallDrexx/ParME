namespace Parme.Core.Initializers
{
    public class RandomRotationalVelocityInitializer : IParticleInitializer
    {
        public string EditorShortName => "Random";
        public string EditorShortValue => $"{MinRotationSpeedInDegreesPerSecond}° - {MaxRotationSpeedInDegreesPerSecond}° / sec";
        public InitializerType InitializerType => InitializerType.RotationalVelocity;
        
        public int MinRotationSpeedInDegreesPerSecond { get; set; }
        public int MaxRotationSpeedInDegreesPerSecond { get; set; }
    }
}