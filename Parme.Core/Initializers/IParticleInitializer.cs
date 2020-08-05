namespace Parme.Core.Initializers
{
    public interface IParticleInitializer
    {
        /// <summary>
        /// Provides the type of initialization functionality being provided
        /// </summary>
        InitializerType InitializerType { get; }
    }
}