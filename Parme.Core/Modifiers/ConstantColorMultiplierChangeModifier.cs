namespace Parme.Core.Modifiers
{
    public class ConstantColorMultiplierChangeModifier : IParticleModifier
    {
        public float RedMultiplierChangePerSecond { get; set; }
        public float GreenMultiplierChangePerSecond { get; set; }
        public float BlueMultiplierChangePerSecond { get; set; }
        public float AlphaMultiplierChangePerSecond { get; set; }
    }
}