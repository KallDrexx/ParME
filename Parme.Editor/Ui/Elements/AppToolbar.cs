using System;
using System.Numerics;
using ImGuiHandler;
using ImGuiNET;
using Parme.Editor.AppOperations;

namespace Parme.Editor.Ui.Elements
{
    public class AppToolbar : ImGuiElement
    {
        private readonly AppOperationQueue _appOperationQueue;
        private readonly ApplicationState _applicationState;

        public event EventHandler NewMenuItemClicked;
        public event EventHandler OpenMenuItemClicked;
        public event EventHandler<bool> SaveMenuItemClicked;

        public AppToolbar(AppOperationQueue appOperationQueue, ApplicationState applicationState)
        {
            _appOperationQueue = appOperationQueue;
            _applicationState = applicationState;
        }

        protected override void CustomRender()
        {
            if (ImGui.BeginMainMenuBar())
            {
                CreateFileMenu();
                CreateViewMenu();

                if (!string.IsNullOrWhiteSpace(_applicationState.ActiveFileName))
                {
                    ImGui.SameLine();
                    ImGui.Text($" - {_applicationState.ActiveFileName}");

                    if (_applicationState.HasUnsavedChanges)
                    {
                        ImGui.SameLine();
                        ImGui.Text("*");
                    }
                }

                var versionString = $"v{_applicationState.Version}";
                var versionWidth = ImGui.CalcTextSize(versionString) * ImGui.GetIO().FontGlobalScale;
                var versionStartPoint = ImGui.GetWindowWidth() - versionWidth.X;
                ImGui.SameLine(versionStartPoint);
                ImGui.Text(versionString);

                var zoomString = $"{(int)(_applicationState.Zoom * 100)}%%";
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

                var canSave = !string.IsNullOrWhiteSpace(_applicationState.ActiveFileName);
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

        private void CreateViewMenu()
        {
            if (ImGui.BeginMenu("View"))
            {
                var color = _applicationState.BackgroundColor;
                if (ImGui.ColorEdit3("Background Color", ref color))
                {
                    _appOperationQueue.Enqueue(new UpdateViewOptionsRequested
                    {
                        UpdatedBackgroundColor = color,
                    });
                }

                if (ImGui.Button("-##DecreaseZoom"))
                {
                    _appOperationQueue.Enqueue(new UpdateViewOptionsRequested
                    {
                        UpdatedZoomLevel = _applicationState.Zoom - 0.1m,
                    });
                }
                
                ImGui.SameLine();
                ImGui.Text($"{(int)(_applicationState.Zoom * 100)}%%");
                
                ImGui.SameLine();
                if (ImGui.Button("+##IncreaseZoom"))
                {
                    _appOperationQueue.Enqueue(new UpdateViewOptionsRequested
                    {
                        UpdatedZoomLevel = _applicationState.Zoom + 0.1m,
                    });
                }
                
                ImGui.Separator();
                
                ImGui.Text($"Live Particle Count: {_applicationState.ParticleCount}");
                
                ImGui.EndMenu();
            }
        }
    }
}