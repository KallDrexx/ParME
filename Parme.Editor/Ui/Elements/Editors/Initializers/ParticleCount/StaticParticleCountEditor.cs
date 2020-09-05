using System.Linq;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.ParticleCount
{
    [EditorForType(typeof(StaticParticleCountInitializer))]
    public class StaticParticleCountEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public int ParticleSpawnCount
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputInt(nameof(ParticleSpawnCount), "Count");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<StaticParticleCountInitializer>()
                .First();

            ParticleSpawnCount = initializer.ParticleSpawnCount;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new StaticParticleCountInitializer
            {
                ParticleSpawnCount = ParticleSpawnCount,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.ParticleCount, initializer));
        }
    }
}