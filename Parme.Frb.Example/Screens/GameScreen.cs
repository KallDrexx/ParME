using FlatRedBall;
using FlatRedBall.Input;
using Microsoft.Xna.Framework.Input;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;
using Parme.CSharp;

namespace Parme.Frb.Example.Screens
{
    public partial class GameScreen
    {
        void CustomInitialize()
        {
            
        }

        void CustomActivity(bool firstTimeCalled)
        {
            const float movementSpeed = 200;
            if (InputManager.Keyboard.KeyDown(Keys.Up))
            {
                SomeCircleInstance.Y += TimeManager.SecondDifference * movementSpeed;
            }

            if (InputManager.Keyboard.KeyDown(Keys.Down))
            {
                SomeCircleInstance.Y -= TimeManager.SecondDifference * movementSpeed;
            }

            if (InputManager.Keyboard.KeyDown(Keys.Right))
            {
                SomeCircleInstance.X += TimeManager.SecondDifference * movementSpeed;
            }

            if (InputManager.Keyboard.KeyDown(Keys.Left))
            {
                SomeCircleInstance.X -= TimeManager.SecondDifference * movementSpeed;
            }

            

            if (InputManager.Keyboard.KeyReleased(Keys.Enter))
            {
                // EmitterDrawableBatchInstance.IsEmitting = !EmitterDrawableBatchInstance.IsEmitting;
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
