using System;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.PositionModifier;

namespace Parme.Editor.Ui
{
    public readonly struct EditorItem : IEquatable<EditorItem>
    {
        public EditorItemType ItemType { get; }
        public InitializerType? InitializerType { get; }
        public IParticleModifier ModifierInstance { get; }
        public IParticlePositionModifier PositionModifierInstance { get; }

        public EditorItem(EditorItemType type, InitializerType? initializerType)
        {
            ItemType = type;
            InitializerType = initializerType;
            ModifierInstance = null;
            PositionModifierInstance = null;
        }

        public EditorItem(IParticleModifier modifierModifierInstance)
        {
            ItemType = EditorItemType.ExistingModifier;
            InitializerType = null;
            ModifierInstance = modifierModifierInstance;
            PositionModifierInstance = null;
        }

        public EditorItem(IParticlePositionModifier positionModifierInstance)
        {
            ItemType = EditorItemType.PositionModifier;
            InitializerType = null;
            ModifierInstance = null;
            PositionModifierInstance = positionModifierInstance;
        }

        public bool Equals(EditorItem other)
        {
            return ItemType == other.ItemType && 
                   InitializerType == other.InitializerType && 
                   ModifierInstance?.GetType() == other.ModifierInstance?.GetType();
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