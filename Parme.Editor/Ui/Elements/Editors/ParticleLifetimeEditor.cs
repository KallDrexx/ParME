using System.ComponentModel;
using Parme.Core;

namespace Parme.Editor.Ui.Elements.Editors
{
    public class ParticleLifetimeEditor : SettingsEditorBase
    {
        public ParticleLifetimeEditor(EmitterSettings settings) : base(settings)
        {
        }
        
        public float Lifetime
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(Lifetime), "Seconds");
        }

        protected override void OnSelfPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Lifetime):
                    EmitterSettings.MaxParticleLifeTime = Lifetime;
                    break;
            }
        }
    }
}