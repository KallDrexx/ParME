using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.Modifiers;

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
                pixels[x] = Color.Red;
            }
            
            var texture = new Texture2D(GraphicsDevice, 10, 10);
            texture.SetData(pixels);

            var modifiers = new IParticleModifier[]
            {
                new ConstantRotationModifier(180f),
                new ConstantAccelerationModifier(new Vector2(0, -25)),
                new ConstantSizeModifier(new Vector2(100, 100)),
            };
                
            _emitter = new Emitter(GraphicsDevice, modifiers)
            {
                ParticleTexture = texture,
                MaxParticleLifetime = 1f,
                SecondsBetweenNewParticles = 0.1f,
                MinInitialParticleVelocity = new Vector2(-5, 10),
                MaxInitialParticleVelocity = new Vector2(5, 15),
                MinInitialPosition = new Vector2(-100, 0),
                MaxInitialPosition = new Vector2(100, 0),
            };
            
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _emitter.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            _emitter.Draw();
            
            base.Draw(gameTime);
        }
    }
}