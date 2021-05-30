using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.CSharp;

namespace Parme.MonoGame
{
    public class MonoGameEmitterRenderGroup
    {
        private readonly List<MonoGameEmitter> _emitters = new List<MonoGameEmitter>();
        private readonly EmitterSpriteBatchHandler _emitterSpriteBatchHandler;
        private readonly SpriteBatch _spriteBatch;

        public MonoGameEmitterRenderGroup(GraphicsDevice graphicsDevice)
        {
            _emitterSpriteBatchHandler = new EmitterSpriteBatchHandler(graphicsDevice);
            _spriteBatch = new SpriteBatch(graphicsDevice);
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
            _emitterSpriteBatchHandler.SetupAndStartSpriteBatch(_spriteBatch, camera, samplerState);

            foreach (var emitter in _emitters)
            {
                emitter.Render(camera, _spriteBatch);
            }
            
            _spriteBatch.End();
        }
    }
}