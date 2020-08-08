using Shouldly;
using Xunit;

namespace Parme.CSharp.Tests
{
    public class ParticleBufferTests
    {
        [Fact]
        public void Can_Fit_More_Particles_Than_Initial_Capacity()
        {
            var buffer = new ParticleBuffer(10);
            for (var x = 0; x < 11; x++)
            {
                buffer.Add(new Particle{IsAlive = true});
            }
            
            buffer.Particles.Length.ShouldBe(11);
        }

        [Fact]
        public void Dead_Particles_At_Edge_Are_Not_Returned_In_Particle_List()
        {
            var buffer = new ParticleBuffer(10);
            for (var x = 0; x < 10; x++)
            {
                var isDead = x == 0 || x == 9;
                buffer.Add(new Particle{IsAlive = !isDead});
            }

            buffer.Particles.Length.ShouldBe(8);
        }
    }
}