using System.Linq;
using ImGuiNET;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.RotationalOrientation
{
    [EditorForType(typeof(RandomRotationInitializer))]
    public class RandomRotationEditor : SettingsEditorBase
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
            ImGui.TextWrapped("Sets the initial angle of the particle to be between the min" +
                              "and max degrees specified");
            
            ImGui.NewLine();
            var min = MinDegrees;
            var max = MaxDegrees;

            if (ImGui.SliderInt("Min", ref min, 0, 360))
            {
                MinDegrees = min;
            }

            if (ImGui.SliderInt("Max", ref max, 0, 360))
            {
                MaxDegrees = max;
            }
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<RandomRotationInitializer>()
                .First();

            MinDegrees = initializer.MinDegrees;
            MaxDegrees = initializer.MaxDegrees;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new RandomRotationInitializer
            {
                MinDegrees = MinDegrees,
                MaxDegrees = MaxDegrees,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.RotationalOrientation, initializer));
        }
    }
}