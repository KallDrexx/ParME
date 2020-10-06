using System.Linq;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Modifiers
{
    [EditorForType(typeof(DragModifier))]
    public class DragEditor : SettingsEditorBase
    {
        [SelfManagedProperty]
        public float DragFactor
        {
            get => Get<float>();
            set => Set(value);
        }
        
        protected override void CustomRender()
        {
            InputFloat(nameof(DragFactor), "Drag");
        }

        protected override void OnNewSettingsLoaded()
        {
            var initializer = EmitterSettings.Modifiers
                .OfType<DragModifier>()
                .First();

            DragFactor = initializer.DragFactor;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            var modifier = new DragModifier
            {
                DragFactor = DragFactor,
            };
            
            CommandHandler.Execute(new UpdateModifierCommand(modifier));
        }
    }
}