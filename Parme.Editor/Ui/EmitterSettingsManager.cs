using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;
using Parme.CSharp;
using Parme.Editor.Ui.Elements;
using Parme.Editor.Ui.Elements.Triggers;

namespace Parme.Editor.Ui
{
    public class EmitterSettingsManager
    {
        private readonly TriggerParentSection _triggerParentSection;
        private readonly OneShotTriggerEditor _oneShotTriggerEditor;
        private readonly TimeElapsedTriggerEditor _timeElapsedTriggerEditor;
        
        private readonly List<IParticleModifier> _modifiers = new List<IParticleModifier>();
        private float _particleMaxLife;
        private IParticleTrigger _particleTrigger;
        private Dictionary<InitializerType, IParticleInitializer> _initializers = new Dictionary<InitializerType, IParticleInitializer>();
            
        private bool _ignoreChangeNotifications;

        public event EventHandler<EmitterSettings> EmitterSettingsChanged; 

        public EmitterSettingsManager(MainSidePanel mainSidePanel)
        {
            _triggerParentSection = new TriggerParentSection{IsVisible = true};
            _oneShotTriggerEditor = new OneShotTriggerEditor{IsVisible = true};
            _timeElapsedTriggerEditor = new TimeElapsedTriggerEditor {IsVisible = true};

            mainSidePanel.TriggerParentSection = _triggerParentSection;
            
            _triggerParentSection.PropertyChanged += TriggerParentSectionOnPropertyChanged;
            _timeElapsedTriggerEditor.PropertyChanged += TimeElapsedTriggerEditorOnPropertyChanged; 
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

            if (_particleTrigger == null)
            {
                _triggerParentSection.TriggerDisplaySection = null;
            }
            else if (_particleTrigger.GetType() == typeof(OneShotTrigger))
            {
                _triggerParentSection.TriggerDisplaySection = _oneShotTriggerEditor;
            }
            else if (_particleTrigger.GetType() == typeof(TimeElapsedTrigger))
            {
                _triggerParentSection.TriggerDisplaySection = _timeElapsedTriggerEditor;
                _timeElapsedTriggerEditor.Frequency = ((TimeElapsedTrigger) _particleTrigger).Frequency;
            }
            else
            {
                throw new NotSupportedException($"No known editor for trigger type {_particleTrigger.GetType()}");
            }

            _ignoreChangeNotifications = false;
        }

        private void TriggerParentSectionOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_ignoreChangeNotifications) return;

            _particleTrigger = _triggerParentSection.SelectedTriggerType switch
            {
                null => null,
                TriggerParentSection.TriggerTypes.OneShot => new OneShotTrigger(),
                TriggerParentSection.TriggerTypes.TimeElapsed => new TimeElapsedTrigger(),
                _ => throw new NotSupportedException(
                    $"No known UI exists for trigger of type {_triggerParentSection.SelectedTriggerType}")
            };
            
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

        private void RaiseEmitterSettingsChangedEvent()
        {
            var settings = new EmitterSettings(_particleTrigger, _initializers.Values, _modifiers, _particleMaxLife);
            EmitterSettingsChanged?.Invoke(this, settings);
        }
    }
}