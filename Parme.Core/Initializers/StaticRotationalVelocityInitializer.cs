namespace Parme.Core.Initializers
{
    public class StaticRotationalVelocityInitializer : IParticleInitializer
    {
        public string EditorShortName => "Static";
        public string EditorShortValue => $"{DegreesPerSecond}° / sec";
        public InitializerType InitializerType => InitializerType.RotationalVelocity;
        
        public int DegreesPerSecond { get; set; }
    }
}