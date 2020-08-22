using Parme.Core;

namespace Parme.Editor.Commands
{
    public interface ICommand
    {
        void ApplyToEmitter(EmitterSettings settings);
    }
}