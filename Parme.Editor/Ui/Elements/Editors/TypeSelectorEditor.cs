using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using Parme.Core;

namespace Parme.Editor.Ui.Elements.Editors
{
    public abstract class TypeSelectorEditor : SettingsEditorBase
    {
        private readonly IReadOnlyDictionary<string, Type> _typeMap;
        private readonly IReadOnlyDictionary<Type, int> _typeIndexMap;
        private readonly string[] _typeNames;
        private int _selectedIndex;
        private SettingsEditorBase _childDisplay;
        
        [SelfManagedProperty]
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

        public TypeSelectorEditor(IEnumerable<Type> types)
        {
            _typeMap = types.Where(x => typeof(IEditorObject).IsAssignableFrom(x))
                .Select(x => (IEditorObject) Activator.CreateInstance(x))
                .ToDictionary(x => x.EditorShortName, x => x.GetType());

            var orderedTypeNames = _typeMap.Keys.OrderBy(x => x).ToArray();
            _typeNames = new[] {"<None>"}
                .Union(orderedTypeNames)
                .ToArray();
            
            _typeIndexMap = orderedTypeNames
                .Select((str, index) => new {name = str, index = index, type = _typeMap[str]})
                .ToDictionary(x => x.type, x => x.index + 1);
        }
        
        protected override void CustomRender()
        {
            if (ImGui.Combo("Type", ref _selectedIndex, _typeNames, _typeNames.Length))
            {
                SelectedType = _typeMap.TryGetValue(_typeNames[_selectedIndex], out var type)
                    ? type
                    : null;
            }
            
            _childDisplay?.Render();
        }

        protected override void OnNewSettingsLoaded()
        {
            UpdateSelectedTypeFromSettings();
            SetChildDisplay();
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            NewTypeSelected();
            SetChildDisplay();
        }

        protected abstract void UpdateSelectedTypeFromSettings();

        protected abstract void NewTypeSelected();

        private void SetChildDisplay()
        {
            if (SelectedType == null)
            {
                _childDisplay = null;
            }
            else
            {
                var instance = (IEditorObject) Activator.CreateInstance(SelectedType);
                var editor = EditorRetriever.GetEditor(instance);

                _childDisplay = editor ?? throw new InvalidOperationException($"No editor known for type {SelectedType.FullName}");
                _childDisplay.CommandHandler = CommandHandler;
                _childDisplay.EmitterSettings = EmitterSettings;
            }
        }
    }
}