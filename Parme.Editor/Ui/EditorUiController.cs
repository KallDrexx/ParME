using System.Numerics;
using ImGuiHandler;
using Parme.Core;
using Parme.CSharp;
using Parme.Editor.Ui.Elements;

namespace Parme.Editor.Ui
{
    public class EditorUiController
    {
        private readonly ImGuiManager _imGuiManager;
        private readonly DemoWindow _imguiDemoWindow;
        private readonly MainSidePanel _mainSidePanel;
        private readonly EmitterSettingsManager _emitterSettingsManager;

        public bool AcceptingKeyboardInput => _imGuiManager.AcceptingKeyboardInput;
        public bool AcceptingMouseInput => _imGuiManager.AcceptingMouseInput;
        public Vector3 BackgroundColor => _mainSidePanel.BackgroundColor;

        public EmitterSettingsManager SettingsManager => _emitterSettingsManager;

        public EditorUiController(ImGuiManager imGuiManager)
        {
            _imGuiManager = imGuiManager;
            
            _imguiDemoWindow = new DemoWindow{IsVisible = false};
            _imGuiManager.AddElement(_imguiDemoWindow);

            _mainSidePanel = new MainSidePanel {IsVisible = true};
            _imGuiManager.AddElement(_mainSidePanel);
            
            _emitterSettingsManager = new EmitterSettingsManager(_mainSidePanel);
        }

        public void ToggleImGuiDemoWindow()
        {
            _imguiDemoWindow.IsVisible = !_imguiDemoWindow.IsVisible;
        }

        public void WindowResized(int width, int height)
        {
            _mainSidePanel.ViewportHeight = height;
        }

        public void NewEmitterSettingsLoaded(EmitterSettings settings)
        {
            _emitterSettingsManager.NewEmitterSettingsLoaded(settings);
        }
    }
}