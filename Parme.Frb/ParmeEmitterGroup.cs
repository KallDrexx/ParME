using System;
using System.Collections.Generic;
using FlatRedBall;
using FlatRedBall.Graphics;
using Parme.CSharp;
using Parme.MonoGame;

namespace Parme.Frb
{
    public class ParmeEmitterGroup : IDrawableBatch
    {
        private readonly ParticleCamera _particleCamera = new ParticleCamera{PositiveYAxisPointsUp = true};
        private readonly ParticlePool _particlePool = new ParticlePool();
        private readonly List<ParmeFrbEmitter> _frbEmitters = new List<ParmeFrbEmitter>();
        private readonly List<ParmeFrbEmitter> _frbEmittersWaitingToBeKilled = new List<ParmeFrbEmitter>();
        private readonly MonoGameEmitterRenderGroup _emitterRenderGroup;

        /// <summary>
        /// Position of the emitter group on the X axis.  This has no effect
        /// </summary>
        public float X { get; set; }
        
        /// <summary>
        /// Position of the emitter group on the Y axis.  This has no effect
        /// </summary>
        public float Y { get; set; }
        
        /// <summary>
        /// Position of the emitter group on the Z axis.  This is used for render sort order for all particles within
        /// this group in relation to other sprites, IDBs, and sets of particles.
        /// </summary>
        public float Z { get; set; }
        public bool UpdateEveryFrame => true;

        public ParmeEmitterGroup()
        {
            _emitterRenderGroup = new MonoGameEmitterRenderGroup(FlatRedBallServices.GraphicsDevice);
        }
        
        public void Draw(Camera camera)
        {
            _particleCamera.Origin = new System.Numerics.Vector2(camera.Position.X, camera.Position.Y);
            _particleCamera.PixelWidth = camera.DestinationRectangle.Width;
            _particleCamera.PixelHeight = camera.DestinationRectangle.Height;
            _particleCamera.HorizontalZoomFactor = camera.DestinationRectangle.Width / camera.OrthogonalWidth;
            _particleCamera.VerticalZoomFactor = camera.DestinationRectangle.Height / camera.OrthogonalHeight;
            
            _emitterRenderGroup.Render(_particleCamera);
        }

        public void Update()
        {
            foreach (var frbEmitter in _frbEmitters)
            {
                frbEmitter.UpdatePosition();
                frbEmitter.Emitter.Update(TimeManager.SecondDifference);
            }

            for (var x = _frbEmittersWaitingToBeKilled.Count - 1; x >= 0; x--)
            {
                var emitter = _frbEmittersWaitingToBeKilled[x];
                if (emitter.IsEmitting)
                {
                    // We want to make sure is emitting is off as of now, this guarantees one last emission round
                    // (via the update call above) before we stop emitting.  This is required for one shot emitters
                    // that will only emit when the object it's attached to is destroyed.  This also helps make sure
                    // someone doesn't accidentally turn an emitter back on while we are waiting for it to be destroyed,
                    // which will cause it to never go away.
                    emitter.IsEmitting = false;
                }
                
                if (emitter.Emitter.CalculateLiveParticleCount() == 0)
                {
                    DestroyEmitter(emitter);
                }
            } 
        }

        public void Destroy()
        {
            foreach (var frbEmitter in _frbEmitters)
            {
                _emitterRenderGroup.RemoveEmitter(frbEmitter.Emitter);
                frbEmitter.Emitter.Dispose();
            }
            
            _frbEmitters.Clear();
        }

        public ParmeFrbEmitter CreateEmitter(IEmitterLogic logic, PositionedObject parent = null)
        {
            var emitter = new ParmeFrbEmitter(_particlePool, logic)
            {
                Parent = parent
            };
            
            _emitterRenderGroup.AddEmitter(emitter.Emitter);
            _frbEmitters.Add(emitter);

            return emitter;
        }

        public void RemoveEmitter(ParmeFrbEmitter emitter, bool waitTillAllParticlesDie)
        {
            if (emitter == null) throw new ArgumentNullException(nameof(emitter));

            if (waitTillAllParticlesDie)
            {
                _frbEmittersWaitingToBeKilled.Add(emitter);
            }
            else
            {
                DestroyEmitter(emitter);
            }
        }

        private void DestroyEmitter(ParmeFrbEmitter emitter)
        {
            _frbEmitters.Remove(emitter);
            _frbEmittersWaitingToBeKilled.Remove(emitter);
            _emitterRenderGroup.RemoveEmitter(emitter.Emitter);

            emitter.Emitter.Dispose();
        }
    }
}