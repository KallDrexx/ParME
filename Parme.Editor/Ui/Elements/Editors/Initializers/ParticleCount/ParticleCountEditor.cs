using System;
using System.Linq;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.ParticleCount
{
    public class ParticleCountEditor : TypeSelectorEditor
    {
        public ParticleCountEditor() 
            : base(new[] {typeof(RandomParticleCountInitializer), typeof(StaticParticleCountInitializer)})
        {
        }

        protected override void UpdateSelectedTypeFromSettings()
        {
            SelectedType = (EmitterSettings.Initializers ?? Array.Empty<IParticleInitializer>())
                .Where(x => x != null)
                .Where(x => x.InitializerType == InitializerType.ParticleCount)
                .Select(x => x.GetType())
                .FirstOrDefault();
        }

        protected override void NewTypeSelected()
        {
            if (SelectedType == null)
            {
                CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.ParticleCount, null));
            }
            else
            {
                var initializer = (IParticleInitializer) Activator.CreateInstance(SelectedType);
                CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.ParticleCount, initializer));
            }
        }
    }
}