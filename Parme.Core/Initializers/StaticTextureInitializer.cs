namespace Parme.Core.Initializers
{
    public class StaticTextureInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.TextureSectionIndex;
        
        public int TextureSectionIndex { get; set; }
    }
}