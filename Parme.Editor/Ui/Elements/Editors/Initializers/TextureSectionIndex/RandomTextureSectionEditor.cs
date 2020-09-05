using ImGuiNET;
using Parme.Core.Initializers;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.TextureSectionIndex
{
    [EditorForType(typeof(RandomTextureInitializer))]
    public class RandomTextureSectionEditor : SettingsEditorBase
    {
        protected override void CustomRender()
        {
            ImGui.TextWrapped("Particles will be created with a randomly defined texture section");
        }

        protected override void OnNewSettingsLoaded()
        {
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
        }
    }
}