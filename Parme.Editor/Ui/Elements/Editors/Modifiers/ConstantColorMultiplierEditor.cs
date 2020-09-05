using System.Linq;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Modifiers
{
    public class ConstantColorMultiplierEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float RedChangePerSecond
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float GreenChangePerSecond
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float BlueChangePerSecond
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float AlphaChangePerSecond
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(RedChangePerSecond), "Red / Sec");
            InputFloat(nameof(GreenChangePerSecond), "Green / Sec");
            InputFloat(nameof(BlueChangePerSecond), "Blue / Sec");
            InputFloat(nameof(AlphaChangePerSecond), "Alpha / Sec");
        }

        protected override void OnNewSettingsLoaded()
        {
            var modifier = EmitterSettings.Modifiers
                .OfType<ConstantColorMultiplierChangeModifier>()
                .First();

            RedChangePerSecond = modifier.RedMultiplierChangePerSecond;
            GreenChangePerSecond = modifier.GreenMultiplierChangePerSecond;
            BlueChangePerSecond = modifier.BlueMultiplierChangePerSecond;
            AlphaChangePerSecond = modifier.AlphaMultiplierChangePerSecond;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var modifier = new ConstantColorMultiplierChangeModifier
            {
                RedMultiplierChangePerSecond = RedChangePerSecond,
                GreenMultiplierChangePerSecond = GreenChangePerSecond,
                BlueMultiplierChangePerSecond = BlueChangePerSecond,
                AlphaMultiplierChangePerSecond = AlphaChangePerSecond,
            };
            
            CommandHandler.Execute(new UpdateModifierCommand(modifier));
        }
    }
}