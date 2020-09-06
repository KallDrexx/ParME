using System;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using ImGuiHandler;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Editor.Commands;
using Parme.Editor.Ui.Elements;

namespace Parme.Editor.Ui
{
    public class EmitterSettingsController
    {
        private const float WorkbenchHeight = 300f;

        private readonly SettingsCommandHandler _commandHandler;
        private readonly Workbench _workbench;
        private readonly ActiveEditorWindow _activeEditorWindow;
        private bool _ignoreChangeNotifications;
        private bool _emitterChanged;

        public Vector3 BackgroundColor => _workbench.BackgroundColor;

        public EmitterSettingsController(ImGuiManager imGuiManager, SettingsCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
            
            _workbench = new Workbench(commandHandler);
            imGuiManager.AddElement(_workbench);
            
            _activeEditorWindow = new ActiveEditorWindow();
            imGuiManager.AddElement(_activeEditorWindow);
            
            _workbench.PropertyChanged += WorkbenchOnPropertyChanged;
            _workbench.ModifierRemovalRequested += WorkbenchOnModifierRemovalRequested;
            _commandHandler.EmitterUpdated += (sender, settings) => _emitterChanged = true;
        }

        public void Update()
        {
            if (_emitterChanged)
            {
                var settings = _commandHandler.GetCurrentSettings();
                UpdateWorkbench(settings);
                NewEditorItemSelected(_workbench.SelectedItem);
                UpdateActiveEditor(settings);
                
                _emitterChanged = false;
            }
        }

        public void ViewportResized(int width, int height)
        {
            _workbench.Position = new Vector2(0, 20);
            _workbench.Size = new Vector2(width, WorkbenchHeight);
            
            _activeEditorWindow.Position = new Vector2(0, WorkbenchHeight + 20);
            _activeEditorWindow.Size = new Vector2(300, height - WorkbenchHeight - 20);
        }

        public void LoadNewSettings(EmitterSettings settings)
        {
            _commandHandler.NewStartingEmitter(settings);

            UpdateWorkbench(settings);
        }

        private void WorkbenchOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            switch (e.PropertyName)
            {
                case nameof(Workbench.SelectedItem):
                    NewEditorItemSelected(_workbench.SelectedItem);
                    UpdateActiveEditor(_commandHandler.GetCurrentSettings());
                    break;
            }
        }

        private void NewEditorItemSelected(EditorItem? item)
        {
            _activeEditorWindow.ItemBeingEdited = item;
            _activeEditorWindow.Child = null;

            if (item != null)
            {
                var editor = EditorRetriever.GetEditor(item.Value);
                if (editor != null)
                {
                    _activeEditorWindow.Child = editor;
                }
            }
        }

        private void UpdateWorkbench(EmitterSettings settings)
        {
            _ignoreChangeNotifications = true;

            _workbench.ParticleLifeTime = settings.MaxParticleLifeTime;
            _workbench.TextureFilename = settings.TextureFileName;
            _workbench.Trigger = settings.Trigger;

            _workbench.TextureSections.Clear();
            foreach (var textureSection in settings.TextureSections ?? Array.Empty<TextureSectionCoords>())
            {
                _workbench.TextureSections.Add(textureSection);
            }

            _workbench.PositionInitializer = (settings.Initializers ?? Array.Empty<IParticleInitializer>())
                .FirstOrDefault(x => x.InitializerType == InitializerType.Position);
            
            _workbench.SizeInitializer = (settings.Initializers ?? Array.Empty<IParticleInitializer>())
                .FirstOrDefault(x => x.InitializerType == InitializerType.Size);
            
            _workbench.VelocityInitializer = (settings.Initializers ?? Array.Empty<IParticleInitializer>())
                .FirstOrDefault(x => x.InitializerType == InitializerType.Velocity);
            
            _workbench.ColorMultiplierInitializer = (settings.Initializers ?? Array.Empty<IParticleInitializer>())
                .FirstOrDefault(x => x.InitializerType == InitializerType.ColorMultiplier);
            
            _workbench.ParticleCountInitializer = (settings.Initializers ?? Array.Empty<IParticleInitializer>())
                .FirstOrDefault(x => x.InitializerType == InitializerType.ParticleCount);
            
            _workbench.TextureSectionInitializer = (settings.Initializers ?? Array.Empty<IParticleInitializer>())
                .FirstOrDefault(x => x.InitializerType == InitializerType.TextureSectionIndex);

            _workbench.Modifiers.Clear();
            foreach (var modifier in settings.Modifiers ?? Array.Empty<IParticleModifier>())
            {
                _workbench.Modifiers.Add(modifier);
            }
            
            // If the selected item is an item that no longer exists, make sure nothing is selected
            if (_workbench.SelectedItem?.ModifierInstance != null)
            {
                var modifierType = _workbench.SelectedItem.Value.ModifierInstance.GetType();
                if (settings.Modifiers?.Any(x => x.GetType() == modifierType) != true)
                {
                    _workbench.SelectedItem = null;
                }
            }

            _ignoreChangeNotifications = false;
        }

        private void UpdateActiveEditor(EmitterSettings settings)
        {
            if (_activeEditorWindow.Child != null)
            {
                _activeEditorWindow.Child.CommandHandler = _commandHandler; // Must be first
                _activeEditorWindow.Child.EmitterSettings = settings;
            }
        }

        private void WorkbenchOnModifierRemovalRequested(object sender, IParticleModifier e)
        {
            var command = new RemoveModifierCommand(e.GetType());
            _commandHandler.Execute(command);
        }
    }
}