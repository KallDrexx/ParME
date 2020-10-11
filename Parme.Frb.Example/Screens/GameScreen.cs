using System;
using FlatRedBall;
using FlatRedBall.Input;
using Microsoft.Xna.Framework.Input;
using Parme.Frb.Example.Entities;

namespace Parme.Frb.Example.Screens
{
    public partial class GameScreen
    {
        void CustomInitialize()
        {
        }

        void CustomActivity(bool firstTimeCalled)
        {
            if (InputManager.Keyboard.KeyDown(Keys.Left))
            {
                var radians = PlayerInstance.RotationDegreesPerSecond * (Math.PI / 180) * TimeManager.SecondDifference;
                PlayerInstance.RotationZ += (float) radians;
            }

            if (InputManager.Keyboard.KeyDown(Keys.Right))
            {
                var radians = PlayerInstance.RotationDegreesPerSecond * (Math.PI / 180) * TimeManager.SecondDifference;
                PlayerInstance.RotationZ -= (float) radians;
            }

            if (InputManager.Keyboard.KeyReleased(Keys.Space))
            {
                var bullet = new Bullet();
                bullet.Velocity.X = (float) (bullet.Speed * Math.Cos(PlayerInstance.RotationZ));
                bullet.Velocity.Y = (float) (bullet.Speed * Math.Sin(PlayerInstance.RotationZ));
                bullet.RotationZ = PlayerInstance.RotationZ;
                BulletList.Add(bullet);
            }
        }

        void CustomDestroy()
        {
        }

        static void CustomLoadStaticContent(string contentManagerName)
        {
        }
    }
}
