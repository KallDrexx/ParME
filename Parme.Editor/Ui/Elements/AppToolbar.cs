using System;
using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements
{
    public class AppToolbar : ImGuiElement
    {
        public event EventHandler NewMenuItemClicked;
        public event EventHandler OpenMenuItemClicked; 
        
        public string CurrentlyOpenFileName { get; set; }
        
        protected override void CustomRender()
        {
            if (ImGui.BeginMainMenuBar())
            {
                CreateFileMenu();

                if (!string.IsNullOrWhiteSpace(CurrentlyOpenFileName))
                {
                    ImGui.SameLine();
                    ImGui.Text($" - {CurrentlyOpenFileName}");
                }

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

                if (ImGui.MenuItem("Open", false))
                {
                    OpenMenuItemClicked?.Invoke(this, EventArgs.Empty);
                }
                
                ImGui.MenuItem("Save", false);
                ImGui.MenuItem("Save As", false);
                
                ImGui.Separator();
                
                ImGui.MenuItem("Quit", false);
                
                ImGui.EndMenu();
            }
        }
    }
}