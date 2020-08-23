using System.Linq;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Size
{
    public class StaticSizeEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public int Width
        {
            get => Get<int>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public int Height
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputInt(nameof(Width), "Width");
            InputInt(nameof(Height), "Height");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<StaticSizeInitializer>()
                .First();

            Width = initializer.Width;
            Height = initializer.Height;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new StaticSizeInitializer
            {
                Width = Width,
                Height = Height,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.Size, initializer));
        }
    }
}