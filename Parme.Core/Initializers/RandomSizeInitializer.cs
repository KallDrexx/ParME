namespace Parme.Core.Initializers
{
    public class RandomSizeInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.Size;
        
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }
    }
}