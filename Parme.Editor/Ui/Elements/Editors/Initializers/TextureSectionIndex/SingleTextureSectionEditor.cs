using ImGuiNET;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.TextureSectionIndex
{
    public class SingleTextureSectionEditor : SettingsEditorBase
    {
        protected override void CustomRender()
        {
            ImGui.TextWrapped("Particles will be created with the first texture section defined");
        }

        protected override void OnNewSettingsLoaded()
        {
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
        }
    }
}