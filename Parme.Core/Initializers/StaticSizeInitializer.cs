namespace Parme.Core.Initializers
{
    public class StaticSizeInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.Size;
        public string EditorShortName => "Static";
        public string EditorShortValue => $"({Width}, {Height})";
        
        public int Width { get; set; }
        public int Height { get; set; }
    }
}