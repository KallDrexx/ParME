using ImGuiHandler;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.ParticleCount
{
    public class StaticParticleCountEditor : ImGuiElement
    {
        public int ParticleSpawnCount
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputInt(nameof(ParticleSpawnCount), "Count");
        }
    }
}