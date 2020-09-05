using System.Linq;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Modifiers
{
    [EditorForType(typeof(ConstantColorMultiplierChangeModifier))]
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
            InputFloat(nameof(RedChangePerSecond), "Red");
            InputFloat(nameof(GreenChangePerSecond), "Green");
            InputFloat(nameof(BlueChangePerSecond), "Blue");
            InputFloat(nameof(AlphaChangePerSecond), "Alpha");
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