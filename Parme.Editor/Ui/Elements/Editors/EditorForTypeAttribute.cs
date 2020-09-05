using System;

namespace Parme.Editor.Ui.Elements.Editors
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EditorForTypeAttribute : Attribute
    {
        public Type Type { get; }
        
        public EditorForTypeAttribute(Type type)
        {
            Type = type;
        }
    }
}