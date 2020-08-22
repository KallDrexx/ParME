using Parme.Core;

namespace Parme.Editor.Commands
{
    public class UpdateParticleLifetimeCommand : ICommand
    {
        private readonly float _newLifetime;

        public UpdateParticleLifetimeCommand(float newLifetime)
        {
            _newLifetime = newLifetime;
        }

        public void ApplyToEmitter(EmitterSettings settings)
        {
            settings.MaxParticleLifeTime = _newLifetime;
        }
    }
}