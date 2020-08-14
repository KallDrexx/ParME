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
        private readonly BasicEffect _basicEffect;
        
        public MonoGameEmitter(IEmitterLogic emitterLogic, GraphicsDevice graphicsDevice, Texture2D texture) 
            : base(emitterLogic)
        {
            _graphicsDevice = graphicsDevice;
            _texture = texture;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _basicEffect = new BasicEffect(graphicsDevice);
        }

        public override void Render(ParticleCamera camera)
        {
            var totalHorizontalZoomFactor = camera.HorizontalZoomFactor * ScaleFactor;
            var totalVerticalZoomFactor = camera.VerticalZoomFactor * ScaleFactor;
            
            _basicEffect.TextureEnabled = true;
            _basicEffect.LightingEnabled = false;
            _basicEffect.FogEnabled = false;
            _basicEffect.VertexColorEnabled = true;
            _basicEffect.World = Matrix.Identity;
            _basicEffect.Projection = Matrix.CreateOrthographic(camera.PixelWidth, -camera.PixelHeight, -1, 1);
            _basicEffect.View =
                Matrix.CreateTranslation(
                    -camera.Origin.X / totalHorizontalZoomFactor, 
                    camera.Origin.Y / totalVerticalZoomFactor,
                    0) *
                Matrix.CreateScale(totalHorizontalZoomFactor, totalVerticalZoomFactor, 1);

            _spriteBatch.Begin(blendState: BlendState.NonPremultiplied, effect: _basicEffect);

            var particles = ParticleBuffer.Particles;
            for (var x = 0; x < particles.Length; x++)
            {
                ref var particle = ref particles[x];
                if (particle.IsAlive)
                {
                    var particleHalfWidth = particle.Size.X / 2;
                    var particleHalfHeight = particle.Size.Y / 2;

                    var startX = particle.Position.X - particleHalfWidth;
                    var startY = camera.PositiveYAxisPointsUp
                        ? -particle.Position.Y - particleHalfHeight
                        : particle.Position.Y - particleHalfHeight;

                    var rectangle = new Rectangle((int) startX, (int) startY, (int) particle.Size.X,
                        (int) particle.Size.Y);

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