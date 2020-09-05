using System.Linq;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.ParticleCount
{
    [EditorForType(typeof(RandomParticleCountInitializer))]
    public class RandomParticleCountEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public int MinSpawnCount
        {
            get => Get<int>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public int MaxSpawnCount
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputInt(nameof(MinSpawnCount), "Minimum");
            InputInt(nameof(MaxSpawnCount), "Maximum");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<RandomParticleCountInitializer>()
                .First();

            MinSpawnCount = initializer.MinimumToSpawn;
            MaxSpawnCount = initializer.MaximumToSpawn;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new RandomParticleCountInitializer
            {
                MinimumToSpawn = MinSpawnCount,
                MaximumToSpawn = MaxSpawnCount,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.ParticleCount, initializer));
        }
    }
}