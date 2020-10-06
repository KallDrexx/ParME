namespace Parme.Core.Modifiers
{
    public class DragModifier : IParticleModifier
    {
        public string EditorShortName => "Drag";
        public string EditorShortValue => $"{DragFactor:0.00}";
        
        public float DragFactor { get; set; }
    }
}