using System.Linq;
using System.Numerics;
using ImGuiNET;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Velocity
{
    [EditorForType(typeof(RandomRangeVelocityInitializer))]
    public class RandomRangeVelocityEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float MinXVelocity
        {
            get => Get<float>();
            set => Set(value);
        }
        
        [SelfManagedProperty]
        public float MaxXVelocity
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float MinYVelocity
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float MaxYVelocity
        {
            get => Get<float>();
            set => Set(value);
        }

        protected override void CustomRender()
        {
            InputFloat(nameof(MinXVelocity), "Min X");
            InputFloat(nameof(MinYVelocity), "Min Y");
            
            ImGui.NewLine();
            InputFloat(nameof(MaxXVelocity), "Max X");
            InputFloat(nameof(MaxYVelocity), "Max Y");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<RandomRangeVelocityInitializer>()
                .First();

            MinXVelocity = initializer.MinXVelocity;
            MinYVelocity = initializer.MinYVelocity;
            MaxXVelocity = initializer.MaxXVelocity;
            MaxYVelocity = initializer.MaxYVelocity;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new RandomRangeVelocityInitializer
            {
                MinXVelocity = MinXVelocity,
                MinYVelocity = MinYVelocity,
                MaxXVelocity = MaxXVelocity,
                MaxYVelocity = MaxYVelocity,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.Velocity, initializer));
        }
    }
}