using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using Parme.Core;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors
{
    public class TextureSectionEditor : SettingsEditorBase
    {
        private const string PopupLabel = "Texture Sections";
        
        private readonly List<TextureSectionCoords> _textureSections = new List<TextureSectionCoords>();
        private bool _isOpen, _openRequested;
        private Texture2D _texture;
        private IntPtr _imguiTextureId;
        private TextureSection _currentSection;
        private float _zoomFactor = 1f;
        private int _gridSize = 32;
        private bool _showGrid;

        public void Open()
        {
            _openRequested = true;
        }
        
        protected override void CustomRender()
        {
            if (_openRequested)
            {
                ImGui.OpenPopup(PopupLabel);
                _openRequested = false;
                _isOpen = true;
            }

            var popupSize = ImGui.GetIO().DisplaySize;
            popupSize = new Vector2(popupSize.X - 50, popupSize.Y - 50);
            ImGui.SetNextWindowSize(popupSize);
            const ImGuiWindowFlags flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
            if (ImGui.BeginPopupModal(PopupLabel, ref _isOpen, flags))
            {
                RenderSectionList(popupSize);
                
                ImGui.NewLine();
                RenderControlsSection();
                
                ImGui.NewLine();
                RenderImageSection(popupSize);

                ImGui.EndPopup();
            }
        }

        protected override void OnNewSettingsLoaded()
        {
            _texture = TextureFileLoader.LoadTexture2D(EmitterSettings.TextureFileName);
            _imguiTextureId = MonoGameImGuiRenderer.BindTexture(_texture);
            
            _textureSections.Clear();
            _textureSections.AddRange(EmitterSettings.TextureSections);

            if (_currentSection != null)
            {
                if (_currentSection.Index >= _textureSections.Count)
                {
                    _currentSection = null;
                }
                else
                {
                    var section = _textureSections[_currentSection.Index];
                    _currentSection = new TextureSection(_currentSection.Index, section);
                }
            }
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            
        }

        private void RenderSectionList(Vector2 popupSize)
        {
            ImGui.Text("Sections:");
            
            if (CommandHandler.CanUndo &&
                CommandHandler.PreviousCommand.GetType() == typeof(UpdateTextureSectionsCommand))
            {
                ImGui.SameLine(ImGui.GetWindowWidth() - 120);
                if (ImGui.Button("Undo"))
                {
                    CommandHandler.Undo();
                }
            }
            
            if (CommandHandler.CanRedo &&
                CommandHandler.NextCommand.GetType() == typeof(UpdateTextureSectionsCommand))
            {
                ImGui.SameLine(ImGui.GetColumnWidth() - 40);
                if (ImGui.Button("Redo"))
                {
                    CommandHandler.Redo();
                }
            }
            
            var sectionWindowSize = new Vector2(popupSize.X - 20, 100);
            ImGui.BeginChild("sections", sectionWindowSize, true);

            var size = new Vector2(70, 70);
            for (var x = 0; x < _textureSections.Count; x++)
            {
                var section = _textureSections[x];
                var uv0 = new Vector2(section.LeftX / (float) _texture.Width, section.TopY / (float) _texture.Height);
                var uv1 = new Vector2(section.RightX / (float) _texture.Width, section.BottomY / (float) _texture.Height);
                ImGui.PushID("section_" + x);
                if (ImGui.ImageButton(_imguiTextureId, size, uv0, uv1))
                {
                    _currentSection = new TextureSection(x, section);
                }
                ImGui.PopID();
                
                ImGui.SameLine();
            }

            if (ImGui.Button("+##AddSection"))
            {
                AddNewSection();
            }
            
            ImGui.EndChild();
        }

        private void RenderControlsSection()
        {
            ImGui.Text("Current Section:");
            ImGui.SameLine();

            if (_currentSection == null)
            {
                ImGui.Text("<No Section Selected>");
                ImGui.NewLine();
                ImGui.NewLine();
                ImGui.NewLine();
                ImGui.NewLine();
                ImGui.NewLine();
            }
            else
            {
                ImGui.Text($"#{_currentSection.Index + 1}");
                
                ImGui.SameLine();
                if (ImGui.Button("Remove"))
                {
                    RemoveCurrentSection();
                }
                
                ImGui.SameLine();
                if (ImGui.Button("Duplicate"))
                {
                    AddNewSection(new TextureSectionCoords(_currentSection.Coords.LeftX,
                        _currentSection.Coords.TopY,
                        _currentSection.Coords.RightX,
                        _currentSection.Coords.BottomY));
                }
                
                var left = _currentSection.Coords.LeftX;
                var top = _currentSection.Coords.TopY;
                var width = _currentSection.Coords.RightX - _currentSection.Coords.LeftX;
                var height = _currentSection.Coords.BottomY - _currentSection.Coords.TopY;
                var valuesChanged = false;

                ImGui.PushItemWidth(100);
                if (ImGui.InputInt("Start X", ref left)) valuesChanged = true;
                if (ImGui.InputInt("Start Y", ref top)) valuesChanged = true;
                if (ImGui.InputInt("Width", ref width)) valuesChanged = true;
                if (ImGui.InputInt("Height", ref height)) valuesChanged = true;
                ImGui.PopItemWidth();

                if (valuesChanged)
                {
                    var coords = new TextureSectionCoords(left, top, left + width, top + height);
                    UpdateSectionValues(new TextureSection(_currentSection.Index, coords));
                }
            }
        }

        private void RenderImageSection(Vector2 popupSize)
        {
            const int xBuffer = 20;
            const int yBuffer = 55;
            const int listAndControlHeight = 325;

            var sectionWindowSize = new Vector2(popupSize.X - xBuffer, popupSize.Y - listAndControlHeight - yBuffer);
            ImGui.BeginChild("fullImage", sectionWindowSize, true, ImGuiWindowFlags.HorizontalScrollbar);

            var scaleHeight = (sectionWindowSize.Y - xBuffer) / _texture.Height;
            var scaleWidth = (sectionWindowSize.X - yBuffer) / _texture.Width;
            var scale = Math.Min(scaleHeight, scaleWidth) * _zoomFactor;
            var screenStartPosition = ImGui.GetCursorScreenPos();

            ImGui.Image(_imguiTextureId, new Vector2(_texture.Width * scale, _texture.Height * scale));
            
            var mouseHoveringImage = ImGui.IsItemHovered();
            var imageSize = ImGui.GetItemRectSize();

            var drawList = ImGui.GetWindowDrawList();
            var guideLineColor = ImGui.GetColorU32(new Vector4(1, 1, 1, 1));

            if (_showGrid)
            {
                var increment = (int) (_gridSize * scale);
                if (increment < 1)
                {
                    increment = 1;
                }

                for (var x = 0; x <= imageSize.X; x += increment)
                {
                    var realX = screenStartPosition.X + x;

                    drawList.AddLine(
                        new Vector2(realX, screenStartPosition.Y),
                        new Vector2(realX, screenStartPosition.Y + imageSize.Y),
                        guideLineColor,
                        0.5f);
                }

                for (var y = 0; y <= imageSize.Y; y += increment)
                {
                    var realY = screenStartPosition.Y + y;

                    drawList.AddLine(
                        new Vector2(screenStartPosition.X, realY),
                        new Vector2(screenStartPosition.X + imageSize.X, realY),
                        guideLineColor,
                        0.25f);
                }
            }

            if (_currentSection != null)
            {
                var top = screenStartPosition.Y + Math.Min(_currentSection.Coords.TopY, _currentSection.Coords.BottomY) * scale;
                var bottom = screenStartPosition.Y + Math.Max(_currentSection.Coords.TopY, _currentSection.Coords.BottomY) * scale;
                var left = screenStartPosition.X + Math.Min(_currentSection.Coords.LeftX, _currentSection.Coords.RightX) * scale;
                var right = screenStartPosition.X + Math.Max(_currentSection.Coords.LeftX, _currentSection.Coords.RightX) * scale;

                if (right <= imageSize.X || bottom <= imageSize.Y)
                {
                    var color = ImGui.GetColorU32(new Vector4(1f, 1f, 0.4f, 1f));
                    drawList.AddRect(new Vector2(left, top), new Vector2(right, bottom), color);
                }
            }

            ImGui.EndChild();

            ImGui.Text("Zoom:");
            ImGui.SameLine();

            ImGui.SetNextItemWidth(100);
            var zoom = (int)(_zoomFactor * 100);
            if (ImGui.InputInt("%##Zoom", ref zoom, 10) && zoom > 0)
            {
                _zoomFactor = (float) zoom / 100;
            }

            ImGui.SameLine();

            if (ImGui.Button("Reset##ResetZoom"))
            {
                _zoomFactor = 1;
            }
            
            ImGui.Text("Grid Size:");
            
            ImGui.SameLine();
            ImGui.SetNextItemWidth(200);
            var gridSize = _gridSize;
            if (ImGui.InputInt("##GridSize", ref gridSize) && gridSize > 0)
            {
                _gridSize = gridSize;
            }

            ImGui.SameLine();
            ImGui.Checkbox("Show", ref _showGrid);

            if (mouseHoveringImage)
            {
                // Show the user what pixel coordinates they have hovered
                var mousePosition = ImGui.GetMousePos();
                var x = (int) Math.Round((mousePosition.X - screenStartPosition.X) / scale);
                var y = (int) Math.Round((mousePosition.Y - screenStartPosition.Y) / scale);

                var text = $"({x}, {y})";
                var textWidth = ImGui.CalcTextSize(text) * ImGui.GetIO().FontGlobalScale;

                ImGui.SameLine(ImGui.GetWindowWidth() - textWidth.X);
                ImGui.Text(text);
            }
        }

        private void AddNewSection(TextureSectionCoords? coords = null)
        {
            var newSections = _textureSections.Concat(new[]
            {
                coords ?? new TextureSectionCoords(0, 0, _texture.Width, _texture.Height),
            }).ToList();
            
            CommandHandler.Execute(new UpdateTextureSectionsCommand(newSections));
            
            // Set the new section as current one, so it's auto selected
            _currentSection = new TextureSection(newSections.Count - 1, new TextureSectionCoords());
        }

        private void RemoveCurrentSection()
        {
            if (_currentSection == null)
            {
                return;
            }

            var updatedList = _textureSections
                .Where((x, index) => index != _currentSection.Index)
                .ToList();
            
            CommandHandler.Execute(new UpdateTextureSectionsCommand(updatedList));
        }

        private void UpdateSectionValues(TextureSection updatedSection)
        {
            var sections = _textureSections.ToList();
            sections[updatedSection.Index] = updatedSection.Coords;
            
            CommandHandler.Execute(new UpdateTextureSectionsCommand(sections));
        }

        private class TextureSection
        {
            public int Index { get; }
            public TextureSectionCoords Coords { get; }

            public TextureSection(int index, TextureSectionCoords coords)
            {
                Index = index;
                Coords = coords;
            }
        }
    }
}
