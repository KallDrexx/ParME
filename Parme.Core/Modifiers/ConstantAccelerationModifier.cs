namespace Parme.Core.Modifiers
{
    public class ConstantAccelerationModifier : IParticleModifier
    {
        public string EditorShortName => "Constant Acceleration";
        public string EditorShortValue => $"({XAcceleration}, {YAcceleration})";
        
        public float XAcceleration { get; set; }
        public float YAcceleration { get; set; }
    }
}