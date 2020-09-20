using System;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using ImGuiNET;
using Parme.Editor.AppOperations;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors
{
    public class TextureFileEditor : SettingsEditorBase
    {
        private string _currentFileName;
        private IntPtr? _imguiTextureId;

        [SelfManagedProperty]
        private string SelectedFileName
        {
            get => Get<string>();
            set => Set(value);
        }

        public override void Dispose()
        {
            if (_imguiTextureId != null)
            {
                MonoGameImGuiRenderer.UnbindTexture(_imguiTextureId.Value);
            }
        }

        protected override void CustomRender()
        {
            var display = !string.IsNullOrWhiteSpace(_currentFileName)
                ? $"File: {_currentFileName}"
                : "File: <None>";
            
            ImGui.TextWrapped(display);

            if (ImGui.Button("Select File"))
            {
                HandleSelectFile();
            }
            
            ImGui.NewLine();

            if (_imguiTextureId != null)
            {
                ImGui.Image(_imguiTextureId.Value, new Vector2(250, 250));
            }
        }

        protected override void OnNewSettingsLoaded()
        {
            _currentFileName = EmitterSettings.TextureFileName;

            if (!string.IsNullOrWhiteSpace(_currentFileName))
            {
                var texture = TextureFileLoader.LoadTexture2D(_currentFileName);
                _imguiTextureId = MonoGameImGuiRenderer.BindTexture(texture);
            }
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(SelectedFileName))
            {
                return;
            }
            
            var textureFileName = Path.GetFileName(SelectedFileName);

            var sourceDirectory = Path.GetDirectoryName(SelectedFileName);
            var emitterDirectory = Path.GetDirectoryName(ApplicationState.ActiveFileName);
            var destinationFileName = Path.Combine(emitterDirectory!, textureFileName!);
            
            // Move the texture image to the same location as the emitter.  They should always be side by side
            // for now.
            if (!string.IsNullOrWhiteSpace(sourceDirectory) && sourceDirectory != emitterDirectory)
            {
                try
                {
                    File.Copy(SelectedFileName, destinationFileName, true);
                }
                catch (Exception exception)
                {
                    var message =
                        $"Failed to move '{textureFileName}' from '{sourceDirectory}' to '{emitterDirectory}': " +
                        $"{exception.Message}";
                    
                    AppOperationQueue.Enqueue(new RaiseErrorMessage(message));
                    return;
                }
            }
            
            // Texture file now exists in the same directory as the emitter, so now update the emitter to use it
            CommandHandler.Execute(new UpdateTextureFileNameCommand(textureFileName));
        }

        private void HandleSelectFile()
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = Path.GetDirectoryName(ApplicationState.ActiveFileName),
                DefaultExt = ".png",
                Filter = "Texture File|*.png"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectedFileName = dialog.FileName;
            }
        }
    }
}