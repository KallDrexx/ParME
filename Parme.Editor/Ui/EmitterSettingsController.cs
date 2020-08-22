using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using ImGuiHandler;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;
using Parme.Editor.Ui.Elements;
using Parme.Editor.Ui.Elements.Editors;
using Parme.Editor.Ui.Elements.Editors.Triggers;

namespace Parme.Editor.Ui
{
    public class EmitterSettingsController
    {
        private const float WorkbenchHeight = 250f;
        
        private readonly Workbench _workbench;
        private readonly ActiveEditorWindow _activeEditorWindow;
        private bool _ignoreChangeNotifications;
        private EmitterSettings _currentSettings;

        public EmitterSettingsController(ImGuiManager imGuiManager)
        {
            _currentSettings = new EmitterSettings();
            
            _workbench = new Workbench();
            imGuiManager.AddElement(_workbench);
            
            _activeEditorWindow = new ActiveEditorWindow();
            imGuiManager.AddElement(_activeEditorWindow);
            
            _workbench.PropertyChanged += WorkbenchOnPropertyChanged;
        }

        public void ViewportResized(int width, int height)
        {
            _workbench.Position = new Vector2(0, height - WorkbenchHeight);
            _workbench.Size = new Vector2(width, WorkbenchHeight);
            
            _activeEditorWindow.Position = new Vector2(0, 20);
            _activeEditorWindow.Size = new Vector2(300, height - WorkbenchHeight - 20);
        }

        public void LoadNewSettings(EmitterSettings settings)
        {
            _currentSettings = settings ?? new EmitterSettings();

            UpdateWorkbench();
        }

        private void WorkbenchOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            switch (e.PropertyName)
            {
                case nameof(Workbench.SelectedItem):
                    NewEditorItemSelected(_workbench.SelectedItem);
                    break;
            }
        }

        private void NewEditorItemSelected(EditorItem? item)
        {
            _activeEditorWindow.ItemBeingEdited = item;
            _activeEditorWindow.Children.Clear();

            switch (item?.ItemType)
            {
                case EditorItemType.Lifetime:
                    
                    break;

                case EditorItemType.Trigger:
                    
                    break;
            }
        }

        private void UpdateWorkbench()
        {
            _ignoreChangeNotifications = true;

            _workbench.ParticleLifeTime = _currentSettings.MaxParticleLifeTime;
            _workbench.TextureFilename = _currentSettings.TextureFileName;

            _workbench.TextureSections.Clear();
            foreach (var textureSection in _currentSettings.TextureSections ?? Array.Empty<TextureSectionCoords>())
            {
                _workbench.TextureSections.Add(textureSection);
            }

            foreach (var initializer in _currentSettings.Initializers ?? Array.Empty<IParticleInitializer>())
            {
                switch (initializer.InitializerType)
                {
                    case InitializerType.Position:
                        _workbench.PositionInitializer = initializer;
                        break;

                    case InitializerType.Size:
                        _workbench.SizeInitializer = initializer;
                        break;

                    case InitializerType.Velocity:
                        _workbench.VelocityInitializer = initializer;
                        break;

                    case InitializerType.ColorMultiplier:
                        _workbench.ColorMultiplierInitializer = initializer;
                        break;

                    case InitializerType.ParticleCount:
                        _workbench.ParticleCountInitializer = initializer;
                        break;

                    case InitializerType.TextureSectionIndex:
                        _workbench.TextureSectionInitializer = initializer;
                        break;
                }
            }

            _workbench.Modifiers.Clear();
            foreach (var modifier in _currentSettings.Modifiers ?? Array.Empty<IParticleModifier>())
            {
                _workbench.Modifiers.Add(modifier);
            }

            _ignoreChangeNotifications = false;
        }
    }
}