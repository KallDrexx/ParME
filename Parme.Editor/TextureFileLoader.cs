using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Parme.MonoGame;

namespace Parme.Editor
{
    public class TextureFileLoader : ITextureFileLoader
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly ApplicationState _applicationState;

        public TextureFileLoader(GraphicsDevice graphicsDevice, ApplicationState applicationState)
        {
            _graphicsDevice = graphicsDevice;
            _applicationState = applicationState;
        }

        public Texture2D LoadTexture2D(string path)
        {
            // All file names should be relative to the emitter
            var emitterPath = Path.GetDirectoryName(_applicationState.ActiveFileName);
            var fullPath = Path.Combine(emitterPath!, path);
            
            return Texture2D.FromFile(_graphicsDevice, fullPath);
        }
    }
}