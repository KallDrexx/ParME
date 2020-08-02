namespace Parme
{
    public interface IEmitterLogic
    {
        void Update(ParticleBuffer particleBuffer, float timeSinceLastFrame);
    }
}