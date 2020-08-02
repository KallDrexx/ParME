using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Parme
{
    public class Emitter
    {
        private readonly ParticleBuffer _particles = new ParticleBuffer(100);
        private readonly SpriteBatch _spriteBatch;
        private readonly EmitterSettings _settings;

        public Emitter(GraphicsDevice graphicsDevice, EmitterSettings settings)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void Update(float timeSinceLastFrame)
        {
            // Update existing particles
            for (var x = _particles.Count - 1; x >= 0; x--)
            {
                ref var particle = ref _particles[x];
                if (!particle.IsAlive)
                {
                    continue;
                }
                
                foreach (var modifier in _settings.Modifiers)
                {
                    modifier.Update(timeSinceLastFrame, ref particle);
                }
                
                particle.Position += _particles[x].Velocity;
                particle.TimeAlive += timeSinceLastFrame;

                if (particle.TimeAlive > _settings.MaxParticleLifeTime)
                {
                    particle.IsAlive = false;
                }
            }

            if (_settings.Trigger.ShouldCreateNewParticles(timeSinceLastFrame))
            {
                var count = _settings.ParticleCountInitializer.GetNewParticleCount();
                for (var x = 0; x < count; x++)
                {
                    var position = _settings.PositionalInitializer.GetNewParticlePosition();
                    var velocity = _settings.VelocityInitializer.GetNewParticleVelocity();
                    var size = _settings.SizeInitializer.GetNextParticleSize();
                    var colorModifier = _settings.ColorInitializer.GetColorOperationForNextParticle();
                    
                    _particles.Add(new Particle
                    {
                        IsAlive = true,
                        TimeAlive = 0,
                        Position = position,
                        Velocity = velocity,
                        Size = size,
                        ColorModifier = colorModifier,
                        RotationInRadians = 0, // TODO: add initializer
                    });
                }
            }
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