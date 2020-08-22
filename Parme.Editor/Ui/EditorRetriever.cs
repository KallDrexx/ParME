using System.ComponentModel;
using Parme.Core;
using Parme.Core.Triggers;
using Parme.Editor.Ui.Elements.Editors;
using Parme.Editor.Ui.Elements.Editors.Triggers;

namespace Parme.Editor.Ui
{
    public static class EditorRetriever
    {
        public static SettingsEditorBase GetEditor(EditorItem item)
        {
            return item.ItemType switch
            {
                EditorItemType.Trigger => new TriggerEditor(),
                _ => null
            };
        }

        public static SettingsEditorBase GetEditor(IEditorObject editableObject)
        {
            return editableObject?.GetType().Name switch
            {
                nameof(OneShotTrigger) => new OneShotTriggerEditor(),
                nameof(TimeElapsedTrigger) => new TimeElapsedTriggerEditor(),
                _ => null
            };
        }
    }
}