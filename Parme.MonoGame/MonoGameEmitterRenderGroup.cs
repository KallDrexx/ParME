using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.CSharp;

namespace Parme.MonoGame
{
    public class MonoGameEmitterRenderGroup
    {
        private readonly List<MonoGameEmitter> _emitters = new List<MonoGameEmitter>();
        private readonly SpriteBatch _spriteBatch;
        private readonly BasicEffect _basicEffect;
        private readonly GraphicsDevice _graphicsDevice;

        public MonoGameEmitterRenderGroup(GraphicsDevice graphicsDevice)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _basicEffect = new BasicEffect(graphicsDevice);
            _graphicsDevice = graphicsDevice;
        }

        public void AddEmitter(MonoGameEmitter emitter)
        {
            if (!_emitters.Contains(emitter))
            {
                _emitters.Add(emitter);
            }
        }

        public void RemoveEmitter(MonoGameEmitter emitter)
        {
            _emitters.Remove(emitter);
        }

        public void Render(ParticleCamera camera, SamplerState samplerState)
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

            _spriteBatch.Begin(blendState: BlendState.NonPremultiplied, 
                effect: _basicEffect, 
                samplerState: samplerState);

            foreach (var emitter in _emitters)
            {
                emitter.Render(camera, _spriteBatch);
            }
            
            _spriteBatch.End();
        }
    }
}