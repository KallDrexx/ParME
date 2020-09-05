using System.Linq;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Modifiers
{
    [EditorForType(typeof(ConstantAccelerationModifier))]
    public class ConstantAccelerationEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float XAcceleration
        {
            get => Get<float>();
            set => Set(value);
        }

        [SelfManagedProperty]
        public float YAcceleration
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(XAcceleration), "X Acceleration");
            InputFloat(nameof(YAcceleration), "Y Acceleration");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Modifiers
                .OfType<ConstantAccelerationModifier>()
                .First();

            XAcceleration = initializer.XAcceleration;
            YAcceleration = initializer.YAcceleration;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var modifier = new ConstantAccelerationModifier
            {
                XAcceleration = XAcceleration,
                YAcceleration = YAcceleration,
            };
            
            CommandHandler.Execute(new UpdateModifierCommand(modifier));
        }
    }
}