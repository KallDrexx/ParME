using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.CSharp;

namespace Parme.MonoGame
{
    public class MonoGameEmitter : Emitter
    {
        private readonly Texture2D _texture;
        private readonly SpriteBatch _spriteBatch;
        private readonly BasicEffect _basicEffect;
        
        public MonoGameEmitter(IEmitterLogic emitterLogic, 
            GraphicsDevice graphicsDevice,
            ITextureFileLoader textureFileLoader) 
            : base(emitterLogic)
        {
            if (textureFileLoader == null)
            {
                throw new ArgumentNullException(nameof(textureFileLoader));
            }

            _texture = string.IsNullOrWhiteSpace(emitterLogic.TextureFilePath)
                ? GetDefaultWhiteTexture(graphicsDevice)
                : textureFileLoader.LoadTexture2D(emitterLogic.TextureFilePath);

            _spriteBatch = new SpriteBatch(graphicsDevice);
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

                    var destinationRectangle = new Rectangle((int) startX, 
                        (int) startY, 
                        (int) particle.Size.X,
                        (int) particle.Size.Y);

                    Rectangle sourceRectangle;
                    if (EmitterLogic.TextureSections.Length == 0)
                    {
                        sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
                    }
                    else
                    {
                        ref var section = ref EmitterLogic.TextureSections[particle.TextureSectionIndex];
                        sourceRectangle = new Rectangle(section.LeftX, 
                            section.TopY,
                            section.RightX - section.LeftX,
                            section.BottomY - section.TopY);
                    }

                    var colorModifier = new Color(particle.RedMultiplier,
                        particle.GreenMultiplier,
                        particle.BlueMultiplier,
                        particle.AlphaMultiplier);

                    _spriteBatch.Draw(_texture,
                        destinationRectangle,
                        sourceRectangle,
                        colorModifier,
                        particle.RotationInRadians,
                        Vector2.Zero,
                        SpriteEffects.None,
                        0f);
                }
            }
            
            _spriteBatch.End();
        }

        private static Texture2D GetDefaultWhiteTexture(GraphicsDevice graphicsDevice)
        {
            const int size = 10;
            var pixels = new Color[size * size];
            for (var x = 0; x < pixels.Length; x++)
            {
                pixels[x] = Color.White;
            }
            
            var texture = new Texture2D(graphicsDevice, size, size);
            texture.SetData(pixels);

            return texture;
        }
    }
}