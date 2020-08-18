using Microsoft.Xna.Framework.Graphics;
using Parme.MonoGame;

namespace Parme.Editor
{
    public class TextureFileLoader : ITextureFileLoader
    {
        private readonly GraphicsDevice _graphicsDevice;

        public TextureFileLoader(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public Texture2D LoadTexture2D(string path)
        {
            return Texture2D.FromFile(_graphicsDevice, path);
        }
    }
}