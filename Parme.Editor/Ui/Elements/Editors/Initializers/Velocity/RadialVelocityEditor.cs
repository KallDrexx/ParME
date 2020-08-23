using System.Linq;
using ImGuiNET;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Velocity
{
    public class RadialVelocityEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float Magnitude
        {
            get => Get<float>();
            set => Set(value);
        }
        
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
            InputFloat(nameof(Magnitude), "Magnitude");

            var min = MinDegrees;
            if (ImGui.SliderInt("Min Degrees", ref min, 0, 360))
            {
                MinDegrees = min;
            }
            
            var max = MaxDegrees;
            if (ImGui.SliderInt("Max Degrees", ref max, 0, 360))
            {
                MaxDegrees = max;
            }
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<RadialVelocityInitializer>()
                .First();

            Magnitude = initializer.Magnitude;
            MinDegrees = (int) initializer.MinDegrees;
            MaxDegrees = (int) initializer.MaxDegrees;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new RadialVelocityInitializer
            {
                Magnitude = Magnitude,
                MinDegrees = MinDegrees,
                MaxDegrees = MaxDegrees,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.Velocity, initializer));
        }
    }
}