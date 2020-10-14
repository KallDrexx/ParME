using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Parme.CSharp.Benchmarks.Templates;

namespace Parme.CSharp.Benchmarks.Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net472)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    [IterationCount(100)]
    public class FireEmitter
    {
        private Emitter _emitter;

        [GlobalSetup]
        public void CreateEmitter()
        {
            var pool = new ParticlePool();
            var logic = Utilities.GetInstance(FireTemplate.Emitter);
            _emitter = new BenchmarkEmitter(logic, pool)
            {
                IsEmittingNewParticles = true
            };
        }

        [GlobalCleanup]
        public void DisposeEmitter()
        {
            _emitter.Dispose();
        }
        
        [Benchmark]
        public void FireEmitterUpdateCall()
        {
            _emitter.Update(1f/60);
        }
    }
}