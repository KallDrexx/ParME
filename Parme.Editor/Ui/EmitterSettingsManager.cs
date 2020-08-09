using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;
using Parme.CSharp;
using Parme.Editor.Ui.Elements;
using Parme.Editor.Ui.Elements.Initializers.ColorMultiplier;
using Parme.Editor.Ui.Elements.Initializers.ParticleCount;
using Parme.Editor.Ui.Elements.Initializers.Position;
using Parme.Editor.Ui.Elements.Initializers.Size;
using Parme.Editor.Ui.Elements.Triggers;

namespace Parme.Editor.Ui
{
    public class EmitterSettingsManager
    {
        private readonly TypeSelector _triggerParentSection;
        private readonly OneShotTriggerEditor _oneShotTriggerEditor;
        private readonly TimeElapsedTriggerEditor _timeElapsedTriggerEditor;
        
        private readonly TypeSelector _particleCountSelector;
        private readonly StaticParticleCountEditor _staticParticleCountEditor;
        private readonly RandomParticleCountEditor _randomParticleCountEditor;
        
        private readonly TypeSelector _colorMultiplierSelector;
        private readonly StaticColorMultiplierEditor _staticColorMultiplierEditor;

        private readonly TypeSelector _positionSelector;
        private readonly StaticPositionEditor _staticPositionEditor;
        private readonly RandomRegionPositionEditor _randomRegionPositionEditor;

        private readonly TypeSelector _sizeSelector;
        private readonly StaticSizeEditor _staticSizeEditor;
        private readonly RandomSizeEditor _randomSizeEditor;
        
        private readonly List<IParticleModifier> _modifiers = new List<IParticleModifier>();
        private float _particleMaxLife;
        private IParticleTrigger _particleTrigger;
        private Dictionary<InitializerType, IParticleInitializer> _initializers = new Dictionary<InitializerType, IParticleInitializer>();
            
        private bool _ignoreChangeNotifications;

        public event EventHandler<EmitterSettings> EmitterSettingsChanged; 

        public EmitterSettingsManager(MainSidePanel mainSidePanel)
        {
            var triggerTypes = new Dictionary<string, Type>
            {
                {"One Shot", typeof(OneShotTrigger)},
                {"Time Elapsed", typeof(TimeElapsedTrigger)}
            };

            var particleCountTypes = new Dictionary<string, Type>
            {
                {"Static", typeof(StaticParticleCountInitializer)},
                {"Random", typeof(RandomParticleCountInitializer)},
            };

            var colorMultiplierTypes = new Dictionary<string, Type>
            {
                {"Static", typeof(StaticColorInitializer)}
            };
            
            var positionTypes = new Dictionary<string, Type>
            {
                {"Static", typeof(StaticPositionInitializer)},
                {"Random Within Region", typeof(RandomRegionPositionInitializer)},
            };

            var sizeTypes = new Dictionary<string, Type>
            {
                {"Static", typeof(StaticSizeInitializer)},
                {"Random", typeof(RandomSizeInitializer)},
            };
            
            _triggerParentSection = new TypeSelector(triggerTypes){IsVisible = true};
            _oneShotTriggerEditor = new OneShotTriggerEditor{IsVisible = true};
            _timeElapsedTriggerEditor = new TimeElapsedTriggerEditor {IsVisible = true};
            
            _particleCountSelector = new TypeSelector(particleCountTypes) {IsVisible = true};
            _staticParticleCountEditor = new StaticParticleCountEditor {IsVisible = true};
            _randomParticleCountEditor = new RandomParticleCountEditor {IsVisible = true};
            
            _colorMultiplierSelector = new TypeSelector(colorMultiplierTypes) {IsVisible = true};
            _staticColorMultiplierEditor = new StaticColorMultiplierEditor {IsVisible = true};
            
            _positionSelector = new TypeSelector(positionTypes) {IsVisible = true};
            _staticPositionEditor = new StaticPositionEditor {IsVisible = true};
            _randomRegionPositionEditor = new RandomRegionPositionEditor {IsVisible = true};
            
            _sizeSelector = new TypeSelector(sizeTypes) {IsVisible = true};
            _staticSizeEditor = new StaticSizeEditor {IsVisible = true};
            _randomSizeEditor = new RandomSizeEditor {IsVisible = true};

            mainSidePanel.TriggerParentSection = _triggerParentSection;
            mainSidePanel.ParticleCountSelector = _particleCountSelector;
            mainSidePanel.ColorMultiplierSelector = _colorMultiplierSelector;
            mainSidePanel.PositionSelector = _positionSelector;
            mainSidePanel.SizeSelector = _sizeSelector;
            
            _triggerParentSection.PropertyChanged += TriggerParentSectionOnPropertyChanged;
            _timeElapsedTriggerEditor.PropertyChanged += TimeElapsedTriggerEditorOnPropertyChanged;
            
            _particleCountSelector.PropertyChanged += ParticleCountSelectorOnPropertyChanged;
            _staticParticleCountEditor.PropertyChanged += StaticParticleCountEditorOnPropertyChanged;
            _randomParticleCountEditor.PropertyChanged += RandomParticleCountEditorOnPropertyChanged;
            
            _colorMultiplierSelector.PropertyChanged += ColorMultiplierSelectorOnPropertyChanged;
            _staticColorMultiplierEditor.PropertyChanged += StaticColorMultiplierEditorOnPropertyChanged;
            
            _positionSelector.PropertyChanged += PositionSelectorOnPropertyChanged;
            _staticPositionEditor.PropertyChanged += StaticPositionEditorOnPropertyChanged;
            _randomRegionPositionEditor.PropertyChanged += RandomRegionPositionEditorOnPropertyChanged;
            
            _sizeSelector.PropertyChanged += SizeSelectorOnPropertyChanged;
            _staticSizeEditor.PropertyChanged += StaticSizeEditorOnPropertyChanged;
            _randomSizeEditor.PropertyChanged += RandomSizeEditorOnPropertyChanged;
        }

