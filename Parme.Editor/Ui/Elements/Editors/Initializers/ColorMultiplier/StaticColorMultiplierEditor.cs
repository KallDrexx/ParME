using System.Numerics;
using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.ColorMultiplier
{
    public class StaticColorMultiplierEditor : ImGuiElement
    {
        public float RedMultiplier
        {
            get => Get<float>();
            set => Set(value);
        }
        
        public float GreenMultiplier
        {
            get => Get<float>();
            set => Set(value);
        }
        
        public float BlueMultiplier
        {
            get => Get<float>();
            set => Set(value);
        }
        
        public float AlphaMultiplier
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            var colors = new Vector4(RedMultiplier, GreenMultiplier, BlueMultiplier, AlphaMultiplier);

            if (ImGui.SliderFloat4("Color", ref colors, 0f, 1f))
            {
                RedMultiplier = colors.X;
                GreenMultiplier = colors.Y;
                BlueMultiplier = colors.Z;
                AlphaMultiplier = colors.W;
            }
        }
    }
}