using ImGuiNET;

namespace Parme.Editor.Ui.Elements.Editors.Modifiers
{
    public class AnimatingTextureEditor : SettingsEditorBase
    {
        protected override void CustomRender()
        {
            ImGui.TextWrapped("Causes the particle to change its texture throughout it's lifetime, starting with" +
                              "the first texture section and ending in the last.  Each texture section will be displayed" +
                              "for an equal amount of time based on the particle lifetime");
        }

        protected override void OnNewSettingsLoaded()
        {
            
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            
        }
    }
}