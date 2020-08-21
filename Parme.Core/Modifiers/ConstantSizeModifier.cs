namespace Parme.Core.Modifiers
{
    public class ConstantSizeModifier : IParticleModifier
    {
        public string EditorShortName => "Constant Size";
        public string EditorShortValue => $"({WidthChangePerSecond}, {HeightChangePerSecond})";
        
        public float WidthChangePerSecond { get; set; }
        public float HeightChangePerSecond { get; set; }
    }
}