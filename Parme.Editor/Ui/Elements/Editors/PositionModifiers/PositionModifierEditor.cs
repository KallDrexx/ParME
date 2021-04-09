using System;
using Parme.Core.PositionModifier;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.PositionModifiers
{
    public class PositionModifierEditor : TypeSelectorEditor
    {
        public PositionModifierEditor() : base(new[] {typeof(AltitudeBouncePositionModifier)})
        {
        }

        protected override void UpdateSelectedTypeFromSettings()
        {
            SelectedType = EmitterSettings.PositionModifier?.GetType();
        }

        protected override void NewTypeSelected()
        {
            if (SelectedType == null)
            {
                CommandHandler.Execute(new UpdatePositionModifierCommand(null));
            }
            else
            {
                var modifier = (IParticlePositionModifier) Activator.CreateInstance(SelectedType);
                CommandHandler.Execute(new UpdatePositionModifierCommand(modifier));
            }
        }
    }
}