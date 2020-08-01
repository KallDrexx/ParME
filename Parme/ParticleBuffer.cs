namespace Parme
{
    public class ParticleBuffer
    {
        private Particle[] _buffer;

        public ParticleBuffer(int initialCapacity)
        {
            _buffer = new Particle[initialCapacity];
        }

        public int Count => _buffer.Length;
        
        /// <summary>
        /// Gets the particle at the specified index.  Since this returns a particle by reference care should be
        /// taken to not hold onto this reference for too long, as it may point to an old particle in memory if
        /// an Add() call ends up resizing the buffer.
        /// </summary>
        public ref Particle this[int index] => ref _buffer[index];

        public void Add(Particle particle)
        {
            for (var index = 0; index < _buffer.Length; index++)
            {
                if (!_buffer[index].IsAlive)
                {
                    _buffer[index] = particle;
                    return;
                }
            }
            
            // We did not find an open slot, so we need to create a larger buffer
            var newBuffer = new Particle[_buffer.Length * 2];
            _buffer.CopyTo(newBuffer, 0);
            newBuffer[_buffer.Length] = particle;
            _buffer = newBuffer;
        }
    }
}