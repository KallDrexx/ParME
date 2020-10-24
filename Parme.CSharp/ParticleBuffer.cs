using System;

namespace Parme.CSharp
{
    public class ParticleBuffer : IDisposable
    {
        private const double ScaleFactor = 1.5;

        private readonly ParticlePool.Reservation _particles;
        private int _firstActiveIndex = 0, _lastActiveIndex = 0;
        private bool _isDisposed;

        public ParticleBuffer(ParticlePool particlePool, int initialCapacity)
        {
            _particles = particlePool.Reserve(initialCapacity);
        }

        /// <summary>
        /// Returns the splice of active particles.  It is guaranteed that no particles outside the slice are alive
        /// but it is *not* guaranteed that all particles inside the splice are alive, so code should keep this in
        /// mind.  The splice is meant to be held only for a short period of time to support accessing particles by
        /// reference, and may be invalidated by an `Add()` call if the Add call results in resizing the buffer,
        /// and thus the splice might point to old/irrelevant particles.
        /// </summary>
        public Span<Particle> Particles
        {
            get
            {
                if (_isDisposed)
                {
                    const string message = "Attempted to get particles from disposed particle buffer";
                    throw new InvalidOperationException(message);
                }
                
                ConstrainActiveArea();
                return _particles.Slice.Span.Slice(_firstActiveIndex, _lastActiveIndex - _firstActiveIndex + 1);
            }
        }

        public void Add(Particle particle)
        {
            if (_isDisposed)
            {
                const string message = "Attempted to add particles to disposed particle buffer";
                throw new InvalidOperationException(message);
            }
            
            ConstrainActiveArea();

            if (_firstActiveIndex > 0)
            {
                _firstActiveIndex--;
                _particles.Slice.Span[_firstActiveIndex] = particle;
                
                return;
            }

            if (_lastActiveIndex < _particles.Slice.Length - 1)
            {
                _lastActiveIndex++;
                _particles.Slice.Span[_lastActiveIndex] = particle;

                return;
            }
            
            // If we got here there's no room left
            var additionalRequested = _particles.Slice.Length * ScaleFactor - _particles.Slice.Length;
            _particles.IncreaseSize((int) Math.Ceiling(additionalRequested));
            
            _lastActiveIndex++;
            _particles.Slice.Span[_lastActiveIndex] = particle;
        }

        public void Dispose()
        {
            _particles?.Dispose();
            _isDisposed = true;
        }

        private void ConstrainActiveArea()
        {
            while (!_particles.Slice.Span[_firstActiveIndex].IsAlive && _firstActiveIndex < _lastActiveIndex)
            {
                _firstActiveIndex++;
            }

            while (!_particles.Slice.Span[_lastActiveIndex].IsAlive && _firstActiveIndex < _lastActiveIndex)
            {
                _lastActiveIndex--;
            }
        }
    }
}