using System;
using System.Linq;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.ColorMultiplier
{
    public class ColorMultiplierInitializerEditor : TypeSelectorEditor
    {
        public ColorMultiplierInitializerEditor() 
            : base(new[] {typeof(StaticColorInitializer)})
        {
        }

        protected override void UpdateSelectedTypeFromSettings()
        {
            SelectedType = (EmitterSettings.Initializers ?? Array.Empty<IParticleInitializer>())
                .Where(x => x != null)
                .Where(x => x.InitializerType == InitializerType.ColorMultiplier)
                .Select(x => x.GetType())
                .FirstOrDefault();
        }

        protected override void NewTypeSelected()
        {
            if (SelectedType == null)
            {
                CommandHandler.CommandPerformed(new UpdateInitializerCommand(InitializerType.ColorMultiplier, null));
            }
            else
            {
                var initializer = (IParticleInitializer) Activator.CreateInstance(SelectedType);
                CommandHandler.CommandPerformed(new UpdateInitializerCommand(InitializerType.ColorMultiplier, initializer));
            }
        }
    }
}