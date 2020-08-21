namespace Parme.Core.Modifiers
{
    public class ConstantColorMultiplierChangeModifier : IParticleModifier
    {
        public string EditorShortName => "Constant Color Mult";
        public string EditorShortValue => $"{RedMultiplierChangePerSecond}, " +
                                          $"{GreenMultiplierChangePerSecond}, " +
                                          $"{BlueMultiplierChangePerSecond}, " +
                                          $"{AlphaMultiplierChangePerSecond}";
        
        public float RedMultiplierChangePerSecond { get; set; }
        public float GreenMultiplierChangePerSecond { get; set; }
        public float BlueMultiplierChangePerSecond { get; set; }
        public float AlphaMultiplierChangePerSecond { get; set; }
    }
}