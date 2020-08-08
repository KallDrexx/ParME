using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class RandomParticleCountCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(RandomParticleCountInitializer);

        public string GenerateProperties(object obj)
        {
            var initializer = (RandomParticleCountInitializer) obj;
            
            return $@"
        public int RandomParticleCountMinToSpawn {{ get; set; }} = {initializer.MinimumToSpawn};
        public int RandomParticleCountMaxToSpawn {{ get; set; }} = {initializer.MaximumToSpawn};
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"newParticleCount = _random.Next(RandomParticleCountMinToSpawn, RandomParticleCountMaxToSpawn);
";
        }
    }
}