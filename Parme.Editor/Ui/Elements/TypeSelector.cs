using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiHandler;
using ImGuiNET;
using Parme.Core.Triggers;

namespace Parme.Editor.Ui.Elements
{
    public class TypeSelector : ImGuiElement
    {
        private readonly IReadOnlyDictionary<string, Type> _typeMap;
        private readonly IReadOnlyDictionary<Type, int> _typeIndexMap;
        private readonly string[] _typeNames;
        private int _selectedIndex;

        public ImGuiElement ChildDisplay { get; set; }

        public TypeSelector(IReadOnlyDictionary<string, Type> typeMap)
        {
            _typeMap = typeMap;

            var orderedTypeNames = typeMap.Keys.OrderBy(x => x).ToArray();
            _typeNames = new[] {"<None>"}.Union(orderedTypeNames).ToArray();

            _typeIndexMap = orderedTypeNames
                .Select((str, index) => new {name = str, index = index, type = typeMap[str]})
                .ToDictionary(x => x.type, x => x.index + 1);
        }

        public TypeSelector(IEnumerable<IParticleTrigger> triggers)
            : this(triggers
                .ToDictionary(x => x.EditorShortName, x => x.GetType()))
        {
        }

        public Type SelectedType
        {
            get => Get<Type>();
            set
            {
                Set(value);
                _selectedIndex = value != null
                    ? _typeIndexMap[value]
                    : 0;
            }
        }

        protected override void CustomRender()
        {
            if (ImGui.Combo("Type", ref _selectedIndex, _typeNames, _typeNames.Length))
            {
                SelectedType = _typeMap.TryGetValue(_typeNames[_selectedIndex], out var type)
                    ? type
                    : null;
            }
            
            ChildDisplay?.Render();
        }
    }
}