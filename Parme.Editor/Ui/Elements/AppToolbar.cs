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
                }

                var versionString = $"v{AppVersion}";
                var width = ImGui.CalcTextSize(versionString) * ImGui.GetIO().FontGlobalScale;
                ImGui.SameLine(ImGui.GetWindowWidth() - width.X);
                ImGui.Text(versionString);

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