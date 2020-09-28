using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Numerics;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using Parme.Core;

namespace Parme.Editor.Ui.Elements.Editors
{
    public class TextureSectionDisplayEditor : SettingsEditorBase
    {
        private readonly List<TextureSectionCoords> _sections = new List<TextureSectionCoords>();
        private readonly TextureSectionEditor _textureSectionEditor;
        private Texture2D _texture;
        private IntPtr? _imguiTextureId;

        public TextureSectionDisplayEditor()
        {
            _textureSectionEditor = new TextureSectionEditor();
        }

        protected override void CustomRender()
        {
            ImGui.Text($"# Of Sections: {_sections.Count}");

            if (_texture != null && _imguiTextureId != null)
            {
                if (ImGui.Button("Edit"))
                {
                    _textureSectionEditor.Open();
                }
                
                ImGui.NewLine();
                ImGui.Columns(2, "sections", false);

                foreach (var section in _sections)
                {
                    var size = new Vector2(80, 80);
                    var uv0 = new Vector2(section.LeftX / (float) _texture.Width, section.TopY / (float) _texture.Height);
                    var uv1 = new Vector2(section.RightX / (float) _texture.Width, section.BottomY / (float) _texture.Height);
                    ImGui.Image(_imguiTextureId.Value, size, uv0, uv1);
                    ImGui.NextColumn();
                }
                
                ImGui.Columns(1);
                
                _textureSectionEditor.Render();
            }
        }

        protected override void OnNewSettingsLoaded()
        {
            _sections.Clear();
            _sections.AddRange(EmitterSettings.TextureSections ?? Array.Empty<TextureSectionCoords>());

            if (!string.IsNullOrWhiteSpace(EmitterSettings.TextureFileName))
            {
                _texture = TextureFileLoader.LoadTexture2D(EmitterSettings.TextureFileName);
                _imguiTextureId = MonoGameImGuiRenderer.BindTexture(_texture);
            }

            if (_texture != null)
            {
                // Since we have no texture, there's no point to the section editor existing, so don't bother
                // doing anything until we actually have a texture loaded
                _textureSectionEditor.ApplicationState = ApplicationState;
                _textureSectionEditor.CommandHandler = CommandHandler;
                _textureSectionEditor.MonoGameImGuiRenderer = MonoGameImGuiRenderer;
                _textureSectionEditor.AppOperationQueue = AppOperationQueue;
                _textureSectionEditor.TextureFileLoader = TextureFileLoader;
                _textureSectionEditor.EmitterSettings = EmitterSettings;
            }
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            
        }
    }
}