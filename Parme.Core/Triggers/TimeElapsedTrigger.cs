namespace Parme.Core.Triggers
{
    public class TimeElapsedTrigger : IParticleTrigger
    {
        public float Frequency { get; set; } = 1f; // default of 1 so new instances don't spam particles too much
    }
}