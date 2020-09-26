namespace Parme.Core.Triggers
{
    public class DistanceBasedTrigger : IParticleTrigger
    {
        public string EditorShortName => "Distance Travelled";
        public string EditorShortValue => $"every {UnitsPerEmission} units";

        // default to 1 so we aren't constantly emitting
        public float UnitsPerEmission { get; set; } = 1f;
    }
}