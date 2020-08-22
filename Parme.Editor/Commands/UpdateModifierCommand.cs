using System;
using System.Linq;
using Parme.Core;
using Parme.Core.Modifiers;

namespace Parme.Editor.Commands
{
    public class UpdateModifierCommand : ICommand
    {
        private readonly IParticleModifier _modifier;

        public UpdateModifierCommand(IParticleModifier modifier)
        {
            _modifier = modifier ?? throw new ArgumentNullException(nameof(modifier));
        }

        public void ApplyToEmitter(EmitterSettings settings)
        {
            var modifiers = (settings.Modifiers ?? Array.Empty<IParticleModifier>()).ToList();
            
            // If a modifier of this type already exists, replace it with the
            // updated one without re-arranging
            var modifierReplaced = false;
            for (var x = 0; x < modifiers.Count; x++)
            {
                if (modifiers[x].GetType() == _modifier.GetType())
                {
                    modifiers[x] = _modifier;
                    modifierReplaced = true;
                }
            }

            if (!modifierReplaced)
            {
                // This is a new modifier
                modifiers.Add(_modifier);
            }

            settings.Modifiers = modifiers;
        }
    }
}