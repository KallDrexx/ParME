using FlatRedBall;
using FlatRedBall.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Parme.CSharp;
using Parme.MonoGame;

namespace Parme.FlatRedBall
{
    public class EmitterDrawableBatch : IDrawableBatch
    {
        private readonly Emitter _emitter;
        private readonly ParticleCamera _particleCamera = new ParticleCamera{PositiveYAxisPointsUp = true};

        public bool IsEmittingParticles => _emitter.IsEmittingNewParticles;
        
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public bool UpdateEveryFrame => true;

        public EmitterDrawableBatch(IEmitterLogic emitterLogic)
        {
            var texture = GetWhiteTexture();
            _emitter = new MonoGameEmitter(emitterLogic, FlatRedBallServices.GraphicsDevice, texture);
        }
        
        public void Draw(Camera camera)
        {
            _particleCamera.Origin = new System.Numerics.Vector2(camera.Position.X, camera.Position.Y);
            _particleCamera.PixelWidth = camera.DestinationRectangle.Width;
            _particleCamera.PixelHeight = camera.DestinationRectangle.Height;
            _particleCamera.HorizontalZoomFactor = camera.DestinationRectangle.Width / camera.OrthogonalWidth;
            _particleCamera.VerticalZoomFactor = camera.DestinationRectangle.Height / camera.OrthogonalHeight;
            
            _emitter.Render(_particleCamera);
        }

        public void Update()
        {
            _emitter.WorldCoordinates = new System.Numerics.Vector2(X, Y);
            _emitter.Update(TimeManager.SecondDifference);
        }

        public void Destroy()
        {
            _emitter.Stop();
            _emitter.KillAllParticles();
        }

        public void StartEmitting() => _emitter.Start();
        public void StopEmitting() => _emitter.Stop();
        public void KillAllParticles() => _emitter.KillAllParticles();

        private static Texture2D GetWhiteTexture()
        {
            var pixels = new Color[10*10];
            for (var x = 0; x < pixels.Length; x++)
            {
                pixels[x] = Color.White;
            }
            
            var texture = new Texture2D(FlatRedBallServices.GraphicsDevice, 10, 10);
            texture.SetData(pixels);

            return texture;
        }
    }
}