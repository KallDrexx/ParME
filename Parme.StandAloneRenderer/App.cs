using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.Initializers.ColorInitializer;
using Parme.Initializers.ParticleCountInitializer;
using Parme.Initializers.PositionalInitializers;
using Parme.Initializers.SizeInitializers;
using Parme.Initializers.VelocityInitializers;
using Parme.Modifiers;
using Parme.Scripting;
using Parme.Triggers;

namespace Parme.StandAloneRenderer
{
    public class App : Game
    {
        private Emitter _emitter;

        public App()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1024,
                PreferredBackBufferHeight = 768,
                PreferMultiSampling = true
            };
        }

        protected override void Initialize()
        {
            var pixels = new Color[10*10];
            for (var x = 0; x < pixels.Length; x++)
            {
                pixels[x] = Color.White;
            }
            
            var texture = new Texture2D(GraphicsDevice, 10, 10);
            texture.SetData(pixels);

            var settings = new EmitterSettings
            {
                ParticleTexture = texture,
                Trigger = new TimeElapsedTrigger(0.01f),
                ParticleCountInitializer = new RandomParticleCountInitializer(0, 5),
                MaxParticleLifeTime = 1f,
                ColorInitializer = new StaticColorInitializer(Color.Orange),
                VelocityInitializer = new RandomRangeVelocityInitializer(
                    new Vector2(0, 2),
                    new Vector2(0, 5)),
                
                PositionalInitializer =  new RandomRegionPositionInitializer(
                    new Vector2(-25, -50), 
                    new Vector2(25, -50)),
                
                SizeInitializer = new StaticSizeInitializer(new Vector2(10, 10)),
                Modifiers =
                {
                    new ConstantRotationModifier(180f),
                    new ConstantAccelerationModifier(new Vector2(-5, 5)),
                    new ConstantSizeModifier(new Vector2(-10, -10)),
                    new ConstantColorChangeModifier(-300, -300, -300),
                }
            };

            var test = CSharpEmitterLogicGenerator.Generate(settings, "Parme.Game", "TestLogic");

            var scriptOptions = ScriptOptions.Default
                .WithReferences(typeof(IEmitterLogic).Assembly);
                
            var logicClass = CSharpScript.EvaluateAsync<IEmitterLogic>(test, scriptOptions).GetAwaiter().GetResult();

            _emitter = new Emitter(GraphicsDevice, logicClass, settings);
            
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _emitter.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            
            _emitter.Draw();
            
            base.Draw(gameTime);
        }
    }
}