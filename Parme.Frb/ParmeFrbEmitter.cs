﻿using System.Numerics;
using FlatRedBall;
using Parme.CSharp;
using Parme.MonoGame;

namespace Parme.Frb
{
    public class ParmeFrbEmitter
    {
        public MonoGameEmitter Emitter { get; }
        public PositionedObject Parent { get; set; }
        
        /// <summary>
        /// The position of the emitter on the X axis relative to its parent, or the origin if it has no parent
        /// </summary>
        public float XOffset { get; set; }
        
        /// <summary>
        /// The position of the emitter on the Y axis relative to its parent, or the origin if it has no parent
        /// </summary>
        public float YOffset { get; set; }
        
        /// <summary>
        /// The rotation of the emitter relative to it's parent's rotation, or it's rotation relative to straight up.
        /// </summary>
        public float RotationOffsetInRadians { get; set; }

        /// <summary>
        /// Controls whether the emitter is actively running logic that will cause it to emit new particles.  If a
        /// one shot trigger is being used, than after the emitter emits new particles this will immediately set back
        /// to false until manually set back to true.
        /// </summary>
        public bool IsEmitting
        {
            get => Emitter.IsEmittingNewParticles;
            set => Emitter.IsEmittingNewParticles = value;
        }

        /// <summary>
        /// If true, then the emitter and its particles will no longer be updated while the current FRB screen
        /// is in a paused state.
        /// </summary>
        public bool StopsOnScreenPause { get; set; } = true;
        
        /// <summary>
        /// If true, when this emitter is destroyed all particles will immediately be killed and removed.
        ///
        /// If false then when `Destroy()` is called the emitter will still have it's existing particles rendered and
        /// updated until all it's particles are dead.  Only then will the emitter be fully cleaned up. 
        /// </summary>
        public bool ImmediatelyKillParticlesOnDestroy { get; set; }

        public ParmeFrbEmitter(ParticlePool particlePool, IEmitterLogic logic)
        {
            Emitter = new MonoGameEmitter(logic,
                particlePool,
                FlatRedBallServices.GraphicsDevice,
                new FrbTextureFileLoader())
            {
                IsEmittingNewParticles = true
            };
        }

        public void UpdatePosition()
        {
            if (Parent != null)
            {
                Emitter.WorldCoordinates = new Vector2(
                    Parent.X + XOffset,
                    Parent.Y + YOffset);

                Emitter.RotationInRadians = Parent.RotationZ + RotationOffsetInRadians;
            }
            else
            {
                Emitter.WorldCoordinates = new Vector2(XOffset, YOffset);
                Emitter.RotationInRadians = RotationOffsetInRadians;
            }
        }

        public void Destroy()
        {
            ParmeEmitterManager.Instance.DestroyEmitter(this, !ImmediatelyKillParticlesOnDestroy);
        }
    }
}