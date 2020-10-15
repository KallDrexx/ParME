using System;
using System.Numerics;

namespace Parme.CSharp
{
    public abstract class Emitter : IDisposable
    {
        protected readonly IEmitterLogic EmitterLogic;
        protected readonly ParticleBuffer ParticleBuffer;
        private bool _isDisposed;
        
        public Vector2 WorldCoordinates { get; set; }
        public float RotationInRadians { get; set; }
        public bool IsEmittingNewParticles { get; set; }
        
        // ReSharper disable once MemberCanBeProtected.Global
        public Vector2 FullTextureSize { get; protected set; }

        protected Emitter(IEmitterLogic emitterLogic, ParticlePool particlePool)
        {
            EmitterLogic = emitterLogic ?? throw new ArgumentNullException(nameof(emitterLogic));

            var initialCapacity = emitterLogic.GetEstimatedCapacity();
            ParticleBuffer = new ParticleBuffer(particlePool, initialCapacity);
        }

        public void Update(float timeSinceLastFrame)
        {
            if (_isDisposed)
            {
                const string message = "Attempted to update a disposed emitter";
                throw new InvalidOperationException(message);
            }
            
            EmitterLogic.Update(ParticleBuffer, timeSinceLastFrame, this);
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
        public void KillAllParticles()
        {
            var particles = ParticleBuffer.Particles;
            for (var x = 0; x < particles.Length; x++)
            {
                ref var particle = ref particles[x];
                particle.IsAlive = false;
            }
        }

        public int CalculateLiveParticleCount()
        {
            var count = 0;
            var particles = ParticleBuffer.Particles;
            for (var x = 0; x < particles.Length; x++)
            {
                if (particles[x].IsAlive)
                {
                    count++;
                }
            }

            return count;
        }

        public void Dispose()
        {
            IsEmittingNewParticles = false;
            KillAllParticles();
            ParticleBuffer.Dispose();
            _isDisposed = true;
        }
    }
}