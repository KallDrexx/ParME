using Parme.Core;

namespace Parme.Editor.Commands
{
    public class UpdateTextureFileNameCommand : ICommand
    {
        private readonly string _newFileName;

        public UpdateTextureFileNameCommand(string newFileName)
        {
            _newFileName = newFileName;
        }

        public void ApplyToEmitter(EmitterSettings settings)
        {
            settings.TextureFileName = _newFileName;
        }
    }
}