using System;
using System.IO;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;

namespace Parme.Editor.AppOperations
{
    public class NewEmitterRequested : IAppOperation
    {
        public string FileName { get; }
        public NewEmitterTemplate Template { get; }

        public NewEmitterRequested(string fileName, NewEmitterTemplate template)
        {
            FileName = fileName;
            Template = template;
        }
        
        public AppOperationResult Run()
        {
            var emitter = GetEmitterForTemplate(Template);
            var emitterJson = emitter.ToJson();

            try
            {
                File.WriteAllText(FileName, emitterJson);
            }
            catch (IOException exception)
            {
                return new AppOperationResult
                {
                    NewErrorMessage = $"Could not create file '{FileName}': {exception.Message}",
                };
            }
            
            return new AppOperationResult
            {
                UpdatedSettings = emitter,
                UpdatedFileName = FileName,
                ResetUnsavedChangesMarker = true,
                ModalToClose = Modal.NewFileDialog,
            };
        }

        private static EmitterSettings GetEmitterForTemplate(NewEmitterTemplate template)
        {
            return template switch
            {
                NewEmitterTemplate.Fire => GetFireEmitter(),
                NewEmitterTemplate.None => GetBlankEmitter(),
                _ => throw new NotSupportedException($"No emitter can be set up for template '{template}'")
            };
        }

        private static EmitterSettings GetFireEmitter()
        {
            var trigger = new TimeElapsedTrigger{Frequency = 0.01f};
            var initializers = new IParticleInitializer[]
            {
                new RandomParticleCountInitializer {MinimumToSpawn = 0, MaximumToSpawn = 5},
                new StaticColorInitializer
                {
                    // Orange
                    Red = 255,
                    Green = 165,
                    Blue = 0,
                    Alpha = 1f
                },

                new RandomRangeVelocityInitializer
                {
                    MinXVelocity = 0,
                    MaxXVelocity = 0,
                    MinYVelocity = 100,
                    MaxYVelocity = 200,
                },

                new RandomRegionPositionInitializer
                {
                    MinXOffset = -25,
                    MaxXOffset = 25,
                    MinYOffset = 20,
                    MaxYOffset = -20,
                },

                new StaticSizeInitializer
                {
                    Width = 10,
                    Height = 10,
                },
            };

            var modifiers = new IParticleModifier[]
            {
                new ConstantRotationModifier {DegreesPerSecond = 100f},
                new ConstantAccelerationModifier
                {
                    XAcceleration = -75,
                    YAcceleration = 0,
                },

                new ConstantSizeModifier
                {
                    WidthChangePerSecond = -5,
                    HeightChangePerSecond = -5,
                },

                new EndingColorModifier()
                {
                    Red = 255,
                    Green = 165,
                    Blue = 0,
                    Alpha = 0f,
                },
            };

            return new EmitterSettings
            {
                Trigger = trigger,
                Initializers = initializers,
                Modifiers = modifiers,
                MaxParticleLifeTime = 1f,
            };
        }

        private static EmitterSettings GetBlankEmitter()
        {
            return new EmitterSettings
            {
                MaxParticleLifeTime = 1,
                Trigger = new OneShotTrigger(),
                Initializers = new IParticleInitializer[]
                {
                    new StaticSizeInitializer {Width = 32, Height = 32},
                    new StaticColorInitializer {Red = 255, Green = 255, Blue = 255, Alpha = 1f},
                    new StaticParticleCountInitializer {ParticleSpawnCount = 1}, 
                }
            };
        }
    }
}