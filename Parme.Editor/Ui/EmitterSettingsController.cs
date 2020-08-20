using System;
using System.ComponentModel;
using System.Numerics;
using ImGuiHandler;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Editor.Ui.Elements;

namespace Parme.Editor.Ui
{
    public class EmitterSettingsController
    {
        private const float WorkbenchHeight = 250f;
        
        private readonly Workbench _workbench;
        private bool _ignoreChangeNotifications;
        private EmitterSettings _currentSettings;

        public EmitterSettingsController(ImGuiManager imGuiManager)
        {
            _currentSettings = new EmitterSettings();
            
            _workbench = new Workbench();
            imGuiManager.AddElement(_workbench);
            
            _workbench.PropertyChanged += WorkbenchOnPropertyChanged;
        }

        public void ViewportResized(int width, int height)
        {
            _workbench.Position = new Vector2(0, height - WorkbenchHeight);
            _workbench.Size = new Vector2(width, WorkbenchHeight);
        }

        public void LoadNewSettings(EmitterSettings settings)
        {
            _ignoreChangeNotifications = true;

            _currentSettings = settings ?? new EmitterSettings();
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

        private void WorkbenchOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }
    }
}