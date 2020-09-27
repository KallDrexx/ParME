using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Parme.CSharp;
using Parme.Editor.AppOperations;
using Parme.Editor.Ui;

namespace Parme.Editor
{
    public class InputHandler
    {
        private readonly EditorUiController _uiController;
        private readonly ParticleCamera _camera;
        private readonly SettingsCommandHandler _commandHandler;
        private readonly AppOperationQueue _appOperationQueue;
        private readonly ApplicationState _applicationState;
        
        private KeyboardState _previousKeyState, _currentKeyState;
        private MouseState _previousMouseState, _currentMouseState;

        public event EventHandler ResetCameraAndEmitterRequested;

        public InputHandler(EditorUiController uiController, 
            ParticleCamera camera, 
            SettingsCommandHandler commandHandler, 
            AppOperationQueue appOperationQueue, 
            ApplicationState applicationState)
        {
            _uiController = uiController;
            _camera = camera;
            _commandHandler = commandHandler;
            _appOperationQueue = appOperationQueue;
            _applicationState = applicationState;
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
            if ((_currentKeyState.IsKeyDown(Keys.LeftControl) || _currentKeyState.IsKeyDown(Keys.RightControl)) &&
                HasBeenPressed(Keys.S) &&
                !string.IsNullOrWhiteSpace(_applicationState.ActiveFileName))
            {
                var operation = new SaveEmitterRequested(_applicationState.ActiveFileName, _applicationState.ActiveEmitter);
                _appOperationQueue.Enqueue(operation);
            }
            
            if (!_uiController.AcceptingKeyboardInput)
            {
                if (HasBeenPressed(Keys.F12))
                {
                    _uiController.ToggleImGuiDemoWindow();
                }

                if (_currentKeyState.IsKeyDown(Keys.LeftControl) && HasBeenPressed(Keys.Z))
                {
                    _commandHandler.Undo();
                }

                if (_currentKeyState.IsKeyDown(Keys.LeftControl) && HasBeenPressed(Keys.Y))
                {
                    _commandHandler.Redo();
                }

                if (HasBeenPressed(Keys.Back))
                {
                    ResetCameraAndEmitterRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void HandleMouseInput()
        {
            if (!_uiController.AcceptingMouseInput)
            {
                // Only react to mouse actions if the UI doesn't have focus
                var pixelChange = _currentMouseState.Position - _previousMouseState.Position;
                var positionChange = new Vector2(
                    pixelChange.X / _camera.HorizontalZoomFactor,
                    pixelChange.Y / _camera.VerticalZoomFactor);
                
                if (_previousMouseState.LeftButton == ButtonState.Pressed && 
                    _currentMouseState.LeftButton == ButtonState.Pressed && 
                    positionChange != Vector2.Zero)
                {
                    _camera.Origin = _camera.PositiveYAxisPointsUp
                        ? _camera.Origin - new System.Numerics.Vector2(positionChange.X, -positionChange.Y)
                        : _camera.Origin - new System.Numerics.Vector2(positionChange.X, positionChange.Y);
                }
                
                var scrollChange = _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
                if (scrollChange != 0)
                {
                    const float scrollZoomModifier = 1.2f;
                    _camera.HorizontalZoomFactor = scrollChange > 0
                        ? _camera.HorizontalZoomFactor * scrollZoomModifier
                        : _camera.HorizontalZoomFactor / scrollZoomModifier;
                    
                    _camera.VerticalZoomFactor = scrollChange > 0
                        ? _camera.VerticalZoomFactor * scrollZoomModifier
                        : _camera.VerticalZoomFactor / scrollZoomModifier;
                }
            }
        }
        
        private bool HasBeenPressed(Keys key)
        {
            return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
        }
    }
}