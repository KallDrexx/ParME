using Parme.Core;
using Parme.Core.Triggers;

namespace Parme.Editor.Commands
{
    public class UpdateTriggerCommand : ICommand
    {
        private readonly IParticleTrigger _trigger;

        public UpdateTriggerCommand(IParticleTrigger trigger)
        {
            _trigger = trigger;
        }

        public void ApplyToEmitter(EmitterSettings settings)
        {
            settings.Trigger = _trigger;
        }
    }
}