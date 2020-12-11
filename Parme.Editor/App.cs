﻿using System;
using ImGuiHandler;
using ImGuiHandler.MonoGame;
using ImGuiNET;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.Core;
using Parme.CSharp;
using Parme.CSharp.CodeGen;
using Parme.Editor.AppOperations;
using Parme.Editor.Ui;
using Parme.MonoGame;
using Vector2 = System.Numerics.Vector2;

namespace Parme.Editor
{
    public class App : Game
    {
        private const float MinSecondsForRecompilingEmitter = 0.25f;
        public const string DefaultExtension = ".emlogic";
        
        private readonly ParticleCamera _camera = new ParticleCamera();
        private readonly ApplicationState _applicationState = new ApplicationState();
        private readonly ParticlePool _particlePool = new ParticlePool();
        private readonly AppOperationQueue _appOperationQueue;
        private readonly SettingsCommandHandler _commandHandler;
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private TextureFileLoader _textureFileLoader;
        private MonoGameEmitter _emitter;
        private MonoGameEmitterRenderGroup _emitterRenderGroup;
        private ImGuiManager _imGuiManager;
        private EditorUiController _uiController;
        private InputHandler _inputHandler;
        private float _lastProcessedEmitterChangeTime;
        private Texture2D _gridTexture;
        private SpriteBatch _spriteBatch;

        public App()
        {
            _appOperationQueue = new AppOperationQueue();
            _commandHandler = new SettingsCommandHandler(_appOperationQueue);
            
            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += WindowOnClientSizeChanged;
        }

