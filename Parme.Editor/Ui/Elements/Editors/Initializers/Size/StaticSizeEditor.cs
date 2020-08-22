using ImGuiHandler;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Size
{
    public class StaticSizeEditor : ImGuiElement
    {
        public int Width
        {
            get => Get<int>();
            set => Set(value);
        }

        public int Height
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputInt(nameof(Width), "Width");
            InputInt(nameof(Height), "Height");
        }
    }
}