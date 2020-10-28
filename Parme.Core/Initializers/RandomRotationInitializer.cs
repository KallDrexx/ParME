namespace Parme.Core.Initializers
{
    public class RandomRotationInitializer : IParticleInitializer
    {
        public string EditorShortName => "Random Rotation";
        public string EditorShortValue => $"{MinDegrees}° - {MaxDegrees}°";
        public InitializerType InitializerType => InitializerType.RotationalOrientation;
        
        public int MinDegrees { get; set; }
        public int MaxDegrees { get; set; }
    }
}