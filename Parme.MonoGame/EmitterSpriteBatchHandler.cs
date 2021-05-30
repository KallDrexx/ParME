using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.CSharp;

namespace Parme.MonoGame
{
    public class EmitterSpriteBatchHandler
    {
        private readonly BasicEffect _basicEffect;

        public EmitterSpriteBatchHandler(GraphicsDevice graphicsDevice)
        {
            _basicEffect = new BasicEffect(graphicsDevice);
        }

        public void SetupAndStartSpriteBatch(SpriteBatch spriteBatch, ParticleCamera camera, SamplerState samplerState)
        {
            var totalHorizontalZoomFactor = camera.HorizontalZoomFactor;
            var totalVerticalZoomFactor = camera.VerticalZoomFactor;
            
            _basicEffect.TextureEnabled = true;
            _basicEffect.LightingEnabled = false;
            _basicEffect.FogEnabled = false;
            _basicEffect.VertexColorEnabled = true;
            _basicEffect.World = Matrix.Identity;
            _basicEffect.Projection = Matrix.CreateOrthographic(camera.PixelWidth, -camera.PixelHeight, -1, 1);
            _basicEffect.View =
                Matrix.CreateTranslation(
                    -camera.Origin.X, 
                    camera.Origin.Y,
                    0) *
                Matrix.CreateScale(totalHorizontalZoomFactor, totalVerticalZoomFactor, 1);

            spriteBatch.Begin(blendState: BlendState.NonPremultiplied, 
                effect: _basicEffect, 
                samplerState: samplerState);
        }
    }
}