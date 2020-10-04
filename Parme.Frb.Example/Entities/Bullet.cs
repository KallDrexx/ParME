using FlatRedBall;

namespace Parme.Frb.Example.Entities
{
    public partial class Bullet
    {
        private float _timeAlive;
        
        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
        }

        private void CustomActivity()
        {
            _timeAlive += TimeManager.SecondDifference;
            if (_timeAlive > LifetimeInSeconds)
            {
                Destroy();
            }
        }

        private void CustomDestroy()
        {
            DeathEmitter.IsEmitting = true;
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
        }
    }
}
