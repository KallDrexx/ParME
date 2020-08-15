using FlatRedBall;
using FlatRedBall.Input;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Xna.Framework.Input;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;
using Parme.CSharp;
using Parme.CSharp.CodeGen;
using Parme.Frb;

namespace Parme.Frb.Example.Screens
{
    public partial class GameScreen
    {
        private EmitterDrawableBatch _emitter;
        
        void CustomInitialize()
        {
            var emitterSettings = GetBasicFlameEmitterSettings();
            var code = EmitterLogicClassGenerator.Generate(emitterSettings, 
                "Parme.Frb.Example.Settings",
                "TestEmitter", 
                true);

            var scriptOptions = ScriptOptions.Default
                .WithReferences(typeof(IEmitterLogic).Assembly);
                
            var logicClass = CSharpScript.EvaluateAsync<IEmitterLogic>(code, scriptOptions).GetAwaiter().GetResult();

            _emitter = new EmitterDrawableBatch(logicClass);
            _emitter.StartEmitting();

            SpriteManager.AddDrawableBatch(_emitter);
        }

        void CustomActivity(bool firstTimeCalled)
        {
            const float movementSpeed = 200;
            if (InputManager.Keyboard.KeyDown(Keys.Up))
            {
                SomeCircleInstance.Y += TimeManager.SecondDifference * movementSpeed;
            }

            if (InputManager.Keyboard.KeyDown(Keys.Down))
            {
                SomeCircleInstance.Y -= TimeManager.SecondDifference * movementSpeed;
            }

            if (InputManager.Keyboard.KeyDown(Keys.Right))
            {
                SomeCircleInstance.X += TimeManager.SecondDifference * movementSpeed;
            }

            if (InputManager.Keyboard.KeyDown(Keys.Left))
            {
                SomeCircleInstance.X -= TimeManager.SecondDifference * movementSpeed;
            }

            _emitter.X = SomeCircleInstance.X;
            _emitter.Y = SomeCircleInstance.Y;
            _emitter.Z = SomeCircleInstance.Z;

            if (InputManager.Keyboard.KeyReleased(Keys.Enter))
            {
                if (_emitter.IsEmittingParticles)
                {
                    _emitter.StopEmitting();
                }
                else
                {
                    _emitter.StartEmitting();
                }
            }
        }

        void CustomDestroy()
        {
        }

        static void CustomLoadStaticContent(string contentManagerName)
        {
        }
        
        private static EmitterSettings GetBasicFlameEmitterSettings()
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
                    MinYOffset = -50,
                    MaxYOffset = -50,
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
                    WidthChangePerSecond = -10,
                    HeightChangePerSecond = -10,
                },

                new ConstantColorMultiplierChangeModifier
                {
                    RedMultiplierChangePerSecond = -1,
                    GreenMultiplierChangePerSecond = -1,
                    BlueMultiplierChangePerSecond = -1,
                    //AlphaMultiplierChangePerSecond = -1,
                },
            };
            
            return new EmitterSettings(trigger, initializers, modifiers, 1f);
        }
    }
}
