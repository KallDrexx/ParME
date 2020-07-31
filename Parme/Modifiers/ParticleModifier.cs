namespace Parme.Modifiers
{
    /// <summary>
    /// Represents a set of logic that should modify some aspect of a particle
    /// </summary>
    public interface IParticleModifier
    {
        void Update(float timeSinceLastFrame, Particle particle);
    }
}