using System;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;

namespace Parme.Editor.Ui
{
    public readonly struct EditorItem : IEquatable<EditorItem>
    {
        public EditorItemType ItemType { get; }
        public InitializerType? InitializerType { get; }
        public IParticleModifier ModifierInstance { get; }

        public EditorItem(EditorItemType type, InitializerType? initializerType)
        {
            ItemType = type;
            InitializerType = initializerType;
            ModifierInstance = null;
        }

        public EditorItem(IParticleModifier modifierModifierInstance)
        {
            ItemType = EditorItemType.ExistingModifier;
            InitializerType = null;
            ModifierInstance = modifierModifierInstance;
        }

        public bool Equals(EditorItem other)
        {
            return ItemType == other.ItemType && 
                   InitializerType == other.InitializerType && 
                   Equals(ModifierInstance, other.ModifierInstance);
        }

        public override bool Equals(object obj)
        {
            return obj is EditorItem other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) ItemType, InitializerType, ModifierInstance);
        }
    }
}