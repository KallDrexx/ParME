using System.Diagnostics;
using System.Reflection;

namespace Parme.Frb.GluePlugin
{
    public static class PluginVersion
    {
        public static string GetRawVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fileVersionInfo.ProductVersion;
        }
    }
}