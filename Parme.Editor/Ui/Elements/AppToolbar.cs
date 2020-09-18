﻿using System;
using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements
{
    public class AppToolbar : ImGuiElement
    {
        public event EventHandler NewMenuItemClicked;
        public event EventHandler OpenMenuItemClicked; 
        
        public string CurrentlyOpenFileName { get; set; }
        public bool UnsavedChangesPresent { get; set; }
        
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
                
                ImGui.MenuItem("Save", false);
                ImGui.MenuItem("Save As", false);
                
                ImGui.Separator();
                
                ImGui.MenuItem("Quit", false);
                
                ImGui.EndMenu();
            }
        }
    }
}