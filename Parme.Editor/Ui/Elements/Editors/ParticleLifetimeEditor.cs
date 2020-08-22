using System.ComponentModel;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors
{
    public class ParticleLifetimeEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
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

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            CommandHandler.CommandPerformed(new UpdateParticleLifetimeCommand(Lifetime));
        }
    }
}