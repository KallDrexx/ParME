using Microsoft.Xna.Framework;

namespace Parme.Initializers.VelocityInitializers
{
    /// <summary>
    /// An initializer that's used to determine the velocity for new particles
    /// </summary>
    public interface IVelocityInitializer
    {
        Vector2 GetNewParticleVelocity();
    }
}