        protected override void Initialize()
        {
            _graphicsDeviceManager.PreferredBackBufferWidth = 1024;
            _graphicsDeviceManager.PreferredBackBufferHeight = 786;
            _graphicsDeviceManager.ApplyChanges();
            
            _gridTexture = SetupGridTexture(GraphicsDevice,32);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _textureFileLoader = new TextureFileLoader(GraphicsDevice, _applicationState);
            _emitterRenderGroup = new MonoGameEmitterRenderGroup(GraphicsDevice);
            
            ResetCamera();
            _applicationState.Zoom = 1;

            var monoGameImGuiRenderer = new MonoGameImGuiRenderer(this);
            _imGuiManager = new ImGuiManager(monoGameImGuiRenderer);
            _uiController = new EditorUiController(_imGuiManager, 
                _commandHandler, 
                _appOperationQueue, 
                _applicationState, 
                _textureFileLoader, 
                monoGameImGuiRenderer);
            
            _inputHandler = new InputHandler(_uiController, _camera, _commandHandler, _appOperationQueue, _applicationState);
            _inputHandler.ResetCameraAndEmitterRequested += (sender, args) => ResetCamera(true);
            
            _uiController.WindowResized(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            
            _appOperationQueue.Enqueue(new UpdateMiscOptionsRequested
            {
                UpdatedSamplerState = SamplerState.PointClamp,
            });

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _applicationState.UpdateTotalTime((float) gameTime.TotalGameTime.TotalSeconds);
            
            if (_textureFileLoader.CachedTextureHasPendingUpdate())
            {
                var operationResult = new EmitterUpdatedNotification(_commandHandler.GetCurrentSettings()).Run();
                _applicationState.Apply(operationResult);
            }

            while (_appOperationQueue.TryDequeue(out var appOperation))
            {
                var operationResult = appOperation.Run();
                _applicationState.Apply(operationResult);
            }

            if (_gridTexture.Width != _applicationState.GridSize)
            {
                _gridTexture = SetupGridTexture(GraphicsDevice, _applicationState.GridSize);
            }

            // Only update the emitter if it's been updated since the last time we have processed it
            // and we have waited the debounce period
            if (Math.Abs(_lastProcessedEmitterChangeTime - _applicationState.TimeLastEmitterUpdated) > 0.0001f &&
                gameTime.TotalGameTime.TotalSeconds - _applicationState.TimeLastEmitterUpdated > MinSecondsForRecompilingEmitter)
            {
                var settings = _applicationState.ActiveEmitter;
                if (_applicationState.EmitterUpdatedFromFileLoad)
                {
                    _commandHandler.NewStartingEmitter(settings);
                }
                
                UpdateEmitter(settings);
                
                _lastProcessedEmitterChangeTime = _applicationState.TimeLastEmitterUpdated;
            }
            
            _commandHandler.UpdateTime((float) gameTime.ElapsedGameTime.TotalSeconds);
            _inputHandler.Update();
            
            _camera.HorizontalZoomFactor = (float) _applicationState.Zoom;
            _camera.VerticalZoomFactor = (float) _applicationState.Zoom;

            if (_emitter != null)
            {
                if (!_emitter.IsEmittingNewParticles && _emitter.CalculateLiveParticleCount() == 0)
                {
                    // This most likely means the emitter is a one shot that has already fired.  Now that its particles
                    // are dead we want to re-emit.  Otherwise it makes it difficult to validate a one shot emitter
                    _emitter.IsEmittingNewParticles = true;
                }
                
                _emitter.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
                
                if (_uiController.EmitterVelocity != Vector2.Zero)
                {
                    // Apply velocity to the emitter, but make sure the camera retains it's distance from the emitter
                    // to keep it in the same spot of the viewport.
                    var distance = _emitter.WorldCoordinates - _camera.Origin;
                    _emitter.WorldCoordinates += _uiController.EmitterVelocity * (float) gameTime.ElapsedGameTime.TotalSeconds;
                    _camera.Origin = _emitter.WorldCoordinates - distance;
                }
            }
            
            _applicationState.ParticleCount = _emitter?.CalculateLiveParticleCount() ?? 0;
            _uiController.Update();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var backgroundColorVector = _applicationState.BackgroundColor;
            var backgroundColor = new Color(backgroundColorVector.X, 
                backgroundColorVector.Y, 
                backgroundColorVector.Z);
            
            GraphicsDevice.Clear(backgroundColor);
            RenderGrid(backgroundColor);

            _emitterRenderGroup.Render(_camera, _applicationState.RenderSamplerState ?? SamplerState.PointClamp);
            _imGuiManager.RenderElements(gameTime.ElapsedGameTime);
            
            base.Draw(gameTime);
        }

        private void RenderGrid(Color backgroundColor)
        {
            _spriteBatch.Begin(samplerState: SamplerState.LinearWrap);
            _spriteBatch.Draw(_gridTexture,
                new Rectangle(0, 
                    0, 
                    GraphicsDevice.Viewport.Width, 
                    GraphicsDevice.Viewport.Height), 
                new Rectangle((int)_camera.Origin.X, 
                    (int) -_camera.Origin.Y, 
                    (int) (GraphicsDevice.Viewport.Width / _camera.HorizontalZoomFactor), 
                    (int) (GraphicsDevice.Viewport.Height / _camera.VerticalZoomFactor)),
                backgroundColor);
            _spriteBatch.End();
        }

        private void UpdateEmitter(EmitterSettings settings)
        {
            if (_emitter != null)
            {
                _emitterRenderGroup.RemoveEmitter(_emitter);
                _emitter.Dispose();
                _emitter = null;
            }

            if (settings != null)
            {
                var code = EmitterLogicClassGenerator.Generate(settings, "Parme.Editor", "Test", true);
            
                var scriptOptions = ScriptOptions.Default
                    .WithReferences(typeof(IEmitterLogic).Assembly);
                
                var logicClass = CSharpScript.EvaluateAsync<IEmitterLogic>(code, scriptOptions).GetAwaiter().GetResult();

                _emitter = new MonoGameEmitter(logicClass, _particlePool, GraphicsDevice, _textureFileLoader);
                _emitterRenderGroup.AddEmitter(_emitter);
                _emitter.IsEmittingNewParticles = true;
                
                // Don't reset the camera if we don't have a moving emitter.  Without a moving emitter then this will
                // just frustrate the user, as they've positioned the camera in that spot for a reason
                if (_uiController.EmitterVelocity != Vector2.Zero)
                {
                    ResetCamera();
                }
            }

            if (_applicationState.AutoSaveOnChange)
            {
                _appOperationQueue.Enqueue(new SaveEmitterRequested(_applicationState.ActiveFileName, settings));
            }
        }

        private void WindowOnClientSizeChanged(object sender, EventArgs e)
        {
            _uiController.WindowResized(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _camera.PixelWidth = GraphicsDevice.Viewport.Width;
            _camera.PixelHeight = GraphicsDevice.Viewport.Height;
        }

        private void ResetCamera(bool resetEmitterPosition = false)
        {
            _camera.Origin = new Vector2(-GraphicsDevice.Viewport.Width / 6f, GraphicsDevice.Viewport.Height / 4f);
            _camera.PositiveYAxisPointsUp = true;
            _camera.PixelWidth = GraphicsDevice.Viewport.Width;
            _camera.PixelHeight = GraphicsDevice.Viewport.Height;

            if (resetEmitterPosition && _emitter != null)
            {
                _emitter.WorldCoordinates = Vector2.Zero;
            }
        }

        private static Texture2D SetupGridTexture(GraphicsDevice graphicsDevice, int squareSizeInPixels)
        {
            var pixels = new Color[squareSizeInPixels * squareSizeInPixels];

            for (var row = 0; row < squareSizeInPixels; row++)
            for (var col = 0; col < squareSizeInPixels; col++)
            {
                Color color;
                if (col == squareSizeInPixels - 1 || row == squareSizeInPixels - 1)
                {
                    color = Color.Black;
                }
                else
                {
                    color = Color.White;
                }

                pixels[row * squareSizeInPixels + col] = color;
            }
            
            var texture = new Texture2D(graphicsDevice, squareSizeInPixels, squareSizeInPixels);
            texture.SetData(pixels);

            return texture;
        }
    }
}