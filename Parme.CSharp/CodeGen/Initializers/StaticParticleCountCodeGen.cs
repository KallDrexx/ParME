using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class StaticParticleCountCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticParticleCountInitializer);

        public string GenerateProperties(object obj)
        {
            var initializer = (StaticParticleCountInitializer) obj;
            
            return $@"
        public int StaticParticleSpawnCount {{ get; set; }} = {initializer.ParticleSpawnCount};
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"newParticleCount = StaticParticleSpawnCount;
";
        }

        public string GenerateCapacityEstimationCode(object obj)
        {
            return "particlesPerTrigger = StaticParticleSpawnCount;";
        }
    }
}