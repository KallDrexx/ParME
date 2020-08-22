using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Velocity
{
    public class RadialVelocityEditor : ImGuiElement
    {
        public float Magnitude
        {
            get => Get<float>();
            set => Set(value);
        }
        
        public float MinDegrees
        {
            get => Get<float>();
            set => Set(value);
        }

        public float MaxDegrees
        {
            get => Get<float>();
            set => Set(value);
        }

        protected override void CustomRender()
        {
            InputFloat(nameof(Magnitude), "Magnitude");

            var min = MinDegrees;
            if (ImGui.SliderFloat("Min Degrees", ref min, 0, 360))
            {
                MinDegrees = min;
            }
            
            var max = MaxDegrees;
            if (ImGui.SliderFloat("Max Degrees", ref max, 0, 360))
            {
                MaxDegrees = max;
            }
        }
    }
}