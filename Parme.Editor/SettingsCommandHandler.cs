using System;
using System.Collections.Generic;
using System.Linq;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Editor.AppOperations;
using Parme.Editor.Commands;

namespace Parme.Editor
{
    public class SettingsCommandHandler
    {
        private const float SecondsWithinDebounce = 0.25f;
        
        private readonly List<ICommand> _commands = new List<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();
        private readonly AppOperationQueue _appOperationQueue;
        private int _minimumStackSize;
        private float _secondsSinceLastCommand;
        
        public bool CanUndo => _commands.Count > _minimumStackSize;
        public bool CanRedo => _redoStack.Count > 0;
        public ICommand PreviousCommand => _commands.LastOrDefault();
        public ICommand NextCommand => _redoStack.Any() ? _redoStack.Peek() : null;

        public SettingsCommandHandler(AppOperationQueue appOperationQueue)
        {
            _appOperationQueue = appOperationQueue;
            NewStartingEmitter(new EmitterSettings());
        }

        public void UpdateTime(float timeSinceLastFrame)
        {
            _secondsSinceLastCommand += timeSinceLastFrame;
        }

        public void NewStartingEmitter(EmitterSettings settings)
        {
            settings ??= new EmitterSettings();
            _commands.Clear();

            _commands.Add(new UpdateParticleLifetimeCommand(settings.MaxParticleLifeTime));
            _commands.Add(new UpdateTextureFileNameCommand(settings.TextureFileName));
            _commands.Add(new UpdateTextureSectionsCommand(settings.TextureSections));
            _commands.Add(new UpdateTriggerCommand(settings.Trigger));
            _commands.Add(new UpdatePositionModifierCommand(settings.PositionModifier));

            foreach (var initializer in settings.Initializers ?? Array.Empty<IParticleInitializer>())
            {
                _commands.Add(new UpdateInitializerCommand(initializer.InitializerType, initializer));
            }

            foreach (var modifier in settings.Modifiers ?? Array.Empty<IParticleModifier>())
            {
                _commands.Add(new UpdateModifierCommand(modifier));
            }

            _minimumStackSize = _commands.Count;
            _secondsSinceLastCommand = SecondsWithinDebounce * 2; // make sure we are out of debounce time
        }

        public void Execute(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            if (_secondsSinceLastCommand < SecondsWithinDebounce && command.GetType() == _commands.Last().GetType())
            {
                // This is a repeated command, usually caused by fast inputs (like a slider).  Therefore to prevent
                // the undo stack being too bloated and not useful, replace the last command with the current one
                _commands[^1] = command;
            }
            else
            {
                _commands.Add(command);
            }
            
            _redoStack.Clear();
            _secondsSinceLastCommand = 0;
            
            RaiseUpdatedEmitterNotification();
        }

        public void Undo()
        {
            if (CanUndo)
            {
                _redoStack.Push(_commands.Last());
                _commands.RemoveAt(_commands.Count - 1);
                
                RaiseUpdatedEmitterNotification();
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                _commands.Add(_redoStack.Pop());
                
                RaiseUpdatedEmitterNotification();
            }
        }

        public EmitterSettings GetCurrentSettings()
        {
            var settings = new EmitterSettings();
            foreach (var command in _commands)
            {
                command.ApplyToEmitter(settings);
            }

            return settings;
        }

        private void RaiseUpdatedEmitterNotification()
        {
            _appOperationQueue.Enqueue(new EmitterUpdatedNotification(GetCurrentSettings()));
        }
    }
}