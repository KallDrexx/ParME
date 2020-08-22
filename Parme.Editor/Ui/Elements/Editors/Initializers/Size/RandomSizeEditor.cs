using ImGuiHandler;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Size
{
    public class RandomSizeEditor : ImGuiElement
    {
        public int MinWidth
        {
            get => Get<int>();
            set => Set(value);
        }
        
        public int MinHeight
        {
            get => Get<int>();
            set => Set(value);
        }

        public int MaxWidth
        {
            get => Get<int>();
            set => Set(value);
        }

        public int MaxHeight
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputInt(nameof(MinWidth), "Min Width");
            InputInt(nameof(MinHeight), "Min Height");
            InputInt(nameof(MaxWidth), "Max Width");
            InputInt(nameof(MaxHeight), "Max Height");
        }
    }
}