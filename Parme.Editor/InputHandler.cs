using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Parme.CSharp;
using Parme.Editor.Ui;

namespace Parme.Editor
{
    public class InputHandler
    {
        private readonly EditorUiController _uiController;
        private readonly ParticleCamera _camera;
        private KeyboardState _previousKeyState, _currentKeyState;
        private MouseState _previousMouseState, _currentMouseState;

        public InputHandler(EditorUiController uiController, ParticleCamera camera)
        {
            _uiController = uiController;
            _camera = camera;
        }
        
        /// <summary>
        /// Checks for any input that happened since the last frame, and adjust the graph accordingly
        /// </summary>
        public void Update()
        {
            _previousKeyState = _currentKeyState;
            _previousMouseState = _currentMouseState;
            _currentKeyState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();

            HandleKeyboardInput();
            HandleMouseInput();
        }

        private void HandleKeyboardInput()
        {
            if (HasBeenPressed(Keys.F12))
            {
                _uiController.ToggleImGuiDemoWindow();
            }
        }

        private void HandleMouseInput()
        {
            if (!_uiController.AcceptingMouseInput)
            {
                // Only react to mouse actions if the UI doesn't have focus
                var positionChange = _currentMouseState.Position - _previousMouseState.Position;
                if (_previousMouseState.LeftButton == ButtonState.Pressed && 
                    _currentMouseState.LeftButton == ButtonState.Pressed && 
                    positionChange != Point.Zero)
                {
                    _camera.Origin = _camera.PositiveYAxisPointsUp
                        ? _camera.Origin - new System.Numerics.Vector2(positionChange.X, -positionChange.Y)
                        : _camera.Origin - new System.Numerics.Vector2(positionChange.X, positionChange.Y);
                }
            }
        }
        
        private bool HasBeenPressed(Keys key)
        {
            return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
        }
    }
}