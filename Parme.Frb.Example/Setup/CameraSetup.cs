    // This is a generated file created by Glue. To change this file, edit the camera settings in Glue.
    // To access the camera settings, push the camera icon.
    using Camera = FlatRedBall.Camera;
    namespace Parme.Frb.Example
    {
        public class CameraSetupData
        {
            public float Scale { get; set; }
            public float ScaleGum { get; set; }
            public bool Is2D { get; set; }
            public int ResolutionWidth { get; set; }
            public int ResolutionHeight { get; set; }
            public decimal? AspectRatio { get; set; }
            public bool AllowWidowResizing { get; set; }
            public bool IsFullScreen { get; set; }
            public ResizeBehavior ResizeBehavior { get; set; }
            public ResizeBehavior ResizeBehaviorGum { get; set; }
            public WidthOrHeight DominantInternalCoordinates { get; set; }
        }
        public enum ResizeBehavior
        {
            StretchVisibleArea,
            IncreaseVisibleArea
        }
        public enum WidthOrHeight
        {
            Width,
            Height
        }
        internal static class CameraSetup
        {
            static Microsoft.Xna.Framework.GraphicsDeviceManager graphicsDeviceManager;
            public static CameraSetupData Data = new CameraSetupData
            {
                Scale = 100f,
                ResolutionWidth = 800,
                ResolutionHeight = 600,
                Is2D = true,
                IsFullScreen = false,
                AllowWidowResizing = false,
                ResizeBehavior = ResizeBehavior.StretchVisibleArea,
                DominantInternalCoordinates = WidthOrHeight.Height,
            }
            ;
            internal static void ResetCamera (Camera cameraToReset = null) 
            {
                if (cameraToReset == null)
                {
                    cameraToReset = FlatRedBall.Camera.Main;
                }
                cameraToReset.Orthogonal = Data.Is2D;
                if (Data.Is2D)
                {
                    cameraToReset.OrthogonalHeight = Data.ResolutionHeight;
                    cameraToReset.OrthogonalWidth = Data.ResolutionWidth;
                    cameraToReset.FixAspectRatioYConstant();
                }
                else
                {
                    cameraToReset.UsePixelCoordinates3D(0);
                    var zoom = cameraToReset.DestinationRectangle.Height / (float)Data.ResolutionHeight;
                    cameraToReset.Z /= zoom; 
                }
                if (Data.AspectRatio != null)
                {
                    SetAspectRatioTo(Data.AspectRatio.Value, Data.DominantInternalCoordinates, Data.ResolutionWidth, Data.ResolutionHeight);
                }
            }
            internal static void SetupCamera (Camera cameraToSetUp, Microsoft.Xna.Framework.GraphicsDeviceManager graphicsDeviceManager) 
            {
                CameraSetup.graphicsDeviceManager = graphicsDeviceManager;
                ResetWindow();
                ResetCamera(cameraToSetUp);
                FlatRedBall.FlatRedBallServices.GraphicsOptions.SizeOrOrientationChanged += HandleResolutionChange;
            }
            internal static void ResetWindow () 
            {
                #if WINDOWS || DESKTOP_GL
                FlatRedBall.FlatRedBallServices.Game.Window.AllowUserResizing = Data.AllowWidowResizing;
                if (Data.IsFullScreen)
                {
                    #if DESKTOP_GL
                    graphicsDeviceManager.HardwareModeSwitch = false;
                    FlatRedBall.FlatRedBallServices.Game.Window.Position = new Microsoft.Xna.Framework.Point(0,0);
                    FlatRedBall.FlatRedBallServices.GraphicsOptions.SetResolution(Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, FlatRedBall.Graphics.WindowedFullscreenMode.FullscreenBorderless);
                    #elif WINDOWS
                    System.IntPtr hWnd = FlatRedBall.FlatRedBallServices.Game.Window.Handle;
                    var control = System.Windows.Forms.Control.FromHandle(hWnd);
                    var form = control.FindForm();
                    form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    #endif
                }
                else
                {
                    var width = (int)(Data.ResolutionWidth * Data.Scale / 100.0f);
                    var height = (int)(Data.ResolutionHeight * Data.Scale / 100.0f);
                    // subtract to leave room for windows borders
                    var maxWidth = Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 6;
                    var maxHeight = Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 28;
                    width = System.Math.Min(width, maxWidth);
                    height = System.Math.Min(height, maxHeight);
                    if (FlatRedBall.FlatRedBallServices.Game.Window.Position.Y < 25)
                    {
                        FlatRedBall.FlatRedBallServices.Game.Window.Position = new Microsoft.Xna.Framework.Point(FlatRedBall.FlatRedBallServices.Game.Window.Position.X, 25);
                    }
                    FlatRedBall.FlatRedBallServices.GraphicsOptions.SetResolution(width, height);
                }
                #elif IOS || ANDROID
                FlatRedBall.FlatRedBallServices.GraphicsOptions.SetFullScreen(FlatRedBall.FlatRedBallServices.GraphicsOptions.ResolutionWidth, FlatRedBall.FlatRedBallServices.GraphicsOptions.ResolutionHeight);
                #elif UWP
                if (Data.IsFullScreen)
                {
                    FlatRedBall.FlatRedBallServices.GraphicsOptions.SetFullScreen(Data.ResolutionWidth, Data.ResolutionHeight);
                }
                else
                {
                    FlatRedBall.FlatRedBallServices.GraphicsOptions.SetResolution(Data.ResolutionWidth, Data.ResolutionHeight);
                    var newWindowSize = new Windows.Foundation.Size(Data.ResolutionWidth, Data.ResolutionHeight);
                    Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryResizeView(newWindowSize); 
                }
                #endif
            }
            private static void HandleResolutionChange (object sender, System.EventArgs args) 
            {
                if (Data.AspectRatio != null)
                {
                    SetAspectRatioTo(Data.AspectRatio.Value, Data.DominantInternalCoordinates, Data.ResolutionWidth, Data.ResolutionHeight);
                }
                if (Data.Is2D && Data.ResizeBehavior == ResizeBehavior.IncreaseVisibleArea)
                {
                    FlatRedBall.Camera.Main.OrthogonalHeight = FlatRedBall.Camera.Main.DestinationRectangle.Height / (Data.Scale/ 100.0f);
                    FlatRedBall.Camera.Main.FixAspectRatioYConstant();
                }
            }
            private static void SetAspectRatioTo (decimal aspectRatio, WidthOrHeight dominantInternalCoordinates, int desiredWidth, int desiredHeight) 
            {
                var resolutionAspectRatio = FlatRedBall.FlatRedBallServices.GraphicsOptions.ResolutionWidth / (decimal)FlatRedBall.FlatRedBallServices.GraphicsOptions.ResolutionHeight;
                int destinationRectangleWidth;
                int destinationRectangleHeight;
                int x = 0;
                int y = 0;
                if (aspectRatio > resolutionAspectRatio)
                {
                    destinationRectangleWidth = FlatRedBall.FlatRedBallServices.GraphicsOptions.ResolutionWidth;
                    destinationRectangleHeight = FlatRedBall.Math.MathFunctions.RoundToInt(destinationRectangleWidth / (float)aspectRatio);
                    y = (FlatRedBall.FlatRedBallServices.GraphicsOptions.ResolutionHeight - destinationRectangleHeight) / 2;
                }
                else
                {
                    destinationRectangleHeight = FlatRedBall.FlatRedBallServices.GraphicsOptions.ResolutionHeight;
                    destinationRectangleWidth = FlatRedBall.Math.MathFunctions.RoundToInt(destinationRectangleHeight * (float)aspectRatio);
                    x = (FlatRedBall.FlatRedBallServices.GraphicsOptions.ResolutionWidth - destinationRectangleWidth) / 2;
                }
                foreach (var camera in FlatRedBall.SpriteManager.Cameras)
                {
                    int currentX = x;
                    int currentY = y;
                    int currentWidth = destinationRectangleWidth;
                    int currentHeight = destinationRectangleHeight;
                    switch(camera.CurrentSplitScreenViewport)
                    {
                        case  Camera.SplitScreenViewport.TopLeft:
                            currentWidth /= 2;
                            currentHeight /= 2;
                            break;
                        case  Camera.SplitScreenViewport.TopRight:
                            currentX = x + destinationRectangleWidth / 2;
                            currentWidth /= 2;
                            currentHeight /= 2;
                            break;
                        case  Camera.SplitScreenViewport.BottomLeft:
                            currentY = y + destinationRectangleHeight / 2;
                            currentWidth /= 2;
                            currentHeight /= 2;
                            break;
                        case  Camera.SplitScreenViewport.BottomRight:
                            currentX = x + destinationRectangleWidth / 2;
                            currentY = y + destinationRectangleHeight / 2;
                            currentWidth /= 2;
                            currentHeight /= 2;
                            break;
                        case  Camera.SplitScreenViewport.TopHalf:
                            currentHeight /= 2;
                            break;
                        case  Camera.SplitScreenViewport.BottomHalf:
                            currentY = y + destinationRectangleHeight / 2;
                            currentHeight /= 2;
                            break;
                        case  Camera.SplitScreenViewport.LeftHalf:
                            currentWidth /= 2;
                            break;
                        case  Camera.SplitScreenViewport.RightHalf:
                            currentX = x + destinationRectangleWidth / 2;
                            currentWidth /= 2;
                            break;
                    }
                    camera.DestinationRectangle = new Microsoft.Xna.Framework.Rectangle(currentX, currentY, currentWidth, currentHeight);
                    if (dominantInternalCoordinates == WidthOrHeight.Height)
                    {
                        camera.OrthogonalHeight = desiredHeight;
                        camera.FixAspectRatioYConstant();
                    }
                    else
                    {
                        camera.OrthogonalWidth = desiredWidth;
                        camera.FixAspectRatioXConstant();
                    }
                }
            }
            
#if WINDOWS
            internal static readonly System.IntPtr HWND_TOPMOST = new System.IntPtr(-1);
            internal static readonly System.IntPtr HWND_NOTOPMOST = new System.IntPtr(-2);
            internal static readonly System.IntPtr HWND_TOP = new System.IntPtr(0);
            internal static readonly System.IntPtr HWND_BOTTOM = new System.IntPtr(1);
    
            [System.Flags]
            internal enum SetWindowPosFlags : uint
            {
                IgnoreMove = 0x0002,
                IgnoreResize = 0x0001,
            }
    
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            internal static extern bool SetWindowPos(System.IntPtr hWnd, System.IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        public static void SetWindowAlwaysOnTop()
        {
            var hWnd = FlatRedBall.FlatRedBallServices.Game.Window.Handle;
            SetWindowPos(
                hWnd,
                HWND_TOPMOST,
                0, 0,
                0, 0, //FlatRedBallServices.GraphicsOptions.ResolutionWidth, FlatRedBallServices.GraphicsOptions.ResolutionHeight,
                SetWindowPosFlags.IgnoreMove | SetWindowPosFlags.IgnoreResize
            );
        }

        public static void UnsetWindowAlwaysOnTop()
        {
            var hWnd = FlatRedBall.FlatRedBallServices.Game.Window.Handle;

            SetWindowPos(
                hWnd,
                HWND_NOTOPMOST,
                0, 0,
                0, 0, //FlatRedBallServices.GraphicsOptions.ResolutionWidth, FlatRedBallServices.GraphicsOptions.ResolutionHeight,
                SetWindowPosFlags.IgnoreMove | SetWindowPosFlags.IgnoreResize
            );
        }
#else
        public static void SetWindowAlwaysOnTop()
            {
                // not supported on this platform, do nothing
            }

            public static void UnsetWindowAlwaysOnTop()
            {
                // not supported on this platform, do nothings
            }

#endif

        }
    }
