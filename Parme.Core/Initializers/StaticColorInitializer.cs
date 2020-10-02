namespace Parme.Core.Initializers
{
    public class StaticColorInitializer : IParticleInitializer
    {
        public InitializerType InitializerType => InitializerType.ColorMultiplier;
        public string EditorShortName => "Static";
        public string EditorShortValue => $"{Red}, {Green}, {Blue}, {Alpha:0.00}";

        /// <summary>
        /// Amount of red to have in the particle.  Valid values are between 0 and 255
        /// </summary>
        public byte Red { get; set; } = 255;

        /// <summary>
        /// Amount of green to have in the particle.  Valid values are between 0 and 255
        /// </summary>
        public byte Green { get; set; } = 255;
        
        /// <summary>
        /// Amount of blue to have in the particle.  Valid values are between 0 and 255.
        /// </summary>
        public byte Blue { get; set; } = 255;
        
        /// <summary>
        /// Opaqueness of the particle.  Valid values are between 0.0 and 1.0.
        /// </summary>
        public float Alpha { get; set; } = 1f;
    }
}