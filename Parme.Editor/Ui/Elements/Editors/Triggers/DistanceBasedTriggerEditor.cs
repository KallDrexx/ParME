using Parme.Core.Triggers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Triggers
{
    [EditorForType(typeof(DistanceBasedTrigger))]
    public class DistanceBasedTriggerEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float UnitsPerEmission
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(UnitsPerEmission), "Distance");
        }

        protected override void OnNewSettingsLoaded()
        {
            UnitsPerEmission = ((DistanceBasedTrigger) EmitterSettings.Trigger).UnitsPerEmission;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var trigger = new DistanceBasedTrigger
            {
                UnitsPerEmission = UnitsPerEmission,
            };
            
            CommandHandler.Execute(new UpdateTriggerCommand(trigger));
        }
    }
}