using System.Linq;
using ImGuiNET;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Size
{
    public class RandomSizeEditor : SettingsEditorBase
    {
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
        
        protected override void CustomRender()
        {
            InputInt(nameof(MinWidth), "Min Width");
            InputInt(nameof(MinHeight), "Min Height");
            
            ImGui.NewLine();
            
            InputInt(nameof(MaxWidth), "Max Width");
            InputInt(nameof(MaxHeight), "Max Height");
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
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new RandomSizeInitializer
            {
                MinWidth = MinWidth,
                MinHeight = MinHeight,
                MaxWidth = MaxWidth,
                MaxHeight = MaxHeight,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.Size, initializer));
        }
    }
}