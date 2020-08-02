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

        public string GetCSharpFieldDefinitions()
        {
            return $@"
        private bool _hasTriggered;
";
        }

        public string GetCSharpCheckCode()
        {
            return $@"
            if (_hasTriggered)
            {{
                shouldCreateNewParticle = false;
            }}
            else
            {{
                shouldCreateNewParticle = true;
                _hasTriggered = true;
            }}
";
        }
    }
}