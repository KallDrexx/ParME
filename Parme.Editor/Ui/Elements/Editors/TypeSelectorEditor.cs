using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private bool _ignoreNotifications;

        public SettingsEditorBase ChildDisplay { get; set; }
        
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
            
            ChildDisplay?.Render();
        }

        protected override void OnNewSettingsLoaded()
        {
            _ignoreNotifications = true;
            UpdateSelectedTypeFromSettings();
            ChildDisplay?.LoadNewSettings(EmitterSettings);
            _ignoreNotifications = false;
            
            OnSelfPropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedType)));
        }

        protected override void OnSelfPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreNotifications) return;

            switch (e.PropertyName)
            {
                case nameof(SelectedType):
                    if (SelectedType == null)
                    {
                        ChildDisplay = null;
                    }
                    else
                    {
                        var instance = (IEditorObject) Activator.CreateInstance(SelectedType);
                        var editor = EditorRetriever.GetEditor(instance);
                        editor.LoadNewSettings(EmitterSettings);

                        ChildDisplay = editor ?? throw new InvalidOperationException($"No editor known for type {SelectedType.FullName}");
                    }

                    break;
            }
        }

        protected abstract void UpdateSelectedTypeFromSettings();
    }
}