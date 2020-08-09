using System.Numerics;
using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements
{
    public class MainSidePanel : ImGuiElement
    {
        public int ViewportHeight { get; set; }
        
        public TypeSelector TriggerParentSection { get; set; }
        public TypeSelector ParticleCountSelector { get; set; }
        public TypeSelector ColorMultiplierSelector { get; set; }
        public TypeSelector PositionSelector { get; set; }
        public TypeSelector SizeSelector { get; set; }
        public TypeSelector VelocitySelector { get; set; }

        public Vector3 BackgroundColor
        {
            get => Get<Vector3>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            const ImGuiWindowFlags flags = ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize;
            
            ImGui.SetNextWindowSize(new Vector2(300, ViewportHeight));
            ImGui.SetNextWindowPos(new Vector2(0, 0));
            if (ImGui.Begin("Emitter Settings", flags))
            {
                ImGui.Text("Background Color");
                var color = BackgroundColor;
                if (ImGui.ColorEdit3("Background Color", ref color, ImGuiColorEditFlags.NoLabel))
                {
                    BackgroundColor = color;
                }
                
                ImGui.Separator();

                ImGui.SetNextItemOpen(true, ImGuiCond.FirstUseEver);
                if (ImGui.TreeNode("Trigger"))
                {
                    TriggerParentSection?.Render();
                    ImGui.NewLine();
                    ImGui.TreePop();
                }

                ImGui.SetNextItemOpen(true, ImGuiCond.FirstUseEver);
                if (ImGui.TreeNode("Particle Count"))
                {
                    ParticleCountSelector?.Render();
                    ImGui.NewLine();
                    ImGui.TreePop();
                }

                ImGui.SetNextItemOpen(true, ImGuiCond.FirstUseEver);
                if (ImGui.TreeNode("Color Multiplier"))
                {
                    ColorMultiplierSelector?.Render();
                    ImGui.NewLine();
                    ImGui.TreePop();
                }

                ImGui.SetNextItemOpen(true, ImGuiCond.FirstUseEver);
                if (ImGui.TreeNode("Position"))
                {
                    PositionSelector?.Render();
                    ImGui.NewLine();
                    ImGui.TreePop();
                }

                ImGui.SetNextItemOpen(true, ImGuiCond.FirstUseEver);
                if (ImGui.TreeNode("Velocity"))
                {
                    VelocitySelector?.Render();
                    ImGui.NewLine();
                    ImGui.TreePop();
                }
                
                ImGui.SetNextItemOpen(true, ImGuiCond.FirstUseEver);
                if (ImGui.TreeNode("Size"))
                {
                    SizeSelector?.Render();
                    ImGui.NewLine();
                    ImGui.TreePop();
                }
            }
            
            ImGui.End();
        }
    }
}