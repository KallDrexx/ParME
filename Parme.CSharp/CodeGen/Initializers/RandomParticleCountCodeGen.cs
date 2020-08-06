using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    public class RandomParticleCountCodeGen : IGenerateCode<RandomParticleCountInitializer>
    {
        public string GenerateProperties(RandomParticleCountInitializer obj)
        {
            return $@"
        public int RandomParticleCountMinToSpawn {{ get; set; }} = {obj.MinimumToSpawn};
        public int RandomParticleCountMaxToSpawn {{ get; set; }} = {obj.MaximumToSpawn};
";
        }

        public string GenerateFields(RandomParticleCountInitializer obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(RandomParticleCountInitializer obj)
        {
            return @"newParticleCount = _random.Next(RandomParticleCountMinToSpawn, RandomParticleCountMaxToSpawn);
";
        }
    }
}