using System.Linq;
using ImGuiNET;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Velocity
{
    [EditorForType(typeof(RadialVelocityInitializer))]
    public class RadialVelocityEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float MinMagnitude
        {
            get => Get<float>();
            set => Set(value);
        }
        
        [SelfManagedProperty]
        public float MaxMagnitude
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

        [SelfManagedProperty]
        public float XAxisScale
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float YAxisScale
        {
            get => Get<float>();
            set => Set(value);
        }

        protected override void CustomRender()
        {
            ImGui.Text("Magnitude");
            InputFloat(nameof(MinMagnitude), "Min##Magnitude");
            InputFloat(nameof(MaxMagnitude), "Max##Magnitude");
            InputFloat(nameof(XAxisScale), "X Axis Scale");
            InputFloat(nameof(YAxisScale), "Y Axis Scale");
            
            ImGui.NewLine();
            ImGui.Text("Angles (degrees)");
            var min = MinDegrees;
            if (ImGui.InputInt("Min##Degrees", ref min) && min >= 0 && min <= 360)
            {
                MinDegrees = min;
            }
            
            var max = MaxDegrees;
            if (ImGui.InputInt("Max##Degrees", ref max) && max >= 0 && max <= 360)
            {
                MaxDegrees = max;
            }
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<RadialVelocityInitializer>()
                .First();

            MinMagnitude = initializer.MinMagnitude;
            MaxMagnitude = initializer.MaxMagnitude;
            MinDegrees = (int) initializer.MinDegrees;
            MaxDegrees = (int) initializer.MaxDegrees;
            XAxisScale = initializer.XAxisScale;
            YAxisScale = initializer.YAxisScale;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new RadialVelocityInitializer
            {
                MinMagnitude = MinMagnitude,
                MaxMagnitude = MaxMagnitude,
                MinDegrees = MinDegrees,
                MaxDegrees = MaxDegrees,
                XAxisScale = XAxisScale,
                YAxisScale = YAxisScale,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.Velocity, initializer));
        }
    }
}