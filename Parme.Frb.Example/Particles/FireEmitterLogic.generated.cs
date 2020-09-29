
using System;
using System.Numerics;
using Parme.Core;
using Parme.CSharp;

namespace Parme.Frb.Example
{

    public class FireEmitterLogic : IEmitterLogic
    {
        private readonly Random _random = new Random();
        private float _timeSinceLastTrigger;

        public float MaxParticleLifeTime { get; set; } = 1f;
        
        public string TextureFilePath { get; } = @"";
        public TextureSectionCoords[] TextureSections { get; } = new TextureSectionCoords[] {
        };
        
        
        public float TimeElapsedTriggerFrequency { get; set; } = 0.01f; 

        public int RandomParticleCountMinToSpawn { get; set; } = 0;
        public int RandomParticleCountMaxToSpawn { get; set; } = 5;

        public byte StaticColorStartingRed { get; set; } = 255;
        public byte StaticColorStartingGreen { get; set; } = 165;
        public byte StaticColorStartingBlue { get; set; } = 0;
        public byte StaticColorStartingAlpha { get; set; } = 255;

        public float RandomRangeVelocityMinX { get; set; } = 0f;
        public float RandomRangeVelocityMaxX { get; set; } = 0f;
        public float RandomRangeVelocityMinY { get; set; } = 100f;
        public float RandomRangeVelocityMaxY { get; set; } = 200f; 

        public float RandomRegionPositionMinXOffset { get; set; } = -25f;
        public float RandomRegionPositionMaxXOffset { get; set; } = 25f;
        public float RandomRegionPositionMinYOffset { get; set; } = 20f;
        public float RandomRegionPositionMaxYOffset { get; set; } = -20f;

        public int StaticSizeWidth { get; set; } = 10;
        public int StaticSizeHeight { get; set; } = 10;

        public float ConstantRotationRadiansPerSecond { get; set; } = 1.7453292519943295f;

        public float ConstantAccelerationX { get; set; } = -75f;
        public float ConstantAccelerationY { get; set; } = 0f;

        public float ConstantSizeWidthChangePerSecond { get; set; } = -5f;
        public float ConstantSizeHeightChangePerSecond { get; set; } = -5f;

        public byte EndingColorRed { get; set; } = 255;
        public byte EndingColorGreen { get; set; } = 165;
        public byte EndingColorBlue { get; set; } = 0;
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
                        particle.RotationInRadians += timeSinceLastFrame * ConstantRotationRadiansPerSecond;
                }
                {
                        particle.Velocity += timeSinceLastFrame * new Vector2(ConstantAccelerationX, ConstantAccelerationY);
                }
                {
                        particle.Size += timeSinceLastFrame * new Vector2(ConstantSizeWidthChangePerSecond, ConstantSizeHeightChangePerSecond);
                }
                {
                        particle.CurrentRed -= (byte) (((particle.InitialRed - EndingColorRed) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentGreen -= (byte) (((particle.InitialGreen - EndingColorGreen) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentBlue -= (byte) (((particle.InitialBlue - EndingColorBlue) / MaxParticleLifeTime) * timeSinceLastFrame);
                        particle.CurrentAlpha -= (byte) (((particle.InitialAlpha - EndingColorAlpha) / MaxParticleLifeTime) * timeSinceLastFrame);
                }

                
                particle.Position += particle.Velocity * timeSinceLastFrame;
            }
            
            var shouldCreateNewParticle = false;
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
                        newParticleCount = _random.Next(RandomParticleCountMinToSpawn, RandomParticleCountMaxToSpawn);
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
                        
                        var x = RandomRangeVelocityMaxX - _random.NextDouble() * (RandomRangeVelocityMaxX - RandomRangeVelocityMinX);
                        var y = RandomRangeVelocityMaxY - _random.NextDouble() * (RandomRangeVelocityMaxY - RandomRangeVelocityMinY);
                        particle.Velocity = new Vector2((float) x, (float) y);
                    }
                    {
                        
                        var x = RandomRegionPositionMaxXOffset - _random.NextDouble() * (RandomRegionPositionMaxXOffset - RandomRegionPositionMinXOffset);
                        var y = RandomRegionPositionMaxYOffset - _random.NextDouble() * (RandomRegionPositionMaxYOffset - RandomRegionPositionMinYOffset);
                        particle.Position = new Vector2((float) x, (float) y);
                    }
                    {
                        particle.Size = new Vector2(StaticSizeWidth, StaticSizeHeight);
                    }


                    // Adjust the particle's position by the emitter's location
                    particle.Position += emitterCoordinates;
                    
                    particleBuffer.Add(particle);            
                }
            }
        }
    }
}
