namespace Parme.Core.Initializers
{
    public class RandomTextureInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.TextureSectionIndex;
        public string EditorShortName => "Random";
        public string EditorShortValue => string.Empty;
    }
}