using Parme.Core.Triggers;

namespace Parme.Editor.Ui.Elements.Editors.Triggers
{
    public class TriggerEditor : TypeSelectorEditor
    {
        public TriggerEditor() 
            : base(new[] {typeof(OneShotTrigger), typeof(TimeElapsedTrigger)})
        {
        }

        protected override void UpdateSelectedTypeFromSettings()
        {
            SelectedType = EmitterSettings.Trigger?.GetType();
        }
    }
}