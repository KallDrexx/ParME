using System;

namespace Parme.CSharp
{
    public class ParticleBuffer
    {
        private const double ScaleFactor = 1.5;
        
        private Particle[] _buffer;
        private int _firstActiveIndex = 0, _lastActiveIndex = 0;

        public ParticleBuffer(int initialCapacity)
        {
            _buffer = new Particle[initialCapacity];
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
                ConstrainActiveArea();
                return _buffer.AsSpan(_firstActiveIndex, _lastActiveIndex - _firstActiveIndex + 1);
            }
        }

        public void Add(Particle particle)
        {
            ConstrainActiveArea();

            if (_firstActiveIndex > 0)
            {
                _firstActiveIndex--;
                _buffer[_firstActiveIndex] = particle;
                
                return;
            }

            if (_lastActiveIndex < _buffer.Length - 1)
            {
                _lastActiveIndex++;
                _buffer[_lastActiveIndex] = particle;

                return;
            }
            
            // If we got here there's no room left
            var newBuffer = new Particle[(int)(_buffer.Length * ScaleFactor)];
            _buffer.CopyTo(newBuffer, 0);
            newBuffer[_buffer.Length] = particle;
            _buffer = newBuffer;

            _lastActiveIndex++;
        }

        private void ConstrainActiveArea()
        {
            while (!_buffer[_firstActiveIndex].IsAlive && _firstActiveIndex < _lastActiveIndex)
            {
                _firstActiveIndex++;
            }

            while (!_buffer[_lastActiveIndex].IsAlive && _firstActiveIndex < _lastActiveIndex)
            {
                _lastActiveIndex--;
            }
        }
    }
}