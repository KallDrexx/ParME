using System.Linq;
using ImGuiNET;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.RotationalVelocity
{
    [EditorForType(typeof(RandomRotationalVelocityInitializer))]
    public class RandomRotationalVelocityEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public int MinDegrees
        {
            get => Get<int>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public int MaxDegrees
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            ImGui.Text("Degrees Per Second");
            InputInt(nameof(MinDegrees), "Min");
            InputInt(nameof(MaxDegrees), "Max");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<RandomRotationalVelocityInitializer>()
                .First();

            MinDegrees = initializer.MinRotationSpeedInDegreesPerSecond;
            MaxDegrees = initializer.MaxRotationSpeedInDegreesPerSecond;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new RandomRotationalVelocityInitializer
            {
                MinRotationSpeedInDegreesPerSecond = MinDegrees,
                MaxRotationSpeedInDegreesPerSecond = MaxDegrees,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.RotationalVelocity, initializer));
        }
    }
}