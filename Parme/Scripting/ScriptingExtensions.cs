using Microsoft.Xna.Framework;

namespace Parme.Scripting
{
    internal static class ScriptingExtensions
    {
        public static string ToCSharpScriptString(this Vector2 vector2)
        {
            return $"new Vector2((float) {vector2.X}, (float) {vector2.Y})";
        }
    }
}