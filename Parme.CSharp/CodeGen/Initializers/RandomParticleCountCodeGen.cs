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

            // Make sure min and max values are in the actual correct properties, otherwise an exception will
            // occur with `_random.Next()` calls.
            var min = Math.Min(initializer.MinimumToSpawn, initializer.MaximumToSpawn);
            var max = Math.Max(initializer.MinimumToSpawn, initializer.MaximumToSpawn);

            return $@"
        public int RandomParticleCountMinToSpawn {{ get; set; }} = {min};
        public int RandomParticleCountMaxToSpawn {{ get; set; }} = {max};
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"newParticleCount = _random.Next(RandomParticleCountMinToSpawn, RandomParticleCountMaxToSpawn + 1);
";
        }

        public string GenerateCapacityEstimationCode(object obj)
        {
            return @"var difference = RandomParticleCountMaxToSpawn - RandomParticleCountMinToSpawn;
                var twoThirds = difference * 0.65f;
                particlesPerTrigger = RandomParticleCountMaxToSpawn - twoThirds;";
        }
    }
}