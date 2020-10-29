using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;

namespace Parme.Frb.Example.Entities
{
    public partial class Player
    {
        
        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            LineInstance.RelativePoint1 = new Point3D(0, 0);
            LineInstance.RelativePoint2 = new Point3D(20, 0);
            TestEmitter.RotationOffsetInRadians = (float) (90 * (Math.PI / 180f));
        }

        private void CustomActivity()
        {
        }

        private void CustomDestroy()
        {
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
        }
    }
}