        public void NewEmitterSettingsLoaded(EmitterSettings settings)
        {
            _modifiers.Clear();
            _initializers.Clear();
            _particleTrigger = null;
            _particleMaxLife = 0f;

            if (settings != null)
            {
                _particleMaxLife = settings.MaxParticleLifeTime;
                _particleTrigger = settings.Trigger;
                _initializers = settings.Initializers
                    .GroupBy(x => x.InitializerType)
                    .Select(x => new {type = x.Key, initializer = x.FirstOrDefault()})
                    .Where(x => x.initializer != null)
                    .ToDictionary(x => x.type, x => x.initializer);
                
                _modifiers.AddRange(settings.Modifiers);
            }
            
            UpdateUi();
        }

        private void UpdateUi()
        {
            _ignoreChangeNotifications = true;

            _triggerParentSection.SelectedType = _particleTrigger?.GetType();
            switch (_particleTrigger?.GetType().Name)
            {
                case nameof(OneShotTrigger):
                    _triggerParentSection.ChildDisplay = _oneShotTriggerEditor;
                    break;
                
                case nameof(TimeElapsedTrigger):
                    _triggerParentSection.ChildDisplay = _timeElapsedTriggerEditor;
                    _timeElapsedTriggerEditor.Frequency = ((TimeElapsedTrigger) _particleTrigger).Frequency;
                    break;
                
                default:
                    _triggerParentSection.ChildDisplay = null;
                    break;
            }

            _initializers.TryGetValue(InitializerType.ParticleCount, out var countInitializer);
            _particleCountSelector.SelectedType = countInitializer?.GetType();
            switch (countInitializer?.GetType().Name)
            {
                case nameof(StaticParticleCountInitializer):
                    _particleCountSelector.ChildDisplay = _staticParticleCountEditor;
                    _staticParticleCountEditor.ParticleSpawnCount = ((StaticParticleCountInitializer)countInitializer).ParticleSpawnCount;
                    break;
                
                case nameof(RandomParticleCountInitializer):
                    _particleCountSelector.ChildDisplay = _randomParticleCountEditor;
                    _randomParticleCountEditor.MinSpawnCount = ((RandomParticleCountInitializer)countInitializer).MinimumToSpawn;
                    _randomParticleCountEditor.MaxSpawnCount = ((RandomParticleCountInitializer)countInitializer).MaximumToSpawn;
                    break;
                
                default:
                    _particleCountSelector.ChildDisplay = null;
                    break;
            }

            _initializers.TryGetValue(InitializerType.ColorMultiplier, out var colorInitializer);
            _colorMultiplierSelector.SelectedType = colorInitializer?.GetType();
            switch (colorInitializer?.GetType().Name)
            {
                case nameof(StaticColorInitializer):
                    _colorMultiplierSelector.ChildDisplay = _staticColorMultiplierEditor;
                    _staticColorMultiplierEditor.RedMultiplier = ((StaticColorInitializer) colorInitializer).RedMultiplier;
                    _staticColorMultiplierEditor.GreenMultiplier = ((StaticColorInitializer) colorInitializer).GreenMultiplier;
                    _staticColorMultiplierEditor.BlueMultiplier = ((StaticColorInitializer) colorInitializer).BlueMultiplier;
                    _staticColorMultiplierEditor.AlphaMultiplier = ((StaticColorInitializer) colorInitializer).AlphaMultiplier;
                    break;
                
                default:
                    _colorMultiplierSelector.ChildDisplay = null;
                    break;
            }

            _initializers.TryGetValue(InitializerType.Position, out var positionInitializer);
            _positionSelector.SelectedType = positionInitializer?.GetType();
            switch (positionInitializer?.GetType().Name)
            {
                case nameof(StaticPositionInitializer):
                    _positionSelector.ChildDisplay = _staticPositionEditor;
                    _staticPositionEditor.XOffset = ((StaticPositionInitializer) positionInitializer).XOffset;
                    _staticPositionEditor.YOffset = ((StaticPositionInitializer) positionInitializer).YOffset;
                    break;
                
                case nameof(RandomRegionPositionInitializer):
                    _positionSelector.ChildDisplay = _randomRegionPositionEditor;
                    _randomRegionPositionEditor.MinXOffset = ((RandomRegionPositionInitializer) positionInitializer).MinXOffset;
                    _randomRegionPositionEditor.MinYOffset = ((RandomRegionPositionInitializer) positionInitializer).MinYOffset;
                    _randomRegionPositionEditor.MaxXOffset = ((RandomRegionPositionInitializer) positionInitializer).MaxXOffset;
                    _randomRegionPositionEditor.MaxYOffset = ((RandomRegionPositionInitializer) positionInitializer).MaxYOffset;
                    break;
                
                default:
                    _positionSelector.ChildDisplay = null;
                    break;
            }

            _initializers.TryGetValue(InitializerType.Size, out var sizeInitializer);
            _sizeSelector.SelectedType = sizeInitializer?.GetType();
            switch (sizeInitializer?.GetType().Name)
            {
                case nameof(StaticSizeInitializer):
                    _sizeSelector.ChildDisplay = _staticSizeEditor;
                    _staticSizeEditor.Width = ((StaticSizeInitializer) sizeInitializer).Width;
                    _staticSizeEditor.Height = ((StaticSizeInitializer) sizeInitializer).Height;
                    break;
                
                case nameof(RandomSizeInitializer):
                    _sizeSelector.ChildDisplay = _randomSizeEditor;
                    _randomSizeEditor.MinWidth = ((RandomSizeInitializer) sizeInitializer).MinWidth;
                    _randomSizeEditor.MinHeight = ((RandomSizeInitializer) sizeInitializer).MinHeight;
                    _randomSizeEditor.MaxWidth = ((RandomSizeInitializer) sizeInitializer).MaxWidth;
                    _randomSizeEditor.MaxHeight = ((RandomSizeInitializer) sizeInitializer).MaxHeight;
                    break;
                
                default:
                    _sizeSelector.ChildDisplay = null;
                    break;
            }

            _ignoreChangeNotifications = false;
        }

