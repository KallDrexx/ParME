using System;

namespace Parme.CSharp.Benchmarks
{
    public class BenchmarkEmitter : Emitter
    {
        public BenchmarkEmitter(IEmitterLogic emitterLogic, ParticlePool particlePool, Random random) 
            : base(emitterLogic, particlePool, random)
        {
        }
    }
}