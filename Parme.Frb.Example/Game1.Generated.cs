namespace Parme.Frb.Example
{
    public partial class Game1
    {
        partial void GeneratedInitialize () 
        {
            var emitterNameMapper = new Parme.Frb.Example.Particles.ParmeEmitterLogicGenerator();
            Parme.Frb.ParmeEmitterManager.Instance.EmitterLogicMapper = emitterNameMapper;
        }
        partial void GeneratedUpdate (Microsoft.Xna.Framework.GameTime gameTime) 
        {
        }
        partial void GeneratedDraw (Microsoft.Xna.Framework.GameTime gameTime) 
        {
        }
    }
}
