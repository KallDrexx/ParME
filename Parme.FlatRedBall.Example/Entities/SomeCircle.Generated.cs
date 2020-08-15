#if ANDROID || IOS || DESKTOP_GL
#define REQUIRES_PRIMARY_THREAD_LOADING
#endif
using Color = Microsoft.Xna.Framework.Color;
using System.Linq;
using FlatRedBall.Graphics;
using FlatRedBall.Math;
using FlatRedBall;
using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall.Instructions;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Screens;

namespace Parme.FlatRedBall.Example.Entities
{
    public partial class SomeCircle : PositionedObject, IDestroyable, ICollidable
    {
        // This is made static so that static lazy-loaded content can access it.
        public static string ContentManagerName { get; set; }
        #if DEBUG
        static bool HasBeenLoadedWithGlobalContentManager = false;
        #endif
        static object mLockObject = new object();
        static System.Collections.Generic.List<string> mRegisteredUnloads = new System.Collections.Generic.List<string>();
        static System.Collections.Generic.List<string> LoadedContentManagers = new System.Collections.Generic.List<string>();
        
        private Circle mCircleInstance;
        public Circle CircleInstance
        {
            get
            {
                return mCircleInstance;
            }
            private set
            {
                mCircleInstance = value;
            }
        }
        private ShapeCollection mGeneratedCollision;
        public ShapeCollection Collision
        {
            get
            {
                return mGeneratedCollision;
            }
        }
        protected Layer LayerProvidedByContainer = null;
        public SomeCircle () 
        	: this(ScreenManager.CurrentScreen.ContentManagerName, true)
        {
        }
        public SomeCircle (string contentManagerName) 
        	: this(contentManagerName, true)
        {
        }
        public SomeCircle (string contentManagerName, bool addToManagers) 
        	: base()
        {
            ContentManagerName = contentManagerName;
            InitializeEntity(addToManagers);
        }
        protected virtual void InitializeEntity (bool addToManagers) 
        {
            LoadStaticContent(ContentManagerName);
            mCircleInstance = new Circle();
            mCircleInstance.Name = "mCircleInstance";
            
            PostInitialize();
            if (addToManagers)
            {
                AddToManagers(null);
            }
        }
        public virtual void ReAddToManagers (Layer layerToAddTo) 
        {
            LayerProvidedByContainer = layerToAddTo;
            SpriteManager.AddPositionedObject(this);
            ShapeManager.AddToLayer(mCircleInstance, LayerProvidedByContainer);
        }
        public virtual void AddToManagers (Layer layerToAddTo) 
        {
            LayerProvidedByContainer = layerToAddTo;
            SpriteManager.AddPositionedObject(this);
            ShapeManager.AddToLayer(mCircleInstance, LayerProvidedByContainer);
            AddToManagersBottomUp(layerToAddTo);
            CustomInitialize();
        }
        public virtual void Activity () 
        {
            
            CustomActivity();
        }
        public virtual void Destroy () 
        {
            SpriteManager.RemovePositionedObject(this);
            
            if (CircleInstance != null)
            {
                ShapeManager.Remove(CircleInstance);
            }
            mGeneratedCollision.RemoveFromManagers(clearThis: false);
            CustomDestroy();
        }
        public virtual void PostInitialize () 
        {
            bool oldShapeManagerSuppressAdd = ShapeManager.SuppressAddingOnVisibilityTrue;
            ShapeManager.SuppressAddingOnVisibilityTrue = true;
            if (mCircleInstance.Parent == null)
            {
                mCircleInstance.CopyAbsoluteToRelative();
                mCircleInstance.AttachTo(this, false);
            }
            CircleInstance.Radius = 16f;
            mGeneratedCollision = new ShapeCollection();
            Collision.Circles.AddOneWay(mCircleInstance);
            ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
        }
        public virtual void AddToManagersBottomUp (Layer layerToAddTo) 
        {
            AssignCustomVariables(false);
        }
        public virtual void RemoveFromManagers () 
        {
            SpriteManager.ConvertToManuallyUpdated(this);
            if (CircleInstance != null)
            {
                ShapeManager.RemoveOneWay(CircleInstance);
            }
            mGeneratedCollision.RemoveFromManagers(clearThis: false);
        }
        public virtual void AssignCustomVariables (bool callOnContainedElements) 
        {
            if (callOnContainedElements)
            {
            }
            CircleInstance.Radius = 16f;
        }
        public virtual void ConvertToManuallyUpdated () 
        {
            this.ForceUpdateDependenciesDeep();
            SpriteManager.ConvertToManuallyUpdated(this);
        }
        public static void LoadStaticContent (string contentManagerName) 
        {
            if (string.IsNullOrEmpty(contentManagerName))
            {
                throw new System.ArgumentException("contentManagerName cannot be empty or null");
            }
            ContentManagerName = contentManagerName;
            #if DEBUG
            if (contentManagerName == FlatRedBallServices.GlobalContentManager)
            {
                HasBeenLoadedWithGlobalContentManager = true;
            }
            else if (HasBeenLoadedWithGlobalContentManager)
            {
                throw new System.Exception("This type has been loaded with a Global content manager, then loaded with a non-global.  This can lead to a lot of bugs");
            }
            #endif
            bool registerUnload = false;
            if (LoadedContentManagers.Contains(contentManagerName) == false)
            {
                LoadedContentManagers.Add(contentManagerName);
                lock (mLockObject)
                {
                    if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBallServices.GlobalContentManager)
                    {
                        FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("SomeCircleStaticUnload", UnloadStaticContent);
                        mRegisteredUnloads.Add(ContentManagerName);
                    }
                }
            }
            if (registerUnload && ContentManagerName != FlatRedBallServices.GlobalContentManager)
            {
                lock (mLockObject)
                {
                    if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBallServices.GlobalContentManager)
                    {
                        FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("SomeCircleStaticUnload", UnloadStaticContent);
                        mRegisteredUnloads.Add(ContentManagerName);
                    }
                }
            }
            CustomLoadStaticContent(contentManagerName);
        }
        public static void UnloadStaticContent () 
        {
            if (LoadedContentManagers.Count != 0)
            {
                LoadedContentManagers.RemoveAt(0);
                mRegisteredUnloads.RemoveAt(0);
            }
            if (LoadedContentManagers.Count == 0)
            {
            }
        }
        [System.Obsolete("Use GetFile instead")]
        public static object GetStaticMember (string memberName) 
        {
            return null;
        }
        public static object GetFile (string memberName) 
        {
            return null;
        }
        object GetMember (string memberName) 
        {
            return null;
        }
        protected bool mIsPaused;
        public override void Pause (InstructionList instructions) 
        {
            base.Pause(instructions);
            mIsPaused = true;
        }
        public virtual void SetToIgnorePausing () 
        {
            InstructionManager.IgnorePausingFor(this);
            InstructionManager.IgnorePausingFor(CircleInstance);
        }
        public virtual void MoveToLayer (Layer layerToMoveTo) 
        {
            var layerToRemoveFrom = LayerProvidedByContainer;
            if (layerToRemoveFrom != null)
            {
                layerToRemoveFrom.Remove(CircleInstance);
            }
            ShapeManager.AddToLayer(CircleInstance, layerToMoveTo);
            LayerProvidedByContainer = layerToMoveTo;
        }
    }
}
