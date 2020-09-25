
using System;
using System.Numerics;
using Parme.Core;
using Parme.CSharp;

namespace Parme.Frb.Example
{

    public class purpleEmitterLogic : IEmitterLogic
    {
        private readonly Random _random = new Random();
        private float _timeSinceLastTrigger;

        public float MaxParticleLifeTime { get; set; } = 1f;
        
        public string TextureFilePath { get; } = @"Content\GlobalContent\SampleParticles2.png";
        public TextureSectionCoords[] TextureSections { get; } = new TextureSectionCoords[] {
        };
        
        
        public float TimeElapsedTriggerFrequency { get; set; } = 1f; 

        public float StaticColorRedMultiplier { get; set; } = 1f;
        public float StaticColorGreenMultiplier { get; set; } = 0.64705884f;
        public float StaticColorBlueMultiplier { get; set; } = 1f;
        public float StaticColorAlphaMultiplier { get; set; } = 1f;

        public float RandomRegionPositionMinXOffset { get; set; } = 0f;
        public float RandomRegionPositionMaxXOffset { get; set; } = 0f;
        public float RandomRegionPositionMinYOffset { get; set; } = 0f;
        public float RandomRegionPositionMaxYOffset { get; set; } = 0f;

        public int StaticSizeWidth { get; set; } = 50;
        public int StaticSizeHeight { get; set; } = 50;

        public int StaticParticleSpawnCount { get; set; } = 1;

        public float RandomRangeVelocityMinX { get; set; } = 0f;
        public float RandomRangeVelocityMaxX { get; set; } = 0f;
        public float RandomRangeVelocityMinY { get; set; } = 0f;
        public float RandomRangeVelocityMaxY { get; set; } = 0f; 

        public float ConstantRotationRadiansPerSecond { get; set; } = 6.283185307179586f;

        
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

                
                particle.Position += particle.Velocity;
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
                        
                        particle.RedMultiplier = StaticColorRedMultiplier;
                        particle.GreenMultiplier = StaticColorGreenMultiplier;
                        particle.BlueMultiplier = StaticColorBlueMultiplier;
                        particle.AlphaMultiplier = StaticColorAlphaMultiplier;
                                }
                    {
                        
                        var x = RandomRegionPositionMaxXOffset - _random.NextDouble() * (RandomRegionPositionMaxXOffset - RandomRegionPositionMinXOffset);
                        var y = RandomRegionPositionMaxYOffset - _random.NextDouble() * (RandomRegionPositionMaxYOffset - RandomRegionPositionMinYOffset);
                        particle.Position = new Vector2((float) x, (float) y);
                    }
                    {
                        particle.Size = new Vector2(StaticSizeWidth, StaticSizeHeight);
                    }
                    {
                        
                        var x = RandomRangeVelocityMaxX - _random.NextDouble() * (RandomRangeVelocityMaxX - RandomRangeVelocityMinX);
                        var y = RandomRangeVelocityMaxY - _random.NextDouble() * (RandomRangeVelocityMaxY - RandomRangeVelocityMinY);
                        particle.Velocity = new Vector2((float) x, (float) y);
                    }


                    // Adjust the particle's position by the emitter's location
                    particle.Position += emitterCoordinates;
                    
                    particleBuffer.Add(particle);            
                }
            }
        }
    }
}
