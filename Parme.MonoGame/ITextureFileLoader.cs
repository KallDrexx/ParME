using Microsoft.Xna.Framework.Graphics;

namespace Parme.MonoGame
{
    public interface ITextureFileLoader
    {
        Texture2D LoadTexture2D(string path);
    }
}