using System.Linq;
using ImGuiNET;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Size
{
    [EditorForType(typeof(RandomSizeInitializer))]
    public class RandomSizeEditor : SettingsEditorBase
    {
        private readonly string[] _axisLabels = new[] {"X", "Y"};
        private int _selectedAxisLabelIndex;
        
        [SelfManagedProperty]
        public int MinWidth
        {
            get => Get<int>();
            set => Set(value);
        }
        
        [SelfManagedProperty]
        public int MinHeight
        {
            get => Get<int>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public int MaxWidth
        {
            get => Get<int>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public int MaxHeight
        {
            get => Get<int>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public bool PreserveAspectRatio
        {
            get => Get<bool>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public RandomSizeInitializer.Axis? RandomizedAxis
        {
            get => Get<RandomSizeInitializer.Axis?>();
            set => Set(value);
        }

        protected override void CustomRender()
        {
            if (!PreserveAspectRatio)
            {
                InputInt(nameof(MinWidth), "Min Width");
                InputInt(nameof(MinHeight), "Min Height");
            
                ImGui.NewLine();
                
                InputInt(nameof(MaxWidth), "Max Width");
                InputInt(nameof(MaxHeight), "Max Height");
            }
            else
            {
                if (RandomizedAxis == RandomSizeInitializer.Axis.Y)
                {
                    ImGui.Text("Height:");
                    InputInt(nameof(MinHeight), "Min##MinHeight");
                    InputInt(nameof(MaxHeight), "Max##MaxHeight");
                }
                else
                {
                    ImGui.Text("Width:");
                    InputInt(nameof(MinWidth), "Min##MinWidth");
                    InputInt(nameof(MaxWidth), "Max##MaxWidth");
                }
                
                ImGui.NewLine();
                ImGui.Text("Axis To Randomize:");
                if (ImGui.Combo("##Axis", ref _selectedAxisLabelIndex, _axisLabels, _axisLabels.Length))
                {
                    RandomizedAxis = _selectedAxisLabelIndex == 1
                        ? RandomSizeInitializer.Axis.Y
                        : RandomSizeInitializer.Axis.X;
                }
            }
            
            ImGui.NewLine();
            Checkbox(nameof(PreserveAspectRatio), "Preserve Aspect Ratio");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<RandomSizeInitializer>()
                .First();

            MinWidth = initializer.MinWidth;
            MinHeight = initializer.MinHeight;
            MaxWidth = initializer.MaxWidth;
            MaxHeight = initializer.MaxHeight;
            PreserveAspectRatio = initializer.PreserveAspectRatio;
            RandomizedAxis = initializer.RandomizedAxis;

            _selectedAxisLabelIndex = RandomizedAxis == RandomSizeInitializer.Axis.Y ? 1 : 0;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new RandomSizeInitializer
            {
                MinWidth = MinWidth,
                MinHeight = MinHeight,
                MaxWidth = MaxWidth,
                MaxHeight = MaxHeight,
                RandomizedAxis = RandomizedAxis,
                PreserveAspectRatio = PreserveAspectRatio,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.Size, initializer));
        }
    }
}