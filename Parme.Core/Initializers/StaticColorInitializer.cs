namespace Parme.Core.Initializers
{
    public class StaticColorInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.ColorMultiplier;
        public string EditorShortName => "Static";
        public string EditorShortValue => $"{RedMultiplier}, {GreenMultiplier}, {BlueMultiplier}, {AlphaMultiplier}";

        /// <summary>
        /// The value to multiply the red color by.  Valid values are between 0.0 and 1.0.
        /// </summary>
        public float RedMultiplier { get; set; } = 1.0f;
        
        /// <summary>
        /// The value to multiply the green color by.  Valid values are between 0.0 and 1.0.
        /// </summary>
        public float GreenMultiplier { get; set; } = 1.0f;
        
        /// <summary>
        /// The value to multiply the blue color by.  Valid values are between 0.0 and 1.0.
        /// </summary>
        public float BlueMultiplier { get; set; } = 1.0f;
        
        /// <summary>
        /// The value to multiply the alpha by.  Valid values are between 0.0 and 1.0.
        /// </summary>
        public float AlphaMultiplier { get; set; } = 1.0f;
    }
}