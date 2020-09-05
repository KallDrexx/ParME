using System.Linq;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Position
{
    [EditorForType(typeof(StaticPositionInitializer))]
    public class StaticPositionEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float XOffset
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float YOffset
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(XOffset), "X Offset");
            InputFloat(nameof(YOffset), "Y Offset");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<StaticPositionInitializer>()
                .First();

            XOffset = initializer.XOffset;
            YOffset = initializer.YOffset;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new StaticPositionInitializer
            {
                XOffset = XOffset,
                YOffset = YOffset,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.Position, initializer));
        }
    }
}