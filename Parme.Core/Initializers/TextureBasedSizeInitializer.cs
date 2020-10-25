namespace Parme.Core.Initializers
{
    public class TextureBasedSizeInitializer : IParticleInitializer
    {
        public string EditorShortName => "Texture Based";
        public string EditorShortValue => $"{Percentage}%";
        public InitializerType InitializerType => InitializerType.Size;

        public int Percentage { get; set; } = 100;
    }
}