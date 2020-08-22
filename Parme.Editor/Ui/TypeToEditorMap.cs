using System;
using System.Collections.Generic;
using Parme.Core.Initializers;
using Parme.Core.Triggers;
using Parme.Editor.Ui.Elements.Editors.Initializers.ColorMultiplier;
using Parme.Editor.Ui.Elements.Editors.Initializers.ParticleCount;
using Parme.Editor.Ui.Elements.Editors.Initializers.Position;
using Parme.Editor.Ui.Elements.Editors.Initializers.Size;
using Parme.Editor.Ui.Elements.Editors.Initializers.Velocity;
using Parme.Editor.Ui.Elements.Editors.Triggers;

namespace Parme.Editor.Ui
{
    public class TypeToEditorMap
    {
        private readonly Dictionary<Type, Type> _map = new Dictionary<Type, Type>
        {
            {typeof(OneShotTrigger), typeof(OneShotTriggerEditor)},
            {typeof(TimeElapsedTrigger), typeof(TimeElapsedTriggerEditor)},
            {typeof(StaticColorInitializer), typeof(StaticColorMultiplierEditor)},
            {typeof(StaticParticleCountInitializer), typeof(StaticParticleCountEditor)},
            {typeof(RandomParticleCountInitializer), typeof(RandomParticleCountEditor)},
            {typeof(StaticPositionInitializer), typeof(StaticPositionEditor)},
            {typeof(RandomRegionPositionInitializer), typeof(RandomRegionPositionEditor)},
            {typeof(StaticSizeInitializer), typeof(StaticSizeEditor)},
            {typeof(RandomSizeInitializer), typeof(RandomSizeEditor)},
            {typeof(RandomRangeVelocityInitializer), typeof(RandomRangeVelocityEditor)},
        };
    }
}