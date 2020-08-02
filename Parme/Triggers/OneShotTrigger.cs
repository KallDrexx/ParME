namespace Parme.Triggers
{
    public class OneShotTrigger : ITrigger
    {
        private bool _hasTriggered;

        public bool ShouldCreateNewParticles(float secondsSinceLastFrame)
        {
            if (_hasTriggered)
            {
                return false;
            }

            _hasTriggered = true;
            return true;
        }

        public void Reset()
        {
            _hasTriggered = false;
        }
    }
}