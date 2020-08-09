using System.Numerics;
using ImGuiHandler;
using ImGuiNET;
using Parme.Editor.Ui.Elements.Triggers;

namespace Parme.Editor.Ui.Elements
{
    public class MainSidePanel : ImGuiElement
    {
        public int ViewportHeight { get; set; }
        
        public TriggerParentSection TriggerParentSection { get; set; }

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

                if (ImGui.TreeNode("Trigger"))
                {
                    TriggerParentSection?.Render();
                    
                    ImGui.TreePop();
                }
            }
            
            ImGui.End();
        }
    }
}