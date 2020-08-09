using ImGuiHandler;
using Parme.Editor.Ui.Elements;

namespace Parme.Editor.Ui
{
    public class EditorUiController
    {
        private readonly ImGuiManager _imGuiManager;
        private readonly DemoWindow _imguiDemoWindow;

        public bool AcceptingKeyboardInput => _imGuiManager.AcceptingKeyboardInput;
        public bool AcceptingMouseInput => _imGuiManager.AcceptingMouseInput;

        public EditorUiController(ImGuiManager imGuiManager)
        {
            _imGuiManager = imGuiManager;
            
            _imguiDemoWindow = new DemoWindow();
            _imGuiManager.AddElement(_imguiDemoWindow);
        }

        public void ToggleImGuiDemoWindow()
        {
            _imguiDemoWindow.IsVisible = !_imguiDemoWindow.IsVisible;
        }
    }
}