using System.Numerics;
using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements
{
    public class Workbench : ImGuiElement
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        
        protected override void CustomRender()
        {
            ImGui.SetNextWindowPos(Position);
            ImGui.SetNextWindowSize(Size);
            if (ImGui.Begin("Workbench", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.BeginChild("Initializers", new Vector2((Size.X / 2) - 10, Size.Y - 40), true);
                
                ImGui.Columns(2, "initializercolumns", false);
                ImGui.Text("Texture File: "); ImGui.NextColumn();
                ImGui.Selectable("SampleParticles.png"); ImGui.NextColumn();

                ImGui.Text("Max Particle Lifetime: "); ImGui.NextColumn();
                
                var temp = 1f;
                ImGui.InputFloat("Seconds", ref temp);
                ImGui.NextColumn();

                ImGui.Text("Texture Section:"); ImGui.NextColumn();
                ImGui.Selectable("Single"); ImGui.NextColumn();
                ImGui.Text("Color Multiplier:"); ImGui.NextColumn();
                ImGui.Selectable("Static (1.0, 1.0, 1.0, 1.0)"); ImGui.NextColumn();
                
                ImGui.Columns(1);
                
                ImGui.EndChild();
                
                ImGui.SameLine();
                ImGui.BeginChild("Modifiers", new Vector2((Size.X / 2) - 10, Size.Y - 40), true);
                ImGui.Selectable("Constant Rotation: 5 degrees"); ImGui.NextColumn();
                ImGui.Selectable("Constant Acceleration: -5, 5"); ImGui.NextColumn();
                ImGui.EndChild();
            }
            
            ImGui.End();
        }
    }
}