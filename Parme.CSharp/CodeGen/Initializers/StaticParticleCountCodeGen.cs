using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class StaticParticleCountCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(StaticParticleCountInitializer);

        public override FormattableString GenerateProperties(object obj)
        {
            var initializer = (StaticParticleCountInitializer) obj;
            
            return $@"
        public int StaticParticleSpawnCount {{ get; set; }} = {initializer.ParticleSpawnCount};
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"newParticleCount = StaticParticleSpawnCount;
";
        }

        public override FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"particlesPerTrigger = StaticParticleSpawnCount;";
        }
    }
}