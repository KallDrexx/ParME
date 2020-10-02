namespace Parme.Core.Modifiers
{
    public class EndingColorModifier : IParticleModifier
    {
        public string EditorShortName => "Ending Color Multiplier";
        public string EditorShortValue => $"{Red}, {Green}, {Blue}, {Alpha:0.00}";

        public byte Red { get; set; } = 255;
        public byte Green { get; set; } = 255;
        public byte Blue { get; set; } = 255;
        public float Alpha { get; set; } = 1f;
    }
}