namespace Parme.Core.PositionModifier
{
    /// <summary>
    /// Represents a special modifier that is responsible for calculating a particle's position every update.  If an
    /// emitter does not have a position modifier set the position will be updated based on the particle's velocity.
    /// Position modifiers are the last modifiers to run, so it can be expected that velocity, acceleration, and
    /// all other calculations that might influence a particle's position has been applied.
    /// </summary>
    public interface IParticlePositionModifier : IEditorObject
    {
        
    }
}