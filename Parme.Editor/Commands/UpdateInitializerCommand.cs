using System;
using System.Linq;
using Parme.Core;
using Parme.Core.Initializers;

namespace Parme.Editor.Commands
{
    public class UpdateInitializerCommand : ICommand
    {
        private readonly InitializerType _initializerType;
        private readonly IParticleInitializer _updatedInstance;
        
        public UpdateInitializerCommand(InitializerType initializerType, IParticleInitializer updatedInstance)
        {
            _initializerType = initializerType;
            _updatedInstance = updatedInstance;
        }

        public void ApplyToEmitter(EmitterSettings settings)
        {
            settings.Initializers = (settings.Initializers ?? Array.Empty<IParticleInitializer>())
                .Where(x => x.InitializerType != _initializerType)
                .Union(new[] {_updatedInstance})
                .Where(x => x != null)
                .ToArray();
        }
    }
}