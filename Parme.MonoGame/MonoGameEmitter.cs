using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.CSharp;

namespace Parme.MonoGame
{
    public class MonoGameEmitter : Emitter
    {
        private readonly Texture2D _texture;

        public MonoGameEmitter(IEmitterLogic emitterLogic,
            ParticlePool particlePool,
            GraphicsDevice graphicsDevice,
            ITextureFileLoader textureFileLoader) 
            : base(emitterLogic, particlePool)
        {
            if (textureFileLoader == null)
            {
                throw new ArgumentNullException(nameof(textureFileLoader));
            }

            _texture = string.IsNullOrWhiteSpace(emitterLogic.TextureFilePath)
                ? GetDefaultWhiteTexture(graphicsDevice)
                : textureFileLoader.LoadTexture2D(emitterLogic.TextureFilePath);
            
            FullTextureSize = new System.Numerics.Vector2(_texture.Width, _texture.Height);
        }

        public void Render(ParticleCamera camera, SpriteBatch spriteBatch)
        {
            var particles = ParticleBuffer.Particles;
            for (var x = 0; x < particles.Length; x++)
            {
                ref var particle = ref particles[x];
                if (particle.IsAlive)
                {
                    var startX = particle.Position.X;
                    var startY = camera.PositiveYAxisPointsUp
                        ? -particle.Position.Y
                        : particle.Position.Y;

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

                    var colorModifier = new Color((byte) particle.CurrentRed,
                        (byte) particle.CurrentGreen,
                        (byte) particle.CurrentBlue,
                        (byte) particle.CurrentAlpha);

                    spriteBatch.Draw(_texture,
                        destinationRectangle,
                        sourceRectangle,
                        colorModifier,
                        -particle.RotationInRadians, // CCW rotations
                        new Vector2(sourceRectangle.Width / 2f, sourceRectangle.Height / 2f), 
                        SpriteEffects.None,
                        0f);
                }
            }
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