using System;
using System.Linq;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.RotationalVelocity
{
    public class RotationalVelocityEditor : TypeSelectorEditor
    {
        public RotationalVelocityEditor() 
            : base(new []
            {
                typeof(StaticRotationalVelocityInitializer),
                typeof(RandomRotationalVelocityInitializer),
            })
        {
        }

        protected override void UpdateSelectedTypeFromSettings()
        {
            SelectedType = (EmitterSettings.Initializers ?? Array.Empty<IParticleInitializer>())
                .Where(x => x != null)
                .Where(x => x.InitializerType == InitializerType.RotationalVelocity)
                .Select(x => x.GetType())
                .FirstOrDefault();
        }

        protected override void NewTypeSelected()
        {
            if (SelectedType == null)
            {
                CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.RotationalVelocity, null));
            }
            else
            {
                var initializer = (IParticleInitializer) Activator.CreateInstance(SelectedType);
                CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.RotationalVelocity, initializer));
            }
        }
    }
}