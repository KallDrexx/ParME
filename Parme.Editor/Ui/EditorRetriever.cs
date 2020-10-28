using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Editor.Ui.Elements.Editors;
using Parme.Editor.Ui.Elements.Editors.Initializers.ColorMultiplier;
using Parme.Editor.Ui.Elements.Editors.Initializers.ParticleCount;
using Parme.Editor.Ui.Elements.Editors.Initializers.Position;
using Parme.Editor.Ui.Elements.Editors.Initializers.RotationalOrientation;
using Parme.Editor.Ui.Elements.Editors.Initializers.RotationalVelocity;
using Parme.Editor.Ui.Elements.Editors.Initializers.Size;
using Parme.Editor.Ui.Elements.Editors.Initializers.TextureSectionIndex;
using Parme.Editor.Ui.Elements.Editors.Initializers.Velocity;
using Parme.Editor.Ui.Elements.Editors.Triggers;

namespace Parme.Editor.Ui
{
    public static class EditorRetriever
    {
        private static readonly Dictionary<Type, Type> TypeToEditorMap = GetEditorTypeMap();

        public static SettingsEditorBase GetEditor(EditorItem item)
        {
            return item.ItemType switch
            {
                EditorItemType.Trigger => new TriggerEditor(),
                EditorItemType.Lifetime => new ParticleLifetimeEditor(),
                EditorItemType.Initializer => GetEditorForInitializer(item),
                EditorItemType.ExistingModifier => GetEditorForModifier(item),
                EditorItemType.NewModifier => new AddModifierEditor(),
                EditorItemType.TextureFileName => new TextureFileEditor(),
                EditorItemType.TextureSection => new TextureSectionDisplayEditor(),
                _ => null
            };
        }

        public static SettingsEditorBase GetEditor(IEditorObject editableObject)
        {
            if (editableObject == null)
            {
                return null;
            }

            if (TypeToEditorMap.TryGetValue(editableObject.GetType(), out var editorType))
            {
                return (SettingsEditorBase) Activator.CreateInstance(editorType);
            }

            return null;
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
                
                case InitializerType.Size:
                    return new SizeEditor();
                
                case InitializerType.Velocity:
                    return new VelocityEditor();
                
                case InitializerType.TextureSectionIndex:
                    return new TextureSectionIndexEditor();
                
                case InitializerType.RotationalVelocity:
                    return new RotationalVelocityEditor();
                
                case InitializerType.RotationalOrientation:
                    return new RotationalOrientationEditor();

                case InitializerType.Unspecified:
                case null:
                    throw new ArgumentException("No initializer type specified");
                
                default:
                    return null;
            }
        }

        private static SettingsEditorBase GetEditorForModifier(EditorItem item)
        {
            return item.ModifierInstance != null
                ? GetEditor(item.ModifierInstance)
                : null;
        }

        private static Dictionary<Type, Type> GetEditorTypeMap()
        {
            return typeof(EditorRetriever)
                .Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(SettingsEditorBase).IsAssignableFrom(x))
                .Select(x => (EditorType: x, Attribute: x.GetCustomAttribute<EditorForTypeAttribute>()))
                .Where(x => x.Attribute != null)
                .ToDictionary(x => x.Attribute.Type, x => x.EditorType);
        }
    }
}