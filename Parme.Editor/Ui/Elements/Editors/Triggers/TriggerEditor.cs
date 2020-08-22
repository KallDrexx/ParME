using System.ComponentModel;
using Parme.Core;
using Parme.Core.Triggers;

namespace Parme.Editor.Ui.Elements.Editors.Triggers
{
    public class TriggerEditor : SettingsEditorBase
    {
        private readonly TypeSelector _selector;
        
        public TriggerEditor(EmitterSettings settings) : base(settings)
        {
            _selector = new TypeSelector(new IParticleTrigger[]
            {
                new OneShotTrigger(), 
                new TimeElapsedTrigger(), 
            });
            
            _selector.SelectedType = settings.Trigger?.GetType();
            _selector.PropertyChanged += SelectorOnPropertyChanged;
        }

        protected override void CustomRender()
        {
            _selector.Render();
        }

        protected override void OnSelfPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void SelectorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(TypeSelector.SelectedType):
                    switch (_selector.SelectedType?.GetType().Name)
                    {
                        case null:
                        {
                            _selector.ChildDisplay = null;
                            EmitterSettings.Trigger = null;
                            break;
                        }

                        case nameof(OneShotTrigger):
                        {
                            break;
                        }
                    }
                    
                    break;
            }
        }
    }
}