using System;
using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements
{
    public class AppToolbar : ImGuiElement
    {
        public event EventHandler NewMenuItemClicked;
        public event EventHandler OpenMenuItemClicked;
        public event EventHandler<bool> SaveMenuItemClicked; 
        
        public string CurrentlyOpenFileName { get; set; }
        public bool UnsavedChangesPresent { get; set; }
        public Version AppVersion { get; set; }
        public int ParticleCount { get; set; }
        public int ZoomPercentage { get; set; }
        
        protected override void CustomRender()
        {
            if (ImGui.BeginMainMenuBar())
            {
                CreateFileMenu();

                if (!string.IsNullOrWhiteSpace(CurrentlyOpenFileName))
                {
                    ImGui.SameLine();
                    ImGui.Text($" - {CurrentlyOpenFileName}");

                    if (UnsavedChangesPresent)
                    {
                        ImGui.SameLine();
                        ImGui.Text("*");
                    }
                    
                    ImGui.SameLine();
                    ImGui.Text($" ({ParticleCount} Particles)");
                }

                var versionString = $"v{AppVersion}";
                var versionWidth = ImGui.CalcTextSize(versionString) * ImGui.GetIO().FontGlobalScale;
                var versionStartPoint = ImGui.GetWindowWidth() - versionWidth.X;
                ImGui.SameLine(versionStartPoint);
                ImGui.Text(versionString);

                var zoomString = $"{ZoomPercentage}%%";
                var zoomWidth = ImGui.CalcTextSize(zoomString) * ImGui.GetIO().FontGlobalScale;
                var zoomStartPoint = versionStartPoint - zoomWidth.X - 25;
                ImGui.SameLine(zoomStartPoint);
                ImGui.Text(zoomString);

                ImGui.EndMainMenuBar();
            }
        }

        private void CreateFileMenu()
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("New"))
                {
                    NewMenuItemClicked?.Invoke(this, EventArgs.Empty);
                }

                if (ImGui.MenuItem("Open"))
                {
                    OpenMenuItemClicked?.Invoke(this, EventArgs.Empty);
                }

                var canSave = !string.IsNullOrWhiteSpace(CurrentlyOpenFileName);
                if (ImGui.MenuItem("Save", canSave))
                {
                    SaveMenuItemClicked?.Invoke(this, false);
                }

                if (ImGui.MenuItem("Save As", canSave))
                {
                    SaveMenuItemClicked?.Invoke(this, true);
                }
                
                ImGui.Separator();
                
                ImGui.MenuItem("Quit", false);
                
                ImGui.EndMenu();
            }
        }
    }
}