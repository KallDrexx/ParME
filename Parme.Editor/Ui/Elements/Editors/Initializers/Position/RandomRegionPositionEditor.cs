using System.Numerics;
using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Position
{
    public class RandomRegionPositionEditor : ImGuiElement
    {
        public float MinXOffset
        {
            get => Get<float>();
            set => Set(value);
        }

        public float MinYOffset
        {
            get => Get<float>();
            set => Set(value);
        }
        
        public float MaxXOffset
        {
            get => Get<float>();
            set => Set(value);
        }

        public float MaxYOffset
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            var min = new Vector2(MinXOffset, MinYOffset);
            var max = new Vector2(MaxXOffset, MaxYOffset);

            if (ImGui.InputFloat2("Minimum", ref min))
            {
                MinXOffset = min.X;
                MinYOffset = min.Y;
            }

            if (ImGui.InputFloat2("Maximum", ref max))
            {
                MaxXOffset = max.X;
                MaxYOffset = max.Y;
            }
        }
    }
}