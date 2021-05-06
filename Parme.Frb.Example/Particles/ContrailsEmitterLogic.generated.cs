
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

        public float RadialVelocityMinMagnitude { get; set; } = 50f;
        public float RadialVelocityMaxMagnitude { get; set; } = 50f;
        public float RadialVelocityMinRadians { get; set; } = 2.356194490192345f;
        public float RadialVelocityMaxRadians { get; set; } = 3.9269908169872414f; 

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
                        
                        particle.TextureSectionIndex = (byte) _random.Next(0, TextureSections.Length);
                    }
                    {
                        
                        particle.Position = new Vector2(StaticPositionXOffset, StaticPositionYOffset);
                    }
                    {
                        
                        particle.CurrentRed = (float) StaticColorStartingRed;
                        particle.CurrentGreen = (float) StaticColorStartingGreen;
                        particle.CurrentBlue = (float) StaticColorStartingBlue;
                        particle.CurrentAlpha = (float) StaticColorStartingAlpha;
                                }
                    {
                        particle.Size = new Vector2(StaticSizeWidth, StaticSizeHeight);
                    }
                    {
                        
                        var radians = RadialVelocityMaxRadians - _random.NextDouble() * (RadialVelocityMaxRadians - RadialVelocityMinRadians);
                        var magnitude = RadialVelocityMaxMagnitude - _random.NextDouble() * (RadialVelocityMaxMagnitude - RadialVelocityMinMagnitude);
                
                        // convert from polar coordinates to cartesian coordinates
                        var x = magnitude * Math.Cos(radians);
                        var y = magnitude * Math.Sin(radians);
                        particle.Velocity = new Vector2((float) x, (float) y);
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
                triggersPerSecond = 3;
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
