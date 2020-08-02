namespace Parme.Triggers
{
    public class TimeElapsedTrigger : ITrigger
    {
        private readonly float _frequency;
        private float _timeSinceLastTrigger;

        public TimeElapsedTrigger(float frequency)
        {
            _frequency = frequency;
        }

        public bool ShouldCreateNewParticles(float secondsSinceLastFrame)
        {
            _timeSinceLastTrigger += secondsSinceLastFrame;
            if (_timeSinceLastTrigger < _frequency)
            {
                return false;
            }

            _timeSinceLastTrigger = 0;
            return true;
        }

        public void Reset()
        {
            _timeSinceLastTrigger = 0;
        }
    }
}