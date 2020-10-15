using Parme.Core;

namespace Parme.CSharp
{
    public interface IEmitterLogic
    {
        string TextureFilePath { get; }
        TextureSectionCoords[] TextureSections { get; }
        void Update(ParticleBuffer particleBuffer, float timeSinceLastFrame, Emitter parent);
        int GetEstimatedCapacity();
    }
}