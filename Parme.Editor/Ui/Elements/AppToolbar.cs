using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using ImGuiHandler;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using Parme.Editor.AppOperations;
using Parme.Editor.Settings;

namespace Parme.Editor.Ui.Elements
{
    public class AppToolbar : ImGuiElement
    {
        private readonly AppOperationQueue _appOperationQueue;
        private readonly ApplicationState _applicationState;
        private readonly List<SamplerState> _samplerStates = new List<SamplerState> 
            {null, SamplerState.PointClamp, SamplerState.LinearClamp, SamplerState.AnisotropicClamp,};

        private readonly List<AutoMoveTextureOption> _autoMoveOptions = new List<AutoMoveTextureOption>()
            {AutoMoveTextureOption.Ask, AutoMoveTextureOption.AlwaysCopy, AutoMoveTextureOption.AlwaysLeave};
        
        private readonly string[] _samplerStateNames = {"<Unknown>", "Point", "Linear", "Anisotropic"};
        private readonly string[] _autoMoveTextureOptions = {"Ask", "Always Copy", "Do Not Copy"};

        private int _selectedSamplerStateIndex;
        private int _selectedAutoMoveIndex;

        public event EventHandler NewMenuItemClicked;
        public event EventHandler OpenMenuItemClicked;
        public event EventHandler<bool> SaveMenuItemClicked;

        public AppToolbar(AppOperationQueue appOperationQueue, ApplicationState applicationState)
        {
            _appOperationQueue = appOperationQueue;
            _applicationState = applicationState;

            _selectedAutoMoveIndex = _autoMoveOptions.IndexOf(applicationState.AutoMoveTextureOption);
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
                if (ImGui.BeginMenu("Settings"))
                {
                    if (ImGui.MenuItem("Auto Save", null, _applicationState.AutoSaveOnChange))
                    {
                        _appOperationQueue.Enqueue(new UpdateMiscOptionsRequested
                        {
                            UpdatedAutoSave = !_applicationState.AutoSaveOnChange,
                        });
                    }
                    
                    ImGui.Text("Copy Texture To Emitter's Location");
                    ImGui.SameLine();
                    if (ImGui.Combo("##automove", 
                        ref _selectedAutoMoveIndex, 
                        _autoMoveTextureOptions,
                        _autoMoveOptions.Count))
                    {
                        _applicationState.AutoMoveTextureOption = _autoMoveOptions[_selectedAutoMoveIndex];
                    }
                    
                    ImGui.EndMenu();
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
                
                ImGui.SameLine();
                if (ImGui.Button("Reset Camera"))
                {
                    _appOperationQueue.Enqueue(new UpdateMiscOptionsRequested
                    {
                        ResetCamera = true,
                        UpdatedZoomLevel = 1.0m,
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
                
                RenderReferenceSpriteSection();

                ImGui.Separator();
                
                ImGui.Text($"Live Particle Count: {_applicationState.ParticleCount}");
                
                ImGui.EndMenu();
            }
        }

        private void RenderReferenceSpriteSection()
        {
            if (ImGui.BeginMenu($"Reference Sprite: {_applicationState.ReferenceSpriteFilename ?? "<None>"}"))
            {
                if (ImGui.MenuItem("<None>"))
                {
                    _appOperationQueue.Enqueue(new NewReferenceSpriteRequested(null));
                }

                foreach (var file in _applicationState.RecentReferenceSprites)
                {
                    if (ImGui.MenuItem(file))
                    {
                        _appOperationQueue.Enqueue(new NewReferenceSpriteRequested(file));
                    }
                }

                ImGui.Separator();
                if (ImGui.MenuItem("Other File"))
                {
                    var dialog = new OpenFileDialog
                    {
                        InitialDirectory = Path.GetDirectoryName(_applicationState.ActiveFileName),
                        DefaultExt = ".png",
                        Filter = "Image File|*.png"
                    };

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        _appOperationQueue.Enqueue(new NewReferenceSpriteRequested(dialog.FileName));
                    }
                }
                
                ImGui.EndMenu();
            }

            var xOffset = _applicationState.ReferenceSpriteOffset.X;
            var yOffset = _applicationState.ReferenceSpriteOffset.Y;
            if (ImGui.InputFloat("X Offset", ref xOffset) || ImGui.InputFloat("Y Offset", ref yOffset))
            {
                _applicationState.ReferenceSpriteOffset = new Vector2(xOffset, yOffset);
            }
        }
    }
}