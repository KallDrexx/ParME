using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;
using Parme.CSharp;
using Parme.Editor.Ui.Elements;
using Parme.Editor.Ui.Elements.Initializers.ParticleCount;
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
            
            _triggerParentSection = new TypeSelector(triggerTypes){IsVisible = true};
            _oneShotTriggerEditor = new OneShotTriggerEditor{IsVisible = true};
            _timeElapsedTriggerEditor = new TimeElapsedTriggerEditor {IsVisible = true};
            _particleCountSelector = new TypeSelector(particleCountTypes) {IsVisible = true};
            _staticParticleCountEditor = new StaticParticleCountEditor {IsVisible = true};
            _randomParticleCountEditor = new RandomParticleCountEditor {IsVisible = true};

            mainSidePanel.TriggerParentSection = _triggerParentSection;
            mainSidePanel.ParticleCountSelector = _particleCountSelector;
            
            _triggerParentSection.PropertyChanged += TriggerParentSectionOnPropertyChanged;
            _timeElapsedTriggerEditor.PropertyChanged += TimeElapsedTriggerEditorOnPropertyChanged;
            _particleCountSelector.PropertyChanged += ParticleCountSelectorOnPropertyChanged;
            _staticParticleCountEditor.PropertyChanged += StaticParticleCountEditorOnPropertyChanged;
            _randomParticleCountEditor.PropertyChanged += RandomParticleCountEditorOnPropertyChanged;
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
            if (_particleTrigger == null)
            {
                _triggerParentSection.ChildDisplay = null;
            }
            else if (_particleTrigger.GetType() == typeof(OneShotTrigger))
            {
                _triggerParentSection.ChildDisplay = _oneShotTriggerEditor;
            }
            else if (_particleTrigger.GetType() == typeof(TimeElapsedTrigger))
            {
                _triggerParentSection.ChildDisplay = _timeElapsedTriggerEditor;
                _timeElapsedTriggerEditor.Frequency = ((TimeElapsedTrigger) _particleTrigger).Frequency;
            }
            else
            {
                throw new NotSupportedException($"No known editor for trigger type {_particleTrigger.GetType()}");
            }

            _initializers.TryGetValue(InitializerType.ParticleCount, out var countInitializer);
            _particleCountSelector.SelectedType = countInitializer?.GetType();
            if (countInitializer == null)
            {
                _particleCountSelector.ChildDisplay = null;
            }
            else if (countInitializer.GetType() == typeof(StaticParticleCountInitializer))
            {
                _particleCountSelector.ChildDisplay = _staticParticleCountEditor;
                _staticParticleCountEditor.ParticleSpawnCount = ((StaticParticleCountInitializer)countInitializer).ParticleSpawnCount;
            }
            else if (countInitializer.GetType() == typeof(RandomParticleCountInitializer))
            {
                _particleCountSelector.ChildDisplay = _randomParticleCountEditor;
                _randomParticleCountEditor.MinSpawnCount = ((RandomParticleCountInitializer)countInitializer).MinimumToSpawn;
                _randomParticleCountEditor.MaxSpawnCount = ((RandomParticleCountInitializer)countInitializer).MaximumToSpawn;
            }
            else
            {
                throw new NotSupportedException($"No known particle count editor for type {countInitializer.GetType()}");
            }

            _ignoreChangeNotifications = false;
        }

        private void TriggerParentSectionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            if (e.PropertyName == nameof(TypeSelector.SelectedType))
            {
                if (_triggerParentSection.SelectedType == typeof(OneShotTrigger))
                {
                    _particleTrigger = new OneShotTrigger();
                }
                else if (_triggerParentSection.SelectedType == typeof(TimeElapsedTrigger))
                {
                    _particleTrigger = new TimeElapsedTrigger();
                }
                else
                {
                    _particleTrigger = null;
                }
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
                IParticleInitializer initializer;
                if (_particleCountSelector.SelectedType == typeof(StaticParticleCountInitializer))
                {
                    initializer = new StaticParticleCountInitializer();
                }
                else if (_particleCountSelector.SelectedType == typeof(RandomParticleCountInitializer))
                {
                    initializer = new RandomParticleCountInitializer();
                }
                else
                {
                    initializer = null;
                }

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

        private void RaiseEmitterSettingsChangedEvent()
        {
            if (_particleTrigger == null) return; // Trigger is required
            
            var settings = new EmitterSettings(_particleTrigger, _initializers.Values, _modifiers, _particleMaxLife);
            EmitterSettingsChanged?.Invoke(this, settings);
        }
    }
}