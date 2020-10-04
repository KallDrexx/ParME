
using System;
using System.Numerics;
using Parme.Core;
using Parme.CSharp;

namespace Parme.Frb.Example
{

    public class ContrailsEmitterLogic : IEmitterLogic
    {
        private readonly Random _random = new Random();
        private Vector2 _lastEmittedPosition;

        public float MaxParticleLifeTime { get; set; } = 0.5f;
        
        public string TextureFilePath { get; } = @"Content\GlobalContent\SampleParticles.png";
        public TextureSectionCoords[] TextureSections { get; } = new TextureSectionCoords[] {
            new TextureSectionCoords(16, 0, 32, 16),
            new TextureSectionCoords(32, 0, 48, 16),
            new TextureSectionCoords(48, 0, 64, 16),
        };
        
        
        public float DistanceBasedTriggerUnitsPerEmission { get; set; } = 20f;
        public int StaticParticleSpawnCount { get; set; } = 1;

        public float StaticPositionXOffset { get; set; } = 0f;
        public float StaticPositionYOffset { get; set; } = 0;

        public byte StaticColorStartingRed { get; set; } = 255;
        public byte StaticColorStartingGreen { get; set; } = 159;
        public byte StaticColorStartingBlue { get; set; } = 11;
        public byte StaticColorStartingAlpha { get; set; } = 255;

        public int StaticSizeWidth { get; set; } = 50;
        public int StaticSizeHeight { get; set; } = 50;

        public byte EndingColorRed { get; set; } = 255;
        public byte EndingColorGreen { get; set; } = 255;
        public byte EndingColorBlue { get; set; } = 255;
        public byte EndingColorAlpha { get; set; } = 0;

        public float ConstantRotationRadiansPerSecond { get; set; } = 3.141592653589793f;

        
        public void Update(ParticleBuffer particleBuffer, float timeSinceLastFrame, Emitter parent)
        {
            var emitterCoordinates = parent.WorldCoordinates;

            // Update existing particles
            var particles = particleBuffer.Particles;
            for (var particleIndex = 0; particleIndex < particles.Length; particleIndex++)
            {
                ref var particle = ref particles[particleIndex];
                if (!particle.IsAlive)
                {
                    continue;
                }
                
                particle.TimeAlive += timeSinceLastFrame;
                if (particle.TimeAlive > MaxParticleLifeTime)
                {
                    particle.IsAlive = false;
                    continue;
                }
                
                // modifiers
                
                {
                        particle.CurrentRed -= (byte) (((particle.InitialRed - EndingColorRed) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentGreen -= (byte) (((particle.InitialGreen - EndingColorGreen) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentBlue -= (byte) (((particle.InitialBlue - EndingColorBlue) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentAlpha -= (byte) (((particle.InitialAlpha - EndingColorAlpha) / MaxParticleLifeTime) * timeSinceLastFrame);
                }
                {
                        particle.RotationInRadians += timeSinceLastFrame * ConstantRotationRadiansPerSecond;
                }

                
                particle.Position += particle.Velocity * timeSinceLastFrame;
            }
            
            var shouldCreateNewParticle = false;
            var stopEmittingAfterUpdate = false;
            {
                
            shouldCreateNewParticle = false;
            var distance = Math.Abs(Vector2.Distance(_lastEmittedPosition, parent.WorldCoordinates));
            if (DistanceBasedTriggerUnitsPerEmission > 0 && distance >= DistanceBasedTriggerUnitsPerEmission)
            {
                shouldCreateNewParticle = true;
                _lastEmittedPosition = parent.WorldCoordinates;
            }

            }
            
            if (shouldCreateNewParticle && parent.IsEmittingNewParticles)
            {
                var newParticleCount = 0;
                {
                    
                    {
                        newParticleCount = StaticParticleSpawnCount;
                    }

                }
                
                for (var newParticleIndex = 0; newParticleIndex < newParticleCount; newParticleIndex++)
                {
                    var particle = new Particle
                    {
                        IsAlive = true,
                        TimeAlive = 0,
                        RotationInRadians = 0, // TODO: add initializer
                    };
                    
                    // Initializers
                    
                    {
                        
                        particle.Position = new Vector2(StaticPositionXOffset, StaticPositionYOffset);
                    }
                    {
                        
                        particle.InitialRed = StaticColorStartingRed;
                        particle.CurrentRed = StaticColorStartingRed;
                        particle.InitialGreen = StaticColorStartingGreen;
                        particle.CurrentGreen = StaticColorStartingGreen;
                        particle.InitialBlue = StaticColorStartingBlue;
                        particle.CurrentBlue = StaticColorStartingBlue;
                        particle.InitialAlpha = StaticColorStartingAlpha;
                        particle.CurrentAlpha = StaticColorStartingAlpha;
                                }
                    {
                        particle.Size = new Vector2(StaticSizeWidth, StaticSizeHeight);
                    }
                    {
                        
                        particle.TextureSectionIndex = (byte) _random.Next(0, TextureSections.Length);
                    }


                    // Adjust the particle's position by the emitter's location
                    particle.Position += emitterCoordinates;
                    
                    particleBuffer.Add(particle);            
                }
            }

            if (stopEmittingAfterUpdate)
            {
                parent.IsEmittingNewParticles = false;
                stopEmittingAfterUpdate = false;
            }
        }
    }
}
