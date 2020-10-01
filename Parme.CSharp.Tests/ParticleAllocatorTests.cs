using Shouldly;
using Xunit;

namespace Parme.CSharp.Tests
{
    public class ParticleAllocatorTests
    {
        [Fact]
        public void Can_Reserve_Specified_Amount()
        {
            var allocator = new ParticlePool();
            var allocation = allocator.Reserve(123);
            
            allocation.ShouldNotBeNull();
            allocation.StartIndex.ShouldBe(0);
            allocation.LastUsedIndex.ShouldBe(122);
            allocation.Slice.Length.ShouldBe(123);
        }

        [Fact]
        public void Multiple_Allocations_Dont_Overlap()
        {
            var allocator = new ParticlePool();
            var first = allocator.Reserve(10);
            var second = allocator.Reserve(10);
            var third = allocator.Reserve(10);
            
            first.StartIndex.ShouldBe(0);
            first.LastUsedIndex.ShouldBe(9);
            second.StartIndex.ShouldBe(10);
            second.LastUsedIndex.ShouldBe(19);
            third.StartIndex.ShouldBe(20);
            third.LastUsedIndex.ShouldBe(29);
        }

        [Fact]
        public void Disposed_Allocation_Has_Memory_Reused()
        {
            var allocator = new ParticlePool();
            var first = allocator.Reserve(10);
            first.Dispose();

            var second = allocator.Reserve(10);
            
            second.StartIndex.ShouldBe(0);
            second.LastUsedIndex.ShouldBe(9);
        }

        [Fact]
        public void Allocation_Can_Be_Increased_In_Size()
        {
            var allocator = new ParticlePool();
            var allocation = allocator.Reserve(10);
            var initialSlice = allocation.Slice;

            for (var x = 0; x < allocation.Slice.Length; x++)
            {
                ref var particle = ref allocation.Slice.Span[x];
                particle.TimeAlive = x + 1;
            }
            
            allocation.IncreaseSize(5);
            
            allocation.StartIndex.ShouldBe(0);
            allocation.LastUsedIndex.ShouldBe(14);
            allocation.Slice.Length.ShouldBe(15);
            
            for (var x = 0; x < 10; x++)
            {
                allocation.Slice.Span[x].TimeAlive.ShouldBe(x + 1, $"Particle {x} had incorrect time alive");
            }
            
            initialSlice.ShouldNotBe(allocation.Slice);
        }

        [Fact]
        public void Can_Allocate_Single_Allocation_Beyond_Allocator_Capacity()
        {
            var allocator = new ParticlePool();
            var capacity = allocator.Capacity;
            var allocation = allocator.Reserve(capacity + 1);
            
            allocation.ShouldNotBeNull();
            allocation.StartIndex.ShouldBe(0);
            allocation.LastUsedIndex.ShouldBe(capacity);
            allocation.Slice.Length.ShouldBe(capacity + 1);
            allocator.Capacity.ShouldBeGreaterThan(capacity + 1);
        }

        [Fact]
        public void Can_Defrag_Allocations_If_Enough_Room_Is_Present()
        {
            var allocator = new ParticlePool();
            var initialCapacity = allocator.Capacity;
            var first = allocator.Reserve(initialCapacity / 3);
            var second = allocator.Reserve(initialCapacity / 3);
            var third = allocator.Reserve(initialCapacity / 3);
            first.Dispose();
            third.Dispose();

            var fourth = allocator.Reserve(initialCapacity / 3 + 10);
            
            second.StartIndex.ShouldBe(0);
            fourth.StartIndex.ShouldBe(second.LastUsedIndex + 1);
            allocator.Capacity.ShouldBe(initialCapacity);
        }

        [Fact]
        public void Allocation_Keeps_Values_After_Defrag()
        {
            var allocator = new ParticlePool();
            var initialCapacity = allocator.Capacity;
            var first = allocator.Reserve(initialCapacity / 3);
            var second = allocator.Reserve(initialCapacity / 3);
            var third = allocator.Reserve(initialCapacity / 3);
            var secondSlice = second.Slice;

            for (var x = 0; x < second.Slice.Length; x++)
            {
                ref var particle = ref second.Slice.Span[x];
                particle.TimeAlive = x + 1;
            }
            
            first.Dispose();
            third.Dispose();

            allocator.Reserve(initialCapacity / 3 + 10);

            for (var x = 0; x < second.Slice.Length; x++)
            {
                second.Slice.Span[x].TimeAlive.ShouldBe(x + 1, 0.0001);
            }
            
            secondSlice.ShouldNotBe(second.Slice);
        }

        [Fact]
        public void Allocation_Grows_If_Past_Capacity()
        {
            var allocator = new ParticlePool();
            var initialCapacity = allocator.Capacity;
            allocator.Reserve(10);
            allocator.Reserve(initialCapacity);

            allocator.Capacity.ShouldBeGreaterThan(initialCapacity);
        }

        [Fact]
        public void Allocation_Particles_Moved_When_Allocator_Grows()
        {
            var allocator = new ParticlePool();
            var initialCapacity = allocator.Capacity;
            var first = allocator.Reserve(10);
            var firstSlice = first.Slice;
            
            for (var x = 0; x < first.Slice.Length; x++)
            {
                ref var particle = ref first.Slice.Span[x];
                particle.TimeAlive = x + 1;
            }
            
            allocator.Reserve(initialCapacity);
            
            for (var x = 0; x < first.Slice.Length; x++)
            {
                first.Slice.Span[x].TimeAlive.ShouldBe(x + 1, 0.0001);
            }
            
            firstSlice.ShouldNotBe(first.Slice);
        }
    }
}