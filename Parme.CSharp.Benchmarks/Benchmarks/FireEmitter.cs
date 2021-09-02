using System;
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
        private ParticlePool _pool;
        private IEmitterLogic _preInstantiatedLogic;
        private Emitter _emitter;

        [GlobalSetup]
        public void CreateEmitter()
        {
            _pool = new ParticlePool();
            _preInstantiatedLogic = Utilities.GetInstance(FireTemplate.Emitter);
            _emitter = new BenchmarkEmitter(_preInstantiatedLogic, _pool, new Random())
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

        [Benchmark]
        public void CreateEmitterWithReUsedLogic()
        {
            using var emitter = new BenchmarkEmitter(_preInstantiatedLogic, _pool, new Random());
            emitter.Update(1f/60);
        }
    }
}