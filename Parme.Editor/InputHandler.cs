using Microsoft.Xna.Framework.Input;
using Parme.Editor.Ui;

namespace Parme.Editor
{
    public class InputHandler
    {
        private readonly EditorUiController _uiController;
        private KeyboardState _previousKeyState, _currentKeyState;
        private MouseState _previousMouseState, _currentMouseState;

        public InputHandler(EditorUiController uiController)
        {
            _uiController = uiController;
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
        }

        private void HandleKeyboardInput()
        {
            if (HasBeenPressed(Keys.F12))
            {
                _uiController.ToggleImGuiDemoWindow();
            }
        }
        
        private bool HasBeenPressed(Keys key)
        {
            return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
        }
    }
}