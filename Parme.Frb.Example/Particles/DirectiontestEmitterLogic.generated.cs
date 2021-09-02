
using System;
using System.Numerics;
using Parme.Core;
using Parme.CSharp;

namespace Parme.Frb.Example
{

    public class DirectiontestEmitterLogic : IEmitterLogic
    {
        private float _timeSinceLastTrigger;

        public float MaxParticleLifeTime { get; set; } = 1f;
        
        public string TextureFilePath { get; } = @"Content\GlobalContent\direction.png";
        public TextureSectionCoords[] TextureSections { get; } = new TextureSectionCoords[] {
        };
        
        
        public float TimeElapsedTriggerFrequency { get; set; } = 1f; 

        public int StaticSizeWidth { get; set; } = 32;
        public int StaticSizeHeight { get; set; } = 32;

        public int StaticParticleSpawnCount { get; set; } = 1;

        public float RadialVelocityMinMagnitude { get; set; } = 100f;
        public float RadialVelocityMaxMagnitude { get; set; } = 100f;
        public float RadialVelocityMinRadians { get; set; } = 3.141592653589793f;
        public float RadialVelocityMaxRadians { get; set; } = 3.141592653589793f;
        public float RadialVelocityXAxisScale { get; set; } = 1f;
        public float RadialVelocityYAxisScale { get; set; } = 1f; 

        public byte StaticColorStartingRed { get; set; } = 255;
        public byte StaticColorStartingGreen { get; set; } = 255;
        public byte StaticColorStartingBlue { get; set; } = 255;
        public byte StaticColorStartingAlpha { get; set; } = 125;

        public byte EndingColorRed { get; set; } = 255;
        public byte EndingColorGreen { get; set; } = 255;
        public byte EndingColorBlue { get; set; } = 255;
        public byte EndingColorAlpha { get; set; } = 0;

        
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
                        particle.CurrentRed -= (((particle.InitialRed - EndingColorRed) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentGreen -= (((particle.InitialGreen - EndingColorGreen) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentBlue -= (((particle.InitialBlue - EndingColorBlue) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentAlpha -= (((particle.InitialAlpha - EndingColorAlpha) / MaxParticleLifeTime) * timeSinceLastFrame);
                }

                
                // position modifier
                particle.Position += particle.Velocity * timeSinceLastFrame;

                particle.RotationInRadians += particle.RotationalVelocityInRadians * timeSinceLastFrame;
            }
            
            var shouldCreateNewParticle = false;
            var stopEmittingAfterUpdate = false;
            {
                
            shouldCreateNewParticle = false;
            _timeSinceLastTrigger += timeSinceLastFrame;
            if (_timeSinceLastTrigger >= TimeElapsedTriggerFrequency)
            {
                shouldCreateNewParticle = true;
                _timeSinceLastTrigger = 0;  
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
                        RotationInRadians = 0,
                        Position = Vector2.Zero,
                        ReferencePosition = Vector2.Zero,
                        RotationalVelocityInRadians = 0f,
                        CurrentRed = 255,
                        CurrentGreen = 255,
                        CurrentBlue = 255,
                        CurrentAlpha = 255,
                        Size = Vector2.Zero,
                        InitialSize = Vector2.Zero,
                        Velocity = Vector2.Zero,
                    };
                    
                    // Initializers
                    
                    {
                        particle.Size = new Vector2(StaticSizeWidth, StaticSizeHeight);
                    }
                    {
                        
                        var radians = RadialVelocityMaxRadians - parent.Random.NextDouble() * (RadialVelocityMaxRadians - RadialVelocityMinRadians);
                        var magnitude = RadialVelocityMaxMagnitude - parent.Random.NextDouble() * (RadialVelocityMaxMagnitude - RadialVelocityMinMagnitude);
                
                        // convert from polar coordinates to cartesian coordinates
                        var x = magnitude * Math.Cos(radians) * RadialVelocityXAxisScale;
                        var y = magnitude * Math.Sin(radians) * RadialVelocityYAxisScale;
                        particle.Velocity = new Vector2((float) x, (float) y);
                    }
                    {
                        
                        particle.CurrentRed = (float) StaticColorStartingRed;
                        particle.CurrentGreen = (float) StaticColorStartingGreen;
                        particle.CurrentBlue = (float) StaticColorStartingBlue;
                        particle.CurrentAlpha = (float) StaticColorStartingAlpha;
                                }


                    // Set the initial values to their current equivalents
                    particle.InitialRed = (byte) particle.CurrentRed;
                    particle.InitialGreen = (byte) particle.CurrentGreen;
                    particle.InitialBlue = (byte) particle.CurrentBlue;
                    particle.InitialAlpha = (byte) particle.CurrentAlpha;
                    particle.InitialSize = particle.Size;

                    // Adjust the particle's rotation, position, and velocity by the emitter's rotation
                    RotateVector(ref particle.Position, parent.RotationInRadians);
                    RotateVector(ref particle.Velocity, parent.RotationInRadians);
                    particle.RotationInRadians += parent.RotationInRadians;

                    // Adjust the particle's position by the emitter's location
                    particle.Position += emitterCoordinates;
                    particle.ReferencePosition = particle.Position;
                    
                    particleBuffer.Add(particle);            
                }
            }

            if (stopEmittingAfterUpdate)
            {
                parent.IsEmittingNewParticles = false;
                stopEmittingAfterUpdate = false;
            }
        }

        public int GetEstimatedCapacity()
        {
            var particlesPerTrigger = 0f;
            var triggersPerSecond = 0f;

            {
                triggersPerSecond = 1 / TimeElapsedTriggerFrequency;
            }
            {
                particlesPerTrigger = StaticParticleSpawnCount;
            }

            
            return (int) Math.Ceiling(particlesPerTrigger * triggersPerSecond * MaxParticleLifeTime);
        }

        private static void RotateVector(ref Vector2 vector, float radians)
        {
            if (vector == Vector2.Zero)
            {
                return;
            }
            
            var magnitude = vector.Length();
            var angle = Math.Atan2(vector.Y, vector.X);
            angle += radians;
            
            vector.X = magnitude * (float) Math.Cos(angle);
            vector.Y = magnitude * (float) Math.Sin(angle);
        }
    }
}
