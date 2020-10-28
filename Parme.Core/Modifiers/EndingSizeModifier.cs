namespace Parme.Core.Modifiers
{
    public class EndingSizeModifier : IParticleModifier
    {
        public string EditorShortName => "Ending Size";
        public string EditorShortValue => $"({Width}, {Height})";
        
        public int Width { get; set; }
        public int Height { get; set; }
    }
}