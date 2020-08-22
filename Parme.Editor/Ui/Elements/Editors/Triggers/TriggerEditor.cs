using System;
using Parme.Core.Triggers;
using Parme.Editor.Commands;

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

        protected override void NewTypeSelected()
        {
            if (SelectedType == null)
            {
                CommandHandler.Execute(new UpdateTriggerCommand(null));
            }
            else
            {
                var trigger = (IParticleTrigger) Activator.CreateInstance(SelectedType);
                CommandHandler.Execute(new UpdateTriggerCommand(trigger));    
            }
        }
    }
}