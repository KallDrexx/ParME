using System;
using ImGuiHandler;
using ImGuiHandler.MonoGame;
using ImGuiNET;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;
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
        public const string DefaultExtension = ".emitter";
        
        private readonly ParticleCamera _camera = new ParticleCamera();
        private readonly SettingsCommandHandler _commandHandler = new SettingsCommandHandler();
        private readonly AppOperationQueue _appOperationQueue = new AppOperationQueue();
        private ITextureFileLoader _textureFileLoader;
        private MonoGameEmitter _emitter;
        private ImGuiManager _imGuiManager;
        private EditorUiController _uiController;
        private InputHandler _inputHandler;
        private float _secondsSinceLastSettingsChange;
        private bool _emitterSettingsUpdated;
        private bool _hasUnsavedChanges;

        private Texture2D _testTexture;
        
        public App()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1024,
                PreferredBackBufferHeight = 768,
                PreferMultiSampling = true
            };

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += WindowOnClientSizeChanged;
        }

        protected override void Initialize()
        {
            _textureFileLoader = new TextureFileLoader(GraphicsDevice);
            
            _camera.Origin = new Vector2(-GraphicsDevice.Viewport.Width / 6f, GraphicsDevice.Viewport.Height / 4f);
            _camera.PositiveYAxisPointsUp = true;
            _camera.PixelWidth = GraphicsDevice.Viewport.Width;
            _camera.PixelHeight = GraphicsDevice.Viewport.Height;
            
            _imGuiManager = new ImGuiManager(new MonoGameImGuiRenderer(this));
            _uiController = new EditorUiController(_imGuiManager, _commandHandler, _appOperationQueue);
            _inputHandler = new InputHandler(_uiController, _camera, _commandHandler);

            ImGui.GetIO().FontGlobalScale = 1.2f;
            _uiController.WindowResized(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            var pixels = new Color[10*10];
            for (var x = 0; x < pixels.Length; x++)
            {
                pixels[x] = Color.White;
            }
            
            _testTexture = new Texture2D(GraphicsDevice, 10, 10);
            _testTexture.SetData(pixels);

            _commandHandler.EmitterUpdated += (sender, emitterSettings) =>
            {
                _emitterSettingsUpdated = true;
                _secondsSinceLastSettingsChange = 0;
            };

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _secondsSinceLastSettingsChange += (float) gameTime.ElapsedGameTime.TotalSeconds;

            var unsavedChangesResetThisFrame = false;
            while (_appOperationQueue.TryDequeue(out var appOperation))
            {
                var appState = appOperation.Run();
                if (appState != null)
                {
                    if (appState.ResetUnsavedChangesMarker)
                    {
                        unsavedChangesResetThisFrame = true;
                    }
                    
                    UpdatedAppState(appState);
                }
            }
            
            if (_secondsSinceLastSettingsChange > MinSecondsForRecompilingEmitter && _emitterSettingsUpdated)
            {
                var settings = _commandHandler.GetCurrentSettings();
                UpdateEmitter(settings);
                _uiController.EmitterSettingsChanged(settings);

                _emitterSettingsUpdated = false;
                _secondsSinceLastSettingsChange = 0;

                if (!unsavedChangesResetThisFrame)
                {
                    _hasUnsavedChanges = true;
                }
            }

            _uiController.UnsavedChangesPresent = _hasUnsavedChanges;
            _commandHandler.UpdateTime((float) gameTime.ElapsedGameTime.TotalSeconds);
            _uiController.Update();
            _inputHandler.Update();
            _emitter?.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var backgroundColorVector = _uiController.BackgroundColor;
            var backgroundColor = new Color(backgroundColorVector.X, 
                backgroundColorVector.Y, 
                backgroundColorVector.Z);
            
            GraphicsDevice.Clear(backgroundColor);

            _emitter?.Render(_camera);
            _imGuiManager.RenderElements(gameTime.ElapsedGameTime);
            
            base.Draw(gameTime);
        }

        private void UpdatedAppState(AppState appState)
        {
            if (appState == null)
            {
                throw new ArgumentNullException(nameof(appState));
            }

            if (!string.IsNullOrWhiteSpace(appState.NewErrorMessage))
            {
                _uiController.DisplayErrorMessage(appState.NewErrorMessage);
            }

            if (appState.UpdatedSettings != null)
            {
                UpdateEmitter(appState.UpdatedSettings);
                _uiController.NewEmitterSettingsLoaded(appState.UpdatedSettings, appState.UpdatedFileName);
                _uiController.UnsavedChangesPresent = false;
            } 
            else if (!string.IsNullOrWhiteSpace(appState.UpdatedFileName))
            {
                _uiController.NewEmitterSettingsLoaded(null, appState.UpdatedFileName);
            }

            if (appState.ResetUnsavedChangesMarker)
            {
                _hasUnsavedChanges = false;
            }
        }

        private void UpdateEmitter(EmitterSettings settings)
        {
            if (_emitter != null)
            {
                _emitter.Stop();
                _emitter.KillAllParticles();
                _emitter = null;
            }

            if (settings != null)
            {
                var code = EmitterLogicClassGenerator.Generate(settings, "Parme.Editor", "Test", true);
            
                var scriptOptions = ScriptOptions.Default
                    .WithReferences(typeof(IEmitterLogic).Assembly);
                
                var logicClass = CSharpScript.EvaluateAsync<IEmitterLogic>(code, scriptOptions).GetAwaiter().GetResult();

                _emitter = new MonoGameEmitter(logicClass, GraphicsDevice, _textureFileLoader)
                {
                    //WorldCoordinates = new System.Numerics.Vector2(400, -200)
                };
                _emitter.Start();
            }
        }

        private void WindowOnClientSizeChanged(object sender, EventArgs e)
        {
            _uiController.WindowResized(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _camera.PixelWidth = GraphicsDevice.Viewport.Width;
            _camera.PixelHeight = GraphicsDevice.Viewport.Height;
        }
    }
}