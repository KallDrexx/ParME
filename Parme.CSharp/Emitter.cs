namespace Parme.CSharp
{
    public abstract class Emitter
    {
        private readonly IEmitterLogic _emitterLogic;
        protected readonly ParticleBuffer ParticleBuffer;

        public bool IsEmittingNewParticles { get; private set; }

        protected Emitter(IEmitterLogic emitterLogic)
        {
            _emitterLogic = emitterLogic;
            
            // TODO: find a way to estimate initial capacity from particle count initializer and trigger
            ParticleBuffer = new ParticleBuffer(50);
        }

        public void Update(float timeSinceLastFrame)
        {
            _emitterLogic.Update(ParticleBuffer, timeSinceLastFrame, this);
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
        
        public abstract void Render();
    }
}