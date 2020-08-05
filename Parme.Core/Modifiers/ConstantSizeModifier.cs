namespace Parme.Core.Modifiers
{
    public class ConstantSizeModifier : IParticleModifier
    {
        public float WidthChangePerSecond { get; set; }
        public float HeightChangePerSecond { get; set; }
    }
}