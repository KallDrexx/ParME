namespace Parme.Initializers.ParticleCountInitializer
{
    public interface IParticleCountInitializer
    {
        /// <summary>
        /// Determines how many particles should be created during this emission period
        /// </summary>
        int GetNewParticleCount();
    }
}