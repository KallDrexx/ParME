namespace Parme.Core.Modifiers
{
    public class ConstantRotationModifier : IParticleModifier
    {
        public string EditorShortName => "Constant Rotation";
        public string EditorShortValue => $"{DegreesPerSecond}°";
        
        public float DegreesPerSecond { get; set; }
    }
}