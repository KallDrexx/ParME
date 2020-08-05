namespace Parme.Core.Initializers
{
    public enum InitializerType
    {
        /// <summary>
        /// Default value, should not be used and will cause exceptions
        /// </summary>
        Unspecified = 0,
        
        /// <summary>
        /// An initializer that designates how many particles should be created each emission period
        /// </summary>
        ParticleCount = 1,
        
        /// <summary>
        /// An initializer that sets the color multiplier value for newly created particles
        /// </summary>
        ColorMultiplier = 2,
        
        /// <summary>
        /// An initializer that determines the position new particles are created at, in relation to the emitter
        /// </summary>
        Position = 3,
        
        /// <summary>
        /// An initializer that determines the size of newly created particles
        /// </summary>
        Size = 4,
        
        /// <summary>
        /// An initializer that determines the starting velocity of created particles
        /// </summary>
        Velocity = 5,
        
        /// <summary>
        /// An initializer that determines which part of a texture the particle should be rendered with 
        /// </summary>
        TextureId = 6,
    }
}