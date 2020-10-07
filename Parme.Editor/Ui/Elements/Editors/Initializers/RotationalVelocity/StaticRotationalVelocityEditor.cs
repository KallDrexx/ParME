using System.Linq;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.RotationalVelocity
{
    [EditorForType(typeof(StaticRotationalVelocityInitializer))]
    public class StaticRotationalVelocityEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public int DegreesPerSecond
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputInt(nameof(DegreesPerSecond), "Degrees Per Second");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Initializers
                .OfType<StaticRotationalVelocityInitializer>()
                .First();

            DegreesPerSecond = initializer.DegreesPerSecond;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var initializer = new StaticRotationalVelocityInitializer
            {
                DegreesPerSecond = DegreesPerSecond,
            };
            
            CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.RotationalVelocity, initializer));
        }
    }
}