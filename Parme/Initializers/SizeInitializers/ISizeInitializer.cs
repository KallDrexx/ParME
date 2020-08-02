using Microsoft.Xna.Framework;

namespace Parme.Initializers.SizeInitializers
{
    public interface ISizeInitializer
    {
        Vector2 GetNextParticleSize();
        string GetCSharpExecutionCode();
    }
}