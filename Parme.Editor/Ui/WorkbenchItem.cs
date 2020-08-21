using System;
using Parme.Core.Initializers;

namespace Parme.Editor.Ui
{
    public readonly struct WorkbenchItem : IEquatable<WorkbenchItem>
    {
        public WorkbenchItemType ItemType { get; }
        public InitializerType? InitializerType { get; }
        public object Instance { get; }

        public WorkbenchItem(WorkbenchItemType type, InitializerType? initializerType, object instance)
        {
            ItemType = type;
            InitializerType = initializerType;
            Instance = instance;
        }

        public bool Equals(WorkbenchItem other)
        {
            return ItemType == other.ItemType && 
                   InitializerType == other.InitializerType && 
                   Equals(Instance, other.Instance);
        }

        public override bool Equals(object obj)
        {
            return obj is WorkbenchItem other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) ItemType, InitializerType, Instance);
        }
    }
}