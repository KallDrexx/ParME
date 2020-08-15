namespace Parme.Frb.Example
{
    public partial class Game1
    {
        GlueControlManager glueControlManager;
        partial void GeneratedInitialize () 
        {
            glueControlManager = new GlueControlManager(8021);
            glueControlManager.Start();
            this.Exiting += (not, used) => glueControlManager.Kill();
        }
        partial void GeneratedUpdate (Microsoft.Xna.Framework.GameTime gameTime) 
        {
        }
        partial void GeneratedDraw (Microsoft.Xna.Framework.GameTime gameTime) 
        {
        }
    }
}
