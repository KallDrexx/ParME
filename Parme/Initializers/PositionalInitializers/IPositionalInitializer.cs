using Microsoft.Xna.Framework;

namespace Parme.Initializers.PositionalInitializers
{
    /// <summary>
    /// An initializer that determines where a new particle should be positioned in relation to its emitter
    /// </summary>
    public interface IPositionalInitializer
    {
        Vector2 GetNewParticlePosition();
    }
}