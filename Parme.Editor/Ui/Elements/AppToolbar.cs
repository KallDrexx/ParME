﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ImGuiHandler;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using Parme.Editor.AppOperations;

namespace Parme.Editor.Ui.Elements
{
    public class AppToolbar : ImGuiElement
    {
        private readonly AppOperationQueue _appOperationQueue;
        private readonly ApplicationState _applicationState;
        private readonly List<SamplerState> _samplerStates = new List<SamplerState> 
            {null, SamplerState.PointClamp, SamplerState.LinearClamp, SamplerState.AnisotropicClamp,};
        
        private readonly string[] _samplerStateNames = {"<Unknown>", "Point", "Linear", "Anisotropic"};
        private int _selectedSamplerStateIndex;

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
                var versionWidth = ImGui.CalcTextSize(versionString);
                var versionStartPoint = ImGui.GetWindowWidth() - versionWidth.X - 25;
                ImGui.SameLine(versionStartPoint);
                ImGui.Text(versionString);

                var zoomString = $"{(int)(_applicationState.Zoom * 100)}%%";
                var zoomWidth = ImGui.CalcTextSize(zoomString);
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

                if (ImGui.BeginMenu("Recent"))
                {
                    if (_applicationState.RecentlyOpenedFiles.Any() != true)
                    {
                        ImGui.MenuItem("<No Files Recently Opened>");
                    }
                    else
                    {
                        foreach (var recentFile in _applicationState.RecentlyOpenedFiles)
                        {
                            if (ImGui.MenuItem(recentFile))
                            {
                                _appOperationQueue.Enqueue(new OpenEmitterRequested(recentFile));
                            }
                        }
                    }
                    
                    ImGui.EndMenu();
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

                if (ImGui.MenuItem("Auto Save", null, _applicationState.AutoSaveOnChange))
                {
                    _appOperationQueue.Enqueue(new UpdateMiscOptionsRequested
                    {
                        UpdatedAutoSave = !_applicationState.AutoSaveOnChange,
                    });
                }
                
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
                    _appOperationQueue.Enqueue(new UpdateMiscOptionsRequested
                    {
                        UpdatedBackgroundColor = color,
                    });
                }

                if (ImGui.Button("-##DecreaseZoom"))
                {
                    _appOperationQueue.Enqueue(new UpdateMiscOptionsRequested
                    {
                        UpdatedZoomLevel = _applicationState.Zoom - 0.1m,
                    });
                }
                
                ImGui.SameLine();
                ImGui.Text($"{(int)(_applicationState.Zoom * 100)}%%");
                
                ImGui.SameLine();
                if (ImGui.Button("+##IncreaseZoom"))
                {
                    _appOperationQueue.Enqueue(new UpdateMiscOptionsRequested
                    {
                        UpdatedZoomLevel = _applicationState.Zoom + 0.1m,
                    });
                }

                _selectedSamplerStateIndex = _samplerStates.IndexOf(_applicationState.RenderSamplerState);
                if (_selectedSamplerStateIndex < 0)
                {
                    _selectedSamplerStateIndex = 0;
                }

                if (ImGui.Combo("Monogame Sampler State",
                    ref _selectedSamplerStateIndex,
                    _samplerStateNames,
                    _samplerStateNames.Length))
                {
                    _appOperationQueue.Enqueue(new UpdateMiscOptionsRequested
                    {
                        UpdatedSamplerState = _samplerStates[_selectedSamplerStateIndex],
                    });
                }

                var gridSize = _applicationState.GridSize;
                if (ImGui.InputInt("Grid Size (pixels)", ref gridSize, 1))
                {
                    _appOperationQueue.Enqueue(new UpdateMiscOptionsRequested
                    {
                        UpdatedGridSize = gridSize,
                    });
                }

                ImGui.Separator();
                
                ImGui.Text($"Live Particle Count: {_applicationState.ParticleCount}");
                
                ImGui.EndMenu();
            }
        }
    }
}