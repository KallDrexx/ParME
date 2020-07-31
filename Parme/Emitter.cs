using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.Modifiers;

namespace Parme
{
    public class Emitter
    {
        private readonly List<Particle> _particles = new List<Particle>();
        private readonly List<IParticleModifier> _modifiers;
        private readonly Random _random = new Random();
        private readonly SpriteBatch _spriteBatch;
        private float _timeSinceLastParticleSpawned;

        public Texture2D ParticleTexture { get; set; }
        public float SecondsBetweenNewParticles { get; set; }
        public float MaxParticleLifetime { get; set; }
        public Vector2 MinInitialParticleVelocity { get; set; }
        public Vector2 MaxInitialParticleVelocity { get; set; }
        public Vector2 MinInitialPosition { get; set; }
        public Vector2 MaxInitialPosition { get; set; }

        public Emitter(GraphicsDevice graphicsDevice, IEnumerable<IParticleModifier> modifiers)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _modifiers = modifiers?.ToList() ?? new List<IParticleModifier>();
        }

        public void Update(float timeSinceLastFrame)
        {
            // Update existing particles
            for (var x = _particles.Count - 1; x >= 0; x--)
            {
                foreach (var modifier in _modifiers)
                {
                    modifier.Update(timeSinceLastFrame, _particles[x]);
                }
                
                _particles[x].Position += _particles[x].Velocity;
                _particles[x].TimeAlive += timeSinceLastFrame;

                if (_particles[x].TimeAlive > MaxParticleLifetime)
                {
                    _particles.RemoveAt(x);
                }
            }
            
            // Spawn new particles if enough time has gone by
            _timeSinceLastParticleSpawned += timeSinceLastFrame;
            if (_timeSinceLastParticleSpawned > SecondsBetweenNewParticles)
            {
                var velocityX = _random.Next((int) MinInitialParticleVelocity.X, (int) MaxInitialParticleVelocity.X);
                var velocityY = _random.Next((int) MinInitialParticleVelocity.Y, (int) MaxInitialParticleVelocity.Y);
                var posX = _random.Next((int) MinInitialPosition.X, (int) MaxInitialPosition.X);
                var posY = _random.Next((int) MinInitialPosition.Y, (int) MaxInitialPosition.Y);
                
                _particles.Add(new Particle
                {
                    Position = new Vector2(posX, posY),
                    Velocity = new Vector2(velocityX, velocityY),
                    TimeAlive = 0,
                    Size = new Vector2(ParticleTexture.Width, ParticleTexture.Height),
                    RotationInRadians = 0f,
                });

                _timeSinceLastParticleSpawned = 0;
            }
        }

        public void Draw()
        {
            var middleOfScreen = new Vector2(1024 / 2, 768 / 2);
            
            _spriteBatch.Begin();
            foreach (var particle in _particles)
            {
                var (posX, posY) = middleOfScreen + new Vector2(particle.Position.X, -particle.Position.Y);
                var rectangle = new Rectangle((int) posX, (int) posY, (int) particle.Size.X, (int) particle.Size.Y);

                _spriteBatch.Draw(ParticleTexture,
                    rectangle,
                    null,
                    Color.White,
                    particle.RotationInRadians,
                    Vector2.Zero,
                    SpriteEffects.None,
                    0f);
            }
            _spriteBatch.End();
        }
    }
}