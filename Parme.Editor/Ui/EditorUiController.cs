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
        private readonly EmitterSettingsController _emitterSettingsController;

        public bool AcceptingKeyboardInput => _imGuiManager.AcceptingKeyboardInput;
        public bool AcceptingMouseInput => _imGuiManager.AcceptingMouseInput;

        public EditorUiController(ImGuiManager imGuiManager, SettingsCommandHandler commandHandler)
        {
            _imGuiManager = imGuiManager;

            _imguiDemoWindow = new DemoWindow{IsVisible = false};
            _imGuiManager.AddElement(_imguiDemoWindow);
            _emitterSettingsController = new EmitterSettingsController(imGuiManager, commandHandler);
        }

        public void Update()
        {
            _emitterSettingsController.Update();
        }

        public void ToggleImGuiDemoWindow()
        {
            _imguiDemoWindow.IsVisible = !_imguiDemoWindow.IsVisible;
        }

        public void WindowResized(int width, int height)
        {
            _emitterSettingsController.ViewportResized(width, height);
        }

        public void NewEmitterSettingsLoaded(EmitterSettings settings)
        {
            //_emitterSettingsManager.NewEmitterSettingsLoaded(settings);
            _emitterSettingsController.LoadNewSettings(settings);
        }
    }
}