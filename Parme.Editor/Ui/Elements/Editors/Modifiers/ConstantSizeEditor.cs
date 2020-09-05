using System.Linq;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Modifiers
{
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
            InputFloat(nameof(WidthChangePerSecond), "Width / Sec");
            InputFloat(nameof(HeightChangePerSecond), "Height / Sec");
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