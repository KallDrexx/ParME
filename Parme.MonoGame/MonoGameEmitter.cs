using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.CSharp;

namespace Parme.MonoGame
{
    public class MonoGameEmitter : Emitter
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Texture2D _texture;
        private readonly SpriteBatch _spriteBatch;
        
        public MonoGameEmitter(IEmitterLogic emitterLogic, GraphicsDevice graphicsDevice, Texture2D texture) 
            : base(emitterLogic)
        {
            _graphicsDevice = graphicsDevice;
            _texture = texture;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }

        public override void Render()
        {
            var middleOfScreen = new Vector2(
                _graphicsDevice.Viewport.Width / 2f,
                _graphicsDevice.Viewport.Height / 2f);
            
            _spriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            var particles = ParticleBuffer.Particles;
            for (var x = 0; x < particles.Length; x++)
            {
                ref var particle = ref particles[x];
                if (particle.IsAlive)
                {
                    var (posX, posY) = middleOfScreen + new Vector2(particle.Position.X, -particle.Position.Y);
                    var rectangle = new Rectangle((int) posX, (int) posY, (int) particle.Size.X, (int) particle.Size.Y);
                    var colorModifier = new Color(particle.RedMultiplier, 
                        particle.GreenMultiplier, 
                        particle.BlueMultiplier,
                        particle.AlphaMultiplier);

                    _spriteBatch.Draw(_texture,
                        rectangle,
                        null,
                        colorModifier,
                        particle.RotationInRadians,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0f);
                }
            }
            
            _spriteBatch.End();
        }
    }
}