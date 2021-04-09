using System;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using ImGuiHandler;
using ImGuiHandler.MonoGame;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Editor.AppOperations;
using Parme.Editor.Commands;
using Parme.Editor.Ui.Elements;
using Parme.MonoGame;

namespace Parme.Editor.Ui
{
    public class EmitterSettingsController
    {
        private const float WorkbenchHeight = 315f;
        private const float MenuBarSize = 20f;

        private readonly SettingsCommandHandler _commandHandler;
        private readonly AppOperationQueue _appOperationQueue;
        private readonly Workbench _workbench;
        private readonly ActiveEditorWindow _activeEditorWindow;
        private readonly ApplicationState _applicationState;
        private readonly ITextureFileLoader _textureFileLoader;
        private readonly MonoGameImGuiRenderer _monoGameImGuiRenderer;
        private bool _ignoreChangeNotifications;
        private float _lastEmitterChangedAt = -1;
        
        public Vector2 EmitterVelocity => _workbench.EmitterVelocity;

        public EmitterSettingsController(ImGuiManager imGuiManager, 
            SettingsCommandHandler commandHandler, 
            ApplicationState applicationState, 
            AppOperationQueue appOperationQueue, 
            ITextureFileLoader textureFileLoader, 
            MonoGameImGuiRenderer monoGameImGuiRenderer)
        {
            _commandHandler = commandHandler;
            _applicationState = applicationState;
            _appOperationQueue = appOperationQueue;
            _textureFileLoader = textureFileLoader;
            _monoGameImGuiRenderer = monoGameImGuiRenderer;

            _workbench = new Workbench(commandHandler) {IsVisible = false};
            imGuiManager.AddElement(_workbench);
            
            _activeEditorWindow = new ActiveEditorWindow() {IsVisible = false};
            imGuiManager.AddElement(_activeEditorWindow);
            
            _workbench.PropertyChanged += WorkbenchOnPropertyChanged;
            _workbench.ModifierRemovalRequested += WorkbenchOnModifierRemovalRequested;
        }

        public void Update()
        {
            if (_applicationState.ActiveEmitter != null)
            {
                _workbench.IsVisible = true;
                _activeEditorWindow.IsVisible = true;

                if (_lastEmitterChangedAt < _applicationState.TimeLastEmitterUpdated)
                {
                    UpdateWorkbench(_applicationState.ActiveEmitter);
                    NewEditorItemSelected(_workbench.SelectedItem);
                    UpdateActiveEditor(_applicationState.ActiveEmitter);

                    _lastEmitterChangedAt = _applicationState.TimeLastEmitterUpdated;
                }
            }
            else
            {
                _workbench.IsVisible = false;
                _activeEditorWindow.IsVisible = false;
            }
        }

        public void ViewportResized(int width, int height)
        {
            _workbench.Position = new Vector2(0, MenuBarSize);
            _workbench.Size = new Vector2(width, WorkbenchHeight);
            
            _activeEditorWindow.Position = new Vector2(0, WorkbenchHeight + MenuBarSize);
            _activeEditorWindow.Size = new Vector2(300, height - WorkbenchHeight - MenuBarSize);
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
            if (!item.Equals(_activeEditorWindow.ItemBeingEdited))
            {
                _activeEditorWindow.ItemBeingEdited = item;

                _activeEditorWindow.Child?.Dispose();
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
        }

        private void UpdateWorkbench(EmitterSettings settings)
        {
            _ignoreChangeNotifications = true;

            _workbench.ParticleLifeTime = settings.MaxParticleLifeTime;
            _workbench.TextureFilename = settings.TextureFileName;
            _workbench.Trigger = settings.Trigger;
            _workbench.PositionModifier = settings.PositionModifier;

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

            _workbench.RotationalVelocityInitializer = (settings.Initializers ?? Array.Empty<IParticleInitializer>())
                .FirstOrDefault(x => x.InitializerType == InitializerType.RotationalVelocity);

            _workbench.RotationalOrientationInitializer = (settings.Initializers ?? Array.Empty<IParticleInitializer>())
                .FirstOrDefault(x => x.InitializerType == InitializerType.RotationalOrientation);

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
                _activeEditorWindow.Child.AppOperationQueue = _appOperationQueue;
                _activeEditorWindow.Child.ApplicationState = _applicationState;
                _activeEditorWindow.Child.TextureFileLoader = _textureFileLoader;
                _activeEditorWindow.Child.MonoGameImGuiRenderer = _monoGameImGuiRenderer;
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