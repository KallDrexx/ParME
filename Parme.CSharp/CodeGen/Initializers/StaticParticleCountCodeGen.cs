using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class StaticParticleCountCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticParticleCountInitializer);

        public FormattableString GenerateProperties(object obj)
        {
            var initializer = (StaticParticleCountInitializer) obj;
            
            return $@"
        public int StaticParticleSpawnCount {{ get; set; }} = {initializer.ParticleSpawnCount};
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"newParticleCount = StaticParticleSpawnCount;
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"particlesPerTrigger = StaticParticleSpawnCount;";
        }
    }
}