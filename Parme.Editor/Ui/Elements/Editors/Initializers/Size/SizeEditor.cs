using System;
using System.Linq;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.Size
{
    public class SizeEditor : TypeSelectorEditor
    {
        public SizeEditor() 
            : base(new []{typeof(StaticSizeInitializer), typeof(RandomSizeInitializer)})
        {
        }

        protected override void UpdateSelectedTypeFromSettings()
        {
            SelectedType = (EmitterSettings.Initializers ?? Array.Empty<IParticleInitializer>())
                .Where(x => x != null)
                .Where(x => x.InitializerType == InitializerType.Size)
                .Select(x => x.GetType())
                .FirstOrDefault();
        }

        protected override void NewTypeSelected()
        {
            if (SelectedType == null)
            {
                CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.Size, null));
            }
            else
            {
                var initializer = (IParticleInitializer) Activator.CreateInstance(SelectedType);
                CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.Size, initializer));
            }
        }
    }
}