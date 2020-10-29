#if ANDROID || IOS || DESKTOP_GL
// Android doesn't allow background loading. iOS doesn't allow background rendering (which is used by converting textures to use premult alpha)
#define REQUIRES_PRIMARY_THREAD_LOADING
#endif
using System.Collections.Generic;
using System.Threading;
using FlatRedBall;
using FlatRedBall.Math.Geometry;
using FlatRedBall.ManagedSpriteGroups;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Utilities;
using BitmapFont = FlatRedBall.Graphics.BitmapFont;
using FlatRedBall.Localization;

namespace Parme.Frb.Example
{
    public static partial class GlobalContent
    {
        
        public static Microsoft.Xna.Framework.Graphics.Texture2D direction { get; set; }
        [System.Obsolete("Use GetFile instead")]
        public static object GetStaticMember (string memberName) 
        {
            switch(memberName)
            {
                case  "direction":
                    return direction;
            }
            return null;
        }
        public static object GetFile (string memberName) 
        {
            switch(memberName)
            {
                case  "direction":
                    return direction;
            }
            return null;
        }
        public static bool IsInitialized { get; private set; }
        public static bool ShouldStopLoading { get; set; }
        const string ContentManagerName = "Global";
        public static void Initialize () 
        {
            
            direction = FlatRedBall.FlatRedBallServices.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/globalcontent/direction.png", ContentManagerName);
            			IsInitialized = true;
            #if DEBUG && WINDOWS
            InitializeFileWatch();
            #endif
        }
        public static void Reload (object whatToReload) 
        {
            if (whatToReload == direction)
            {
                var oldTexture = direction;
                {
                    var cm = FlatRedBall.FlatRedBallServices.GetContentManagerByName("Global");
                    cm.UnloadAsset(direction);
                    direction = FlatRedBall.FlatRedBallServices.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("content/globalcontent/direction.png");
                }
                FlatRedBall.SpriteManager.ReplaceTexture(oldTexture, direction);
            }
        }
        #if DEBUG && WINDOWS
        static System.IO.FileSystemWatcher watcher;
        private static void InitializeFileWatch () 
        {
            string globalContent = FlatRedBall.IO.FileManager.RelativeDirectory + "content/globalcontent/";
            if (System.IO.Directory.Exists(globalContent))
            {
                watcher = new System.IO.FileSystemWatcher();
                watcher.Path = globalContent;
                watcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
                watcher.Filter = "*.*";
                watcher.Changed += HandleFileChanged;
                watcher.EnableRaisingEvents = true;
            }
        }
        private static void HandleFileChanged (object sender, System.IO.FileSystemEventArgs e) 
        {
            try
            {
                System.Threading.Thread.Sleep(500);
                var fullFileName = e.FullPath;
                var relativeFileName = FlatRedBall.IO.FileManager.MakeRelative(FlatRedBall.IO.FileManager.Standardize(fullFileName));
                if (relativeFileName == "content/globalcontent/direction.png")
                {
                    Reload(direction);
                }
            }
            catch{}
        }
        #endif
        
        
    }
}
