
using System;
using System.Numerics;
using Parme.Core;
using Parme.CSharp;

namespace Parme.Frb.Example
{

    public class FireExample : IEmitterLogic
    {
        private readonly Random _random = new Random();
        private float _timeSinceLastTrigger;

        public int MaxParticleLifeTime { get; set; } = 1;
        
        public string TextureFilePath { get; } = @"Content\SampleParticles.png";
        public TextureSectionCoords[] TextureSections { get; } = new TextureSectionCoords[] {
            new TextureSectionCoords(0, 64, 31, 96),
            new TextureSectionCoords(32, 64, 63, 96),
            new TextureSectionCoords(64, 64, 95, 96),
            new TextureSectionCoords(0, 96, 31, 127),
            new TextureSectionCoords(32, 96, 63, 127),
            new TextureSectionCoords(64, 96, 95, 127),
        };
        
        
        public float TimeElapsedTriggerFrequency { get; set; } = 0.01f; 

        public int RandomParticleCountMinToSpawn { get; set; } = 0;
        public int RandomParticleCountMaxToSpawn { get; set; } = 5;

        public float StaticColorRedMultiplier { get; set; } = 1f;
        public float StaticColorGreenMultiplier { get; set; } = 0.64705884f;
        public float StaticColorBlueMultiplier { get; set; } = 0f;
        public float StaticColorAlphaMultiplier { get; set; } = 1f;

        public float RandomRangeVelocityMinX { get; set; } = 0f;
        public float RandomRangeVelocityMaxX { get; set; } = 0f;
        public float RandomRangeVelocityMinY { get; set; } = 2f;
        public float RandomRangeVelocityMaxY { get; set; } = 5f; 

        public float RandomRegionPositionMinXOffset { get; set; } = -25f;
        public float RandomRegionPositionMaxXOffset { get; set; } = 25f;
        public float RandomRegionPositionMinYOffset { get; set; } = -50f;
        public float RandomRegionPositionMaxYOffset { get; set; } = -50f;

        public int StaticSizeWidth { get; set; } = 100;
        public int StaticSizeHeight { get; set; } = 100;

        public float ConstantRotationRadiansPerSecond { get; set; } = 1.7453292519943295f;

        public float ConstantAccelerationX { get; set; } = -5f;
        public float ConstantAccelerationY { get; set; } = 5f;

        public float ConstantSizeWidthChangePerSecond { get; set; } = -100f;
        public float ConstantSizeHeightChangePerSecond { get; set; } = -100f;

        public float ConstantColorRedMultiplierChangePerSecond { get; set; } = -1f;
        public float ConstantColorGreenMultiplierChangePerSecond { get; set; } = -1f;
        public float ConstantColorBlueMultiplierChangePerSecond { get; set; } = -1f;
        public float ConstantColorAlphaMultiplierChangePerSecond { get; set; } = -1f;

        
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
                        particle.RedMultiplier += timeSinceLastFrame * ConstantColorRedMultiplierChangePerSecond;
                        particle.GreenMultiplier += timeSinceLastFrame * ConstantColorGreenMultiplierChangePerSecond;
                        particle.BlueMultiplier += timeSinceLastFrame * ConstantColorBlueMultiplierChangePerSecond;
                        particle.AlphaMultiplier += timeSinceLastFrame * ConstantColorAlphaMultiplierChangePerSecond;
                }
                {
                        
                        particle.TextureSectionIndex = (byte) ((particle.TimeAlive / MaxParticleLifeTime) * 
                                                               (TextureSections.Length / MaxParticleLifeTime));
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
                        
                        particle.RedMultiplier = StaticColorRedMultiplier;
                        particle.GreenMultiplier = StaticColorGreenMultiplier;
                        particle.BlueMultiplier = StaticColorBlueMultiplier;
                        particle.AlphaMultiplier = StaticColorAlphaMultiplier;
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
