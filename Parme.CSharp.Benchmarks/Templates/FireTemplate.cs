using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;

namespace Parme.CSharp.Benchmarks.Templates
{
    public static class FireTemplate
    {
        public static EmitterSettings Emitter
        {
            get
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
        } 
    }
}