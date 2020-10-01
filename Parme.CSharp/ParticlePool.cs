using System;
using System.Collections.Generic;
using System.Linq;

namespace Parme.CSharp
{
    /// <summary>
    /// A re-usable set of particles.  This allows emitters to be created and destroyed while the collection of
    /// particles they rely on can be re-used by the next emitter that comes online.  This is meant to help with
    /// garbage collection pressure and keep the actual emitter objects as small as possible.
    ///
    /// When an emitter is created it will reserve a certain capacity of particles from the pool.  Those particles
    /// will only be usable by that emitter until the emitter releases it's reservation (via `IDispose`).  It is
    /// *important* that all emitters that reserve a slice of the pool are disposed, or else the pool will never know
    /// that the slot allotted to them is available, and thus memory leaks will occur.
    ///
    /// The pool is not thread safe, and no code should hold onto memory slices from the pool for more than
    /// a short period of time, as anytime the allocation pool grows all slices may be invalidated and point to
    /// old particles.
    /// </summary>
    public class ParticlePool
    {
        private const double ScaleFactor = 1.25;
        
        private Particle[] _particles = new Particle[1000];
        private SortedSet<Reservation> _allocations = new SortedSet<Reservation>(new ReservationComparer());

        public int Capacity => _particles.Length;

        internal Reservation Reserve(int capacityRequested)
        {
            Reservation CreateAllocation(int startIndex1)
            {
                var slice = _particles.AsMemory(startIndex1, capacityRequested);
                var lastIndex = startIndex1 + capacityRequested - 1;
                var reservation = new Reservation(this, startIndex1, lastIndex, slice);
                _allocations.Add(reservation);

                return reservation;
            }

            if (capacityRequested <= 0)
            {
                throw new InvalidOperationException("A positive capacity value is required for reservation");
            }
            
            // Try and find a gap that's big enough in the existing particle array
            var totalReserved = 0;
            var startIndex = 0;
            var nextUnavailableIndex = _particles.Length; // Default to the end in case there are no reservations
            foreach (var reservation in _allocations)
            {
                totalReserved += reservation.LastUsedIndex - reservation.StartIndex + 1; // inclusive
                nextUnavailableIndex = reservation.StartIndex;
                if (nextUnavailableIndex - startIndex >= capacityRequested)
                {
                    // Gap found
                    break;
                }
                
                // Gap not found
                startIndex = reservation.LastUsedIndex + 1;
                nextUnavailableIndex = _particles.Length;
            }

            if (nextUnavailableIndex - startIndex >= capacityRequested)
            {
                // Make the reservation from here
                return CreateAllocation(startIndex);
            }
            
            // We don't have an open slot to put this in.  This could be a case of fragmentation, in which case we
            // don't necessarily need to grow the array, just compact it.  However, even if it's fragmented we want 
            // to make sure there's at least some free space left over after compaction, or else it's pretty likely
            // we will either constantly be fragmented or have to grow very soon anyway.
            const double minFragmentationFree = 0.05;
            var totalFree = _particles.Length - totalReserved;
            var isFragmented = totalFree >= capacityRequested;
            var freePercentageAfterAddition = (totalFree - capacityRequested) / (double) _particles.Length;
            
            var additionalCapacity = isFragmented && freePercentageAfterAddition > minFragmentationFree
                ? 0 // de-frag only
                : (int) Math.Max(_particles.Length * ScaleFactor - _particles.Length, capacityRequested);
            
            MoveParticlesToNewArray(additionalCapacity);

            if (_allocations.Any())
            {
                startIndex = _allocations.Max.LastUsedIndex + 1;
            }
            else
            {
                // We had to resize the array even though no allocations are present
                startIndex = 0;
            }
            
            return CreateAllocation(startIndex);
        }

        private void MoveParticlesToNewArray(int additionalCapacity)
        {
            Particle[] newParticleArray;
            if (additionalCapacity > 0)
            {
                var newCapacity = _particles.Length + additionalCapacity;
                newParticleArray = new Particle[(int) newCapacity];
            }
            else
            {
                // We can compact in place since reservations are sorted in order
                newParticleArray = _particles;
            }

            var newReservations = new SortedSet<Reservation>(new ReservationComparer());
            
            // Point reservations into the new array
            var nextAvailableIndex = 0;
            foreach (var allocation in _allocations)
            {
                if (newParticleArray != _particles || nextAvailableIndex != allocation.StartIndex)
                {
                    // We are moving allocations to a new particle array, or a new position within the array
                    var count = allocation.LastUsedIndex - allocation.StartIndex + 1;
                    var newStart = nextAvailableIndex;
                    var newEnd = nextAvailableIndex + count - 1;
                    var newSlice = newParticleArray.AsMemory(newStart, count);
                
                    allocation.Slice.CopyTo(newSlice);
                    allocation.SwapSlice(newStart, newEnd, newSlice);
                }

                newReservations.Add(allocation);
                nextAvailableIndex = allocation.LastUsedIndex + 1;
            }

            _particles = newParticleArray;
            _allocations = newReservations;
        }

        internal class Reservation : IDisposable
        {
            private readonly ParticlePool _pool;
            
            public int StartIndex { get; private set; }
            public int LastUsedIndex { get; private set; }
            public Memory<Particle> Slice { get; private set; }

            public Reservation(ParticlePool pool, int startIndex, int lastUsedIndex, Memory<Particle> slice)
            {
                _pool = pool;
                StartIndex = startIndex;
                LastUsedIndex = lastUsedIndex;
                Slice = slice;
                
                // Since this is a brand new reservation, we need to make sure all particles are reset to not be
                // alive.
                for (var x = 0; x < slice.Length; x++)
                {
                    ref var particle = ref slice.Span[x];
                    particle.IsAlive = false;
                }
            }

            public void SwapSlice(int newStartIndex, int newLastIndex, Memory<Particle> newSlice)
            {
                Slice = newSlice;
                StartIndex = newStartIndex;
                LastUsedIndex = newLastIndex;
            }

            public void IncreaseSize(int additionalCapacity)
            {
                if (additionalCapacity <= 0)
                {
                    return;
                }

                // Creating a new reservation may overwrite the particles this slice is using, so we have to
                // make a temporary copy of all of our current particles.
                var temp = new Particle[Slice.Length];
                Slice.CopyTo(temp);

                _pool._allocations.Remove(this);

                // This is ugly but I'm not in the mood to refactor the reservation logic atm
                var newAllocation = _pool.Reserve(temp.Length + additionalCapacity);
                _pool._allocations.Remove(newAllocation);
                StartIndex = newAllocation.StartIndex;
                LastUsedIndex = newAllocation.LastUsedIndex;
                Slice = newAllocation.Slice;
                temp.CopyTo(Slice);

                _pool._allocations.Add(this);
            }

            public void Dispose()
            {
                _pool._allocations.Remove(this);
            }
        }

        private class ReservationComparer : IComparer<Reservation>
        {
            public int Compare(Reservation x, Reservation y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                var startIndexComparison = x.StartIndex.CompareTo(y.StartIndex);
                return startIndexComparison != 0 
                    ? startIndexComparison 
                    : x.LastUsedIndex.CompareTo(y.LastUsedIndex);
            }
        }
    }
}