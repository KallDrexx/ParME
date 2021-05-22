namespace Parme.Editor.Settings
{
    /// <summary>
    /// Option for if a texture should be automatically moved to the same location as the emitter
    /// </summary>
    public enum AutoMoveTextureOption
    {
        /// <summary>
        /// Ask if the user wants the texture moved to the same location as the emitter
        /// </summary>
        
        Ask = 0,
        
        /// <summary>
        /// When a texture is selected, the texture file will be copied to the same directory as the emitter definition
        /// </summary>
        AlwaysCopy = 1,
        
        /// <summary>
        /// When a texture is selected, the texture file will *not* be copied and the emitter will point to the texture
        /// where it currently exists in relation to the emitter definition file.
        /// </summary>
        AlwaysLeave = 2,
    }
}