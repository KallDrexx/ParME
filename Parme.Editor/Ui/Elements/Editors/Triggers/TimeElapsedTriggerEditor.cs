using System.ComponentModel;
using Parme.Core.Triggers;

namespace Parme.Editor.Ui.Elements.Editors.Triggers
{
    public class TimeElapsedTriggerEditor : SettingsEditorBase
    {
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

        protected override void OnSelfPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var trigger = (TimeElapsedTrigger) EmitterSettings.Trigger;
            switch (e.PropertyName)
            {
                case nameof(Frequency):
                    trigger.Frequency = Frequency;
                    break;
            }
        }
    }
}