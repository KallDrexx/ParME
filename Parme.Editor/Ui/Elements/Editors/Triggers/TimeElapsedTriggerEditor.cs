using System.ComponentModel;
using Parme.Core;
using Parme.Core.Triggers;

namespace Parme.Editor.Ui.Elements.Editors.Triggers
{
    public class TimeElapsedTriggerEditor : SettingsEditorBase
    {
        public TimeElapsedTriggerEditor(EmitterSettings settings) : base(settings)
        {
        }
        
        public float Frequency
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(Frequency), "Frequency");
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