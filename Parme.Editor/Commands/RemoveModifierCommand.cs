using System;
using System.Linq;
using Parme.Core;
using Parme.Core.Modifiers;

namespace Parme.Editor.Commands
{
    public class RemoveModifierCommand : ICommand
    {
        private readonly Type _modifierType;

        public RemoveModifierCommand(Type modifierType)
        {
            _modifierType = modifierType;
        }

        public void ApplyToEmitter(EmitterSettings settings)
        {
            settings.Modifiers = (settings.Modifiers ?? Array.Empty<IParticleModifier>())
                .Where(x => x.GetType() != _modifierType)
                .ToArray();
        }
    }
}