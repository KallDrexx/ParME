using Parme.Core.Triggers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Triggers
{
    public class TimeElapsedTriggerEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float Frequency
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(Frequency), "Frequency");
        }

        protected override void OnNewSettingsLoaded()
        {
            Frequency = ((TimeElapsedTrigger) EmitterSettings.Trigger).Frequency;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var trigger = new TimeElapsedTrigger
            {
                Frequency = Frequency,
            };
            
            CommandHandler.CommandPerformed(new UpdateTriggerCommand(trigger));
        }
    }
}