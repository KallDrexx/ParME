using ImGuiHandler;
using ImGuiNET;

namespace Parme.Editor.Ui.Elements
{
    public class DemoWindow : ImGuiElement
    {
        protected override void CustomRender()
        {
            ImGui.ShowDemoWindow();
        }
    }
}