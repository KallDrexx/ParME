using System.Linq;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Modifiers
{
    [EditorForType(typeof(ConstantRotationModifier))]
    public class ConstantRotationEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public int DegreesPerSecond
        {
            get => Get<int>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputInt(nameof(DegreesPerSecond), "Degrees");
        }

        protected override void OnNewSettingsLoaded()
        {
            var modifier = EmitterSettings.Modifiers
                .OfType<ConstantRotationModifier>()
                .First();

            DegreesPerSecond = (int)modifier.DegreesPerSecond;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var modifier = new ConstantRotationModifier
            {
                DegreesPerSecond = DegreesPerSecond,
            };
            
            CommandHandler.Execute(new UpdateModifierCommand(modifier));
        }
    }
}