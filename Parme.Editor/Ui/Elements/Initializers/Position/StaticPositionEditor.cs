using ImGuiHandler;

namespace Parme.Editor.Ui.Elements.Initializers.Position
{
    public class StaticPositionEditor : ImGuiElement
    {
        public float XOffset
        {
            get => Get<float>();
            set => Set(value);
        }

        public float YOffset
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(XOffset), "X Offset");
            InputFloat(nameof(YOffset), "Y Offset");
        }
    }
}