using Parme.Core;
using Parme.Core.PositionModifier;

namespace Parme.Editor.Commands
{
    public class UpdatePositionModifierCommand : ICommand
    {
        private readonly IParticlePositionModifier _modifier;

        public UpdatePositionModifierCommand(IParticlePositionModifier modifier)
        {
            _modifier = modifier;
        }

        public void ApplyToEmitter(EmitterSettings settings)
        {
            settings.PositionModifier = _modifier;
        }
    }
}