using Microsoft.Xna.Framework;

namespace Parme.Initializers.ColorInitializer
{
    public interface IColorInitializer
    {
        Color GetColorOperationForNextParticle();
        string GetCSharpExecutionCode();
    }
}