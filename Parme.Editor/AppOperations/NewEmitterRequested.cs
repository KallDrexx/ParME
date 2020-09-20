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
            };
        }

        private static EmitterSettings GetEmitterForTemplate(NewEmitterTemplate template)
        {
            switch (template)
            {
                case NewEmitterTemplate.Fire:
                    return GetFireEmitter();
                
                case NewEmitterTemplate.None:
                    return new EmitterSettings();
                
                default:
                    throw new NotSupportedException($"No emitter can be set up for template '{template}'");
            }
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
                    RedMultiplier = 1.0f,
                    GreenMultiplier = 165f / 255f,
                    BlueMultiplier = 0f,
                    AlphaMultiplier = 1f
                },

                new RandomRangeVelocityInitializer
                {
                    MinXVelocity = 0,
                    MaxXVelocity = 0,
                    MinYVelocity = 2,
                    MaxYVelocity = 5,
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
                    XAcceleration = -5,
                    YAcceleration = 5,
                },

                new ConstantSizeModifier
                {
                    WidthChangePerSecond = -5,
                    HeightChangePerSecond = -5,
                },

                new ConstantColorMultiplierChangeModifier
                {
                    RedMultiplierChangePerSecond = -1,
                    GreenMultiplierChangePerSecond = -1,
                    BlueMultiplierChangePerSecond = -1,
                    AlphaMultiplierChangePerSecond = -0.5f,
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
    }
}