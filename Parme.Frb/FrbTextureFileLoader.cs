using FlatRedBall;
using Microsoft.Xna.Framework.Graphics;
using Parme.MonoGame;

namespace Parme.Frb
{
    public class FrbTextureFileLoader : ITextureFileLoader
    {
        public Texture2D LoadTexture2D(string path)
        {
            return FlatRedBallServices.Load<Texture2D>(path);
        }
    }
}