using System.ComponentModel;

namespace Parme.Editor.Ui.Elements.Editors
{
    public class ParticleLifetimeEditor : SettingsEditorBase
    {
        public float Lifetime
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(Lifetime), "Seconds");
        }

        protected override void OnNewSettingsLoaded()
        {
            Lifetime = EmitterSettings.MaxParticleLifeTime;
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