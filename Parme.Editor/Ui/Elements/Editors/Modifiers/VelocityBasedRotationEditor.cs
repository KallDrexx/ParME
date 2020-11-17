using ImGuiNET;
using Parme.Core.Modifiers;

namespace Parme.Editor.Ui.Elements.Editors.Modifiers
{
    [EditorForType(typeof(VelocityBasedRotationModifier))]
    public class VelocityBasedRotationEditor : SettingsEditorBase
    {
        protected override void CustomRender()
        {
            ImGui.TextWrapped("Every frame the particle will have the rotation set to be the same as the the " +
                              "particle's velocity.\n\nThis modifier overrides all other rotational settings");
        }

        protected override void OnNewSettingsLoaded()
        {
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
        }
    }
}