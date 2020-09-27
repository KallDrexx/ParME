using System.Numerics;

namespace Parme.CSharp
{
    public abstract class Emitter
    {
        protected readonly IEmitterLogic EmitterLogic;
        protected readonly ParticleBuffer ParticleBuffer;
        
        public Vector2 WorldCoordinates { get; set; }
        public float ScaleFactor { get; set; } = 1f;
        public bool IsEmittingNewParticles { get; set; }

        protected Emitter(IEmitterLogic emitterLogic)
        {
            EmitterLogic = emitterLogic;
            
            // TODO: find a way to estimate initial capacity from particle count initializer and trigger
            ParticleBuffer = new ParticleBuffer(50);
        }

        public void Update(float timeSinceLastFrame)
        {
            EmitterLogic.Update(ParticleBuffer, timeSinceLastFrame, this);
        }

        /// <summary>
        /// Begins emitting particles
        /// </summary>
        public void Start()
        {
            IsEmittingNewParticles = true;
        }

        /// <summary>
        /// Stops emitting new particles, but existing particles will still be active
        /// </summary>
        public void Stop()
        {
            IsEmittingNewParticles = false;
        }
        
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
            int count = 0;
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
        
        // ReSharper disable once UnusedMemberInSuper.Global
        public abstract void Render(ParticleCamera camera);
    }
}