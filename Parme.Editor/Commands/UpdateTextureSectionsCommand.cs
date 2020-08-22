using System.Collections.Generic;
using Parme.Core;

namespace Parme.Editor.Commands
{
    public class UpdateTextureSectionsCommand : ICommand
    {
        private readonly IReadOnlyList<TextureSectionCoords> _newSections;

        public UpdateTextureSectionsCommand(IReadOnlyList<TextureSectionCoords> newSections)
        {
            _newSections = newSections;
        }

        public void ApplyToEmitter(EmitterSettings settings)
        {
            settings.TextureSections = _newSections;
        }
    }
}