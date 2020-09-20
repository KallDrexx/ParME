using System;
using System.Numerics;
using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements
{
    public class MessagePopup : ImGuiElement
    {
        private const string PopupLabel = "Message";
        
        private bool _openRequested;
        private bool _isOpen;
        private string _message;

        public event EventHandler ModalClosed;

        public void Display(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                _openRequested = true;
                _message = message;
            }
        }
        
        protected override void CustomRender()
        {
            if (_openRequested)
            {
                ImGui.OpenPopup(PopupLabel);
                _isOpen = true;
                _openRequested = false;
            }

            var wasOpen = _isOpen;
            ImGui.SetNextWindowSize(new Vector2(700, 500));
            if (ImGui.BeginPopupModal(PopupLabel, ref _isOpen, ImGuiWindowFlags.Modal))
            {
                ImGui.TextWrapped(_message);
                ImGui.NewLine();
                
                ImGui.Separator();

                if (ImGui.Button("Close"))
                {
                    _isOpen = false;
                    ImGui.CloseCurrentPopup();
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