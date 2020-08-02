namespace Parme.Initializers.ParticleCountInitializer
{
    public interface IParticleCountInitializer
    {
        int GetNewParticleCount();
        string GetCSharpExecutionCode();
    }
}