using System;
using System.Linq;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.TextureSectionIndex
{
    public class TextureSectionIndexEditor : TypeSelectorEditor
    {
        public TextureSectionIndexEditor() 
            : base(new[] {
                typeof(SingleTextureInitializer), 
                typeof(RandomTextureInitializer),
            })
        {
        }

        protected override void UpdateSelectedTypeFromSettings()
        {
            SelectedType = (EmitterSettings.Initializers ?? Array.Empty<IParticleInitializer>())
                .Where(x => x != null)
                .Where(x => x.InitializerType == InitializerType.TextureSectionIndex)
                .Select(x => x.GetType())
                .FirstOrDefault();
        }

        protected override void NewTypeSelected()
        {
            if (SelectedType == null)
            {
                CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.TextureSectionIndex, null));
            }
            else
            {
                var initializer = (IParticleInitializer) Activator.CreateInstance(SelectedType);
                CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.TextureSectionIndex, initializer));
            }
        }
    }
}