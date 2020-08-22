using System.Linq;
using System.Numerics;
using ImGuiNET;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Position
{
    public class RandomRegionPositionEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float MinXOffset
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float MinYOffset
        {
            get => Get<float>();
            set => Set(value);
        }
        
        [SelfManagedProperty]
        public float MaxXOffset
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float MaxYOffset
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(MinXOffset), "Min X");
            InputFloat(nameof(MinYOffset), "Min Y");
            ImGui.NewLine();
            
            InputFloat(nameof(MaxXOffset), "Max X");
            InputFloat(nameof(MaxYOffset), "Max Y");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<RandomRegionPositionInitializer>()
                .First();

            MinXOffset = initializer.MinXOffset;
            MinYOffset = initializer.MinYOffset;
            MaxXOffset = initializer.MaxXOffset;
            MaxYOffset = initializer.MaxYOffset;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new RandomRegionPositionInitializer
            {
                MinXOffset = MinXOffset,
                MinYOffset = MinYOffset,
                MaxXOffset = MaxXOffset,
                MaxYOffset = MaxYOffset,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.Position, initializer));
        }
    }
}