        private void TriggerParentSectionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;
            
            if (e.PropertyName == nameof(TypeSelector.SelectedType))
            {
                var trigger = _triggerParentSection.SelectedType != null
                    ? (IParticleTrigger) Activator.CreateInstance(_triggerParentSection.SelectedType)
                    : null;

                _particleTrigger = trigger;
            }

            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void TimeElapsedTriggerEditorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            var trigger = (TimeElapsedTrigger) _particleTrigger;
            if (e.PropertyName == nameof(_timeElapsedTriggerEditor.Frequency))
            {
                trigger.Frequency = _timeElapsedTriggerEditor.Frequency;
            }

            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void ParticleCountSelectorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;
            
            if (e.PropertyName == nameof(TypeSelector.SelectedType))
            {
                var initializer = _particleCountSelector.SelectedType != null
                    ? (IParticleInitializer) Activator.CreateInstance(_particleCountSelector.SelectedType)
                    : null;

                _initializers[InitializerType.ParticleCount] = initializer;
            }

            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void RandomParticleCountEditorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            var initializer = (RandomParticleCountInitializer) _initializers[InitializerType.ParticleCount];
            switch (e.PropertyName)
            {
                case nameof(RandomParticleCountEditor.MinSpawnCount):
                    initializer.MinimumToSpawn = _randomParticleCountEditor.MinSpawnCount;
                    break;
                
                case nameof(RandomParticleCountEditor.MaxSpawnCount):
                    initializer.MaximumToSpawn = _randomParticleCountEditor.MaxSpawnCount;
                    break;
            }
            
            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void StaticParticleCountEditorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            var initializer = (StaticParticleCountInitializer) _initializers[InitializerType.ParticleCount];
            switch (e.PropertyName)
            {
                case nameof(StaticParticleCountEditor.ParticleSpawnCount):
                    initializer.ParticleSpawnCount = _staticParticleCountEditor.ParticleSpawnCount;
                    break;
            }
            
            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void ColorMultiplierSelectorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;
            
            if (e.PropertyName == nameof(TypeSelector.SelectedType))
            {
                var initializer = _colorMultiplierSelector.SelectedType != null
                    ? (IParticleInitializer) Activator.CreateInstance(_colorMultiplierSelector.SelectedType)
                    : null;

                _initializers[InitializerType.ColorMultiplier] = initializer;
            }
            
            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void StaticColorMultiplierEditorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            var initializer = (StaticColorInitializer) _initializers[InitializerType.ColorMultiplier];
            switch (e.PropertyName)
            {
                case nameof(StaticColorMultiplierEditor.RedMultiplier):
                    initializer.RedMultiplier = _staticColorMultiplierEditor.RedMultiplier;
                    break;
                
                case nameof(StaticColorMultiplierEditor.GreenMultiplier):
                    initializer.GreenMultiplier = _staticColorMultiplierEditor.GreenMultiplier;
                    break;
                
                case nameof(StaticColorMultiplierEditor.BlueMultiplier):
                    initializer.BlueMultiplier = _staticColorMultiplierEditor.BlueMultiplier;
                    break;
                
                case nameof(StaticColorMultiplierEditor.AlphaMultiplier):
                    initializer.AlphaMultiplier = _staticColorMultiplierEditor.AlphaMultiplier;
                    break;
            }
            
            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void PositionSelectorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            if (e.PropertyName == nameof(TypeSelector.SelectedType))
            {
                var initializer = _positionSelector.SelectedType != null
                    ? (IParticleInitializer) Activator.CreateInstance(_positionSelector.SelectedType)
                    : null;

                _initializers[InitializerType.Position] = initializer;
            }
            
            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void StaticPositionEditorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            var initializer = (StaticPositionInitializer) _initializers[InitializerType.Position];
            switch (e.PropertyName)
            {
                case nameof(StaticPositionEditor.XOffset):
                    initializer.XOffset = _staticPositionEditor.XOffset;
                    break;
                
                case nameof(StaticPositionEditor.YOffset):
                    initializer.YOffset = _staticPositionEditor.YOffset;
                    break;
            }
            
            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void RandomRegionPositionEditorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;
            
            var initializer = (RandomRegionPositionInitializer) _initializers[InitializerType.Position];
            switch (e.PropertyName)
            {
                case nameof(RandomRegionPositionEditor.MinXOffset):
                    initializer.MinXOffset = _randomRegionPositionEditor.MinXOffset;
                    break;
                
                case nameof(RandomRegionPositionEditor.MinYOffset):
                    initializer.MinYOffset = _randomRegionPositionEditor.MinYOffset;
                    break;
                
                case nameof(RandomRegionPositionEditor.MaxXOffset):
                    initializer.MaxXOffset = _randomRegionPositionEditor.MaxXOffset;
                    break;
                
                case nameof(RandomRegionPositionEditor.MaxYOffset):
                    initializer.MaxYOffset = _randomRegionPositionEditor.MaxYOffset;
                    break;
            }
            
            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void SizeSelectorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            if (e.PropertyName == nameof(TypeSelector.SelectedType))
            {
                var initializer = _sizeSelector.SelectedType != null
                    ? (IParticleInitializer) Activator.CreateInstance(_sizeSelector.SelectedType)
                    : null;

                _initializers[InitializerType.Size] = initializer;
            }
            
            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void StaticSizeEditorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            var initializer = (StaticSizeInitializer) _initializers[InitializerType.Size];
            switch (e.PropertyName)
            {
                case nameof(StaticSizeEditor.Width):
                    initializer.Width = _staticSizeEditor.Width;
                    break;
                
                case nameof(StaticSizeEditor.Height):
                    initializer.Height = _staticSizeEditor.Height;
                    break;
            }
            
            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void RandomSizeEditorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;
            
            var initializer = (RandomSizeInitializer) _initializers[InitializerType.Size];
            switch (e.PropertyName)
            {
                case nameof(RandomSizeEditor.MinWidth):
                    initializer.MinWidth = _randomSizeEditor.MinWidth;
                    break;
                
                case nameof(RandomSizeEditor.MinHeight):
                    initializer.MinHeight = _randomSizeEditor.MinHeight;
                    break;
                
                case nameof(RandomSizeEditor.MaxWidth):
                    initializer.MaxWidth = _randomSizeEditor.MaxWidth;
                    break;
                
                case nameof(RandomSizeEditor.MaxHeight):
                    initializer.MaxHeight = _randomSizeEditor.MaxHeight;
                    break;
            }
            
            UpdateUi();
            RaiseEmitterSettingsChangedEvent();
        }

        private void RaiseEmitterSettingsChangedEvent()
        {
            if (_particleTrigger == null) return; // Trigger is required
            
            var settings = new EmitterSettings(_particleTrigger, _initializers.Values, _modifiers, _particleMaxLife);
            EmitterSettingsChanged?.Invoke(this, settings);
        }
    }
}