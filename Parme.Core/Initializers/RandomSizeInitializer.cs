namespace Parme.Core.Initializers
{
    public class RandomSizeInitializer : IParticleInitializer
    {
        public enum Axis { X, Y }
        
        public InitializerType InitializerType => InitializerType.Size;
        public string EditorShortName => "Random In Range";
        public string EditorShortValue => 
            $"({MinWidth}, {MinHeight}) - ({MaxWidth}, {MaxHeight}) {(PreserveAspectRatio ? "(Ratio Preserved)" : "")}";
        
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public int MinHeight { get; set; }
        public int MaxHeight { get; set; }
        public bool PreserveAspectRatio { get; set; }
        public Axis? RandomizedAxis { get; set; }
    }
}