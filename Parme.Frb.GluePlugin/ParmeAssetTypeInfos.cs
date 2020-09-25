using FlatRedBall.Glue.Elements;

namespace Parme.Frb.GluePlugin
{
    public static class ParmeAssetTypeInfos
    {
        /// <summary>
        /// ATI specifically to tell Glue our plugin handles .emitter files
        /// </summary>
        public static AssetTypeInfo EmitterFileAti => new AssetTypeInfo
        {
            Extension = "emitter",
        };
    }
}