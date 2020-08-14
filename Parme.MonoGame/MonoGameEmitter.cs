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

        public override void Render(ParticleCamera camera)
        {
            _spriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            var cameraHalfWidth = camera.PixelWidth / 2;
            var cameraHalfHeight = camera.PixelHeight / 2;

            var particles = ParticleBuffer.Particles;
            for (var x = 0; x < particles.Length; x++)
            {
                ref var particle = ref particles[x];
                if (particle.IsAlive)
                {
                    if (camera.IsInView(ref particle))
                    {
                        var positionDifference = camera.Origin - particle.Position;
                        var particleHalfWidth = particle.Size.X / 2;
                        var particleHalfHeight = particle.Size.Y / 2;

                        var startX = cameraHalfWidth - positionDifference.X - particleHalfWidth;
                        var startY = camera.PositiveYAxisPointsUp
                            ? cameraHalfHeight + positionDifference.Y - particleHalfHeight
                            : cameraHalfHeight - positionDifference.Y - particleHalfHeight;
                        
                        var rectangle = new Rectangle((int) startX, (int) startY, (int) particle.Size.X, (int) particle.Size.Y);
                        
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
            }
            
            _spriteBatch.End();
        }
    }
}