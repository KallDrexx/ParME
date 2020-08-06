using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class StaticParticleCountCodeGen : IGenerateCode<StaticParticleCountInitializer>
    {
        public string GenerateProperties(StaticParticleCountInitializer obj)
        {
            return $@"
        public int StaticParticleSpawnCount {{ get; set }} = {obj.ParticleSpawnCount};
";
        }

        public string GenerateFields(StaticParticleCountInitializer obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(StaticParticleCountInitializer obj)
        {
            return @"newParticleCount = StaticParticleSpawnCount;
";
        }
    }
}