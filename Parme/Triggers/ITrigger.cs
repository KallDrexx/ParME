namespace Parme.Triggers
{
    public interface ITrigger
    {
        bool ShouldCreateNewParticles(float secondsSinceLastFrame);
        void Reset();

        string GetCSharpFieldDefinitions();
        string GetCSharpCheckCode();
    }
}