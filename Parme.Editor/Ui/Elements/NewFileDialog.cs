using System;
using System.Windows.Forms;
using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements
{
    public class NewFileDialog : ImGuiElement
    {
        private const string PopupLabel = "New Particle Emitter";
        private readonly NewEmitterTemplate[] _emitterTemplates = {NewEmitterTemplate.None, NewEmitterTemplate.Fire};
        private readonly string[] _templateNames = {"<Blank>", "Fire"};
        private int _selectedTemplateIndex;
        private bool _openRequested;
        private bool _isOpen;

        public event EventHandler CreateButtonClicked;
        public event EventHandler ModalClosed;

        [HasTextBuffer(256)]
        public string NewFileName
        {
            get => Get<string>();
            set => Set(value);
        }
        
        public string ErrorMessage { get; set; }
        public NewEmitterTemplate SelectedTemplate => _emitterTemplates[_selectedTemplateIndex];
        public bool DialogIsOpen => _isOpen;

        public void OpenPopup()
        {
            _openRequested = true;
            NewFileName = string.Empty;
            _selectedTemplateIndex = 0;
            ErrorMessage = string.Empty;
        }

        public void ClosePopup()
        {
            _isOpen = false;
        }

        protected override void CustomRender()
        {
            if (_openRequested)
            {
                ImGui.OpenPopup(PopupLabel);
                _openRequested = false;
                _isOpen = true;
            }

            var wasOpen = _isOpen;
            if (ImGui.BeginPopupModal(PopupLabel, ref _isOpen, ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGui.Text("File Name:");
                ImGui.SameLine();
                
                ImGui.SetNextItemWidth(500);
                InputText(nameof(NewFileName), "##NewFileName");
                ImGui.SameLine();
                
                if (ImGui.Button("..."))
                {
                    var dialog = new SaveFileDialog
                    {
                        AddExtension = true,
                        DefaultExt = App.DefaultExtension,
                        Filter = $"Particle Emitter Definition|*{App.DefaultExtension}",
                        OverwritePrompt = true,
                    };
                    
                    var result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        NewFileName = dialog.FileName;
                    }
                }
                
                ImGui.Text("Starting Template:");
                ImGui.SameLine();

                ImGui.Combo("##TemplateSelector", ref _selectedTemplateIndex, _templateNames, _templateNames.Length);

                ImGui.NewLine();
                if (ImGui.Button("Create"))
                {
                    CreateButtonClicked?.Invoke(this, EventArgs.Empty);
                }

                if (!string.IsNullOrWhiteSpace(ErrorMessage))
                {
                    ImGui.Separator();
                    ImGui.TextWrapped(ErrorMessage);
                }
            
                ImGui.EndPopup();
            }

            if (wasOpen && !_isOpen)
            {
                ModalClosed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}