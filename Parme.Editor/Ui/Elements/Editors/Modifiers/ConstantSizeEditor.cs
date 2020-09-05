using System.Linq;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Modifiers
{
    [EditorForType(typeof(ConstantSizeModifier))]
    public class ConstantSizeEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float WidthChangePerSecond
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float HeightChangePerSecond
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(WidthChangePerSecond), "Width");
            InputFloat(nameof(HeightChangePerSecond), "Height");
        }

        protected override void OnNewSettingsLoaded()
        {
            var modifier = EmitterSettings.Modifiers
                .OfType<ConstantSizeModifier>()
                .First();

            WidthChangePerSecond = modifier.WidthChangePerSecond;
            HeightChangePerSecond = modifier.HeightChangePerSecond;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var modifier = new ConstantSizeModifier
            {
                WidthChangePerSecond = WidthChangePerSecond,
                HeightChangePerSecond = HeightChangePerSecond,
            };
            
            CommandHandler.Execute(new UpdateModifierCommand(modifier));
        }
    }
}