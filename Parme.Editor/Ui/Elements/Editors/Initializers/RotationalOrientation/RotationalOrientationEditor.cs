﻿using System;
using System.Linq;
using Parme.Core.Initializers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors.Initializers.RotationalOrientation
{
    public class RotationalOrientationEditor : TypeSelectorEditor
    {
        public RotationalOrientationEditor() : 
            base(new[]
            {
                typeof(RandomRotationInitializer),
            })
        {
        }

        protected override void UpdateSelectedTypeFromSettings()
        {
            SelectedType = (EmitterSettings.Initializers ?? Array.Empty<IParticleInitializer>())
                .Where(x => x != null)
                .Where(x => x.InitializerType == InitializerType.RotationalOrientation)
                .Select(x => x.GetType())
                .FirstOrDefault();
        }

        protected override void NewTypeSelected()
        {
            if (SelectedType == null)
            {
                CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.RotationalOrientation, null));
            }
            else
            {
                var initializer = (IParticleInitializer) Activator.CreateInstance(SelectedType);
                CommandHandler.Execute(new UpdateInitializerCommand(InitializerType.RotationalOrientation, initializer));
            }
        }
    }
}