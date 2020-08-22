using System.Numerics;
using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Velocity
{
    public class RandomRangeVelocityEditor : ImGuiElement
    {
        public float MinXVelocity
        {
            get => Get<float>();
            set => Set(value);
        }
        
        public float MaxXVelocity
        {
            get => Get<float>();
            set => Set(value);
        }

        public float MinYVelocity
        {
            get => Get<float>();
            set => Set(value);
        }

        public float MaxYVelocity
        {
            get => Get<float>();
            set => Set(value);
        }

        protected override void CustomRender()
        {
            var min = new Vector2(MinXVelocity, MinYVelocity);
            var max = new Vector2(MaxXVelocity, MaxYVelocity);

            if (ImGui.InputFloat2("Minimum", ref min))
            {
                MinXVelocity = min.X;
                MinYVelocity = min.Y;
            }

            if (ImGui.InputFloat2("Maximum", ref max))
            {
                MaxXVelocity = max.X;
                MaxYVelocity = max.Y;
            }
        }
    }
}