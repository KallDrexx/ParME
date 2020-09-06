using System;
using System.Linq;
using ImGuiNET;
using Parme.Core;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;

namespace Parme.Editor.Ui.Elements.Editors
{
    public class AddModifierEditor : SettingsEditorBase
    {
        private static readonly IParticleModifier[] AllModifiers = GetAllModifiers();

        private Type[] _modifierTypes = new Type[0];
        private string[] _modifierTypeNames = new string[0];
        private int _selectedModifierTypeIndex;

        protected override void CustomRender()
        {
            ImGui.Text("Add New Modifier");
            ImGui.Combo("Type",
                ref _selectedModifierTypeIndex,
                _modifierTypeNames,
                _modifierTypeNames.Length);

            if (_selectedModifierTypeIndex > 0)
            {
                ImGui.NewLine();
                if (ImGui.Button($"Add {_modifierTypeNames[_selectedModifierTypeIndex]}"))
                {
                    var modifierToAdd = _modifierTypes[_selectedModifierTypeIndex];
                    var instance = (IParticleModifier) Activator.CreateInstance(modifierToAdd);
                    var command = new UpdateModifierCommand(instance);
                    CommandHandler.Execute(command);
                }
            }
        }

        protected override void OnNewSettingsLoaded()
        {
            var activeModifiers = EmitterSettings.Modifiers
                .OfType<IEditorObject>()
                .ToArray();

            var selectableModifiers = new IParticleModifier[] {null}
                .Concat(AllModifiers.Where(x => activeModifiers.All(y => y.GetType() != x.GetType())))
                .ToArray();

            _modifierTypes = selectableModifiers.Select(x => x?.GetType()).ToArray();
            _modifierTypeNames = selectableModifiers.Select(x => x?.EditorShortName ?? "<None>").ToArray();
            _selectedModifierTypeIndex = 0;
        }

        protected override void OnSelfManagedPropertyChanged(string propertyName)
        {
            
        }

        private static IParticleModifier[] GetAllModifiers()
        {
            return typeof(IParticleModifier).Assembly
                .GetTypes()
                .Where(x => !x.IsInterface)
                .Where(x => !x.IsAbstract)
                .Where(x => typeof(IParticleModifier).IsAssignableFrom(x))
                .Select(x => (IParticleModifier) Activator.CreateInstance(x))
                .OrderBy(x => x.EditorShortName)
                .ToArray();
        }
    }
}