using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Parme
{
    public class Emitter
    {
        private readonly ParticleBuffer _particles = new ParticleBuffer(100);
        private readonly SpriteBatch _spriteBatch;
        private readonly IEmitterLogic _emitterLogic;
        private readonly EmitterSettings _settings;

        public Emitter(GraphicsDevice graphicsDevice, IEmitterLogic emitterLogic, EmitterSettings settings)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _emitterLogic = emitterLogic ?? throw new ArgumentNullException(nameof(emitterLogic));
            _settings = settings;
        }

        public void Update(float timeSinceLastFrame)
        {
            _emitterLogic.Update(_particles, timeSinceLastFrame);
        }

        public void Draw()
        {
            var middleOfScreen = new Vector2(1024 / 2, 768 / 2);
            
            _spriteBatch.Begin();
            for (var x = 0; x < _particles.Count; x++)
            {
                ref var particle = ref _particles[x];
                if (particle.IsAlive)
                {
                    var (posX, posY) = middleOfScreen + new Vector2(particle.Position.X, -particle.Position.Y);
                    var rectangle = new Rectangle((int) posX, (int) posY, (int) particle.Size.X, (int) particle.Size.Y);

                    _spriteBatch.Draw(_settings.ParticleTexture,
                        rectangle,
                        null,
                        particle.ColorModifier,
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