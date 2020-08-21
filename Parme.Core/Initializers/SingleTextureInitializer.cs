namespace Parme.Core.Initializers
{
    public class SingleTextureInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.TextureSectionIndex;
        public string EditorShortName => "Static";
        public string EditorShortValue => string.Empty;
    }
}