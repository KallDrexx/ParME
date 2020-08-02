namespace Parme.Initializers.ParticleCountInitializer
{
    public class StaticParticleCountInitializer : IParticleCountInitializer
    {
        private readonly int _count;

        public StaticParticleCountInitializer(int count)
        {
            _count = count;
        }

        public int GetNewParticleCount()
        {
            return _count;
        }

        public string GetCSharpExecutionCode()
        {
            return $"newParticleCount = {_count};";
        }
    }
}