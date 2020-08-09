using ImGuiHandler;

namespace Parme.Editor.Ui.Elements.Triggers
{
    public class TimeElapsedTriggerEditor : ImGuiElement
    {
        public float Frequency
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(Frequency), "Frequency");
        }
    }
}