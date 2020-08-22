using ImGuiHandler;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.ParticleCount
{
    public class RandomParticleCountEditor : ImGuiElement
    {
        public int MinSpawnCount
        {
            get => Get<int>();
            set => Set(value);
        }

        public int MaxSpawnCount
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputInt(nameof(MinSpawnCount), "Minimum");
            InputInt(nameof(MaxSpawnCount), "Maximum");
        }
    }
}