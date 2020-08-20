using System.Numerics;
using ImGuiHandler;
using Parme.Core;
using Parme.Editor.Ui.Elements;

namespace Parme.Editor.Ui
{
    public class EditorUiController
    {
        private readonly ImGuiManager _imGuiManager;
        private readonly DemoWindow _imguiDemoWindow;
        private readonly MainSidePanel _mainSidePanel;
        private readonly EmitterSettingsManager _emitterSettingsManager;
        private readonly EmitterSettingsController _emitterSettingsController;

        public bool AcceptingKeyboardInput => _imGuiManager.AcceptingKeyboardInput;
        public bool AcceptingMouseInput => _imGuiManager.AcceptingMouseInput;
        public Vector3 BackgroundColor => _mainSidePanel.BackgroundColor;

        public EmitterSettingsManager SettingsManager => _emitterSettingsManager;

        public EditorUiController(ImGuiManager imGuiManager)
        {
            _imGuiManager = imGuiManager;

            _imguiDemoWindow = new DemoWindow{IsVisible = false};
            _imGuiManager.AddElement(_imguiDemoWindow);

            _mainSidePanel = new MainSidePanel {IsVisible = false};
            _imGuiManager.AddElement(_mainSidePanel);

            _emitterSettingsManager = new EmitterSettingsManager(_mainSidePanel);
            _emitterSettingsController = new EmitterSettingsController(imGuiManager);
        }

        public void ToggleImGuiDemoWindow()
        {
            _imguiDemoWindow.IsVisible = !_imguiDemoWindow.IsVisible;
        }

        public void WindowResized(int width, int height)
        {
            _mainSidePanel.ViewportHeight = height;
            _emitterSettingsController.ViewportResized(width, height);
        }

        public void NewEmitterSettingsLoaded(EmitterSettings settings)
        {
            //_emitterSettingsManager.NewEmitterSettingsLoaded(settings);
            _emitterSettingsController.LoadNewSettings(settings);
        }
    }
}