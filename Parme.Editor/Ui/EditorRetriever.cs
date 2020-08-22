using System;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Triggers;
using Parme.Editor.Ui.Elements.Editors;
using Parme.Editor.Ui.Elements.Editors.Initializers.ColorMultiplier;
using Parme.Editor.Ui.Elements.Editors.Initializers.ParticleCount;
using Parme.Editor.Ui.Elements.Editors.Initializers.Position;
using Parme.Editor.Ui.Elements.Editors.Triggers;

namespace Parme.Editor.Ui
{
    public static class EditorRetriever
    {
        public static SettingsEditorBase GetEditor(EditorItem item)
        {
            return item.ItemType switch
            {
                EditorItemType.Trigger => new TriggerEditor(),
                EditorItemType.Lifetime => new ParticleLifetimeEditor(),
                EditorItemType.Initializer => GetEditorForInitializer(item),
                _ => null
            };
        }

        public static SettingsEditorBase GetEditor(IEditorObject editableObject)
        {
            return editableObject?.GetType().Name switch
            {
                nameof(OneShotTrigger) => new OneShotTriggerEditor(),
                nameof(TimeElapsedTrigger) => new TimeElapsedTriggerEditor(),
                nameof(StaticColorInitializer) => new StaticColorMultiplierEditor(),
                nameof(RandomParticleCountInitializer) => new RandomParticleCountEditor(),
                nameof(StaticParticleCountInitializer) => new StaticParticleCountEditor(),
                nameof(StaticPositionInitializer) => new StaticPositionEditor(),
                nameof(RandomRegionPositionInitializer) => new RandomRegionPositionEditor(),
                _ => null
            };
        }

        private static SettingsEditorBase GetEditorForInitializer(EditorItem item)
        {
            if (item.ItemType != EditorItemType.Initializer)
            {
                throw new ArgumentException("Item supplied isn't an initializer");
            }

            switch (item.InitializerType)
            {
                case InitializerType.ColorMultiplier:
                    return new ColorMultiplierInitializerEditor();
                
                case InitializerType.ParticleCount:
                    return new ParticleCountEditor();
                
                case InitializerType.Position:
                    return new PositionEditor();
                
                case InitializerType.Unspecified:
                case null:
                    throw new ArgumentException("No initializer type specified");
                
                default:
                    return null;
            }
        }
    }
}