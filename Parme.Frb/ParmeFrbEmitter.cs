using System;
using FlatRedBall;
using FlatRedBall.Math;
using Microsoft.Xna.Framework;
using Parme.CSharp;
using Parme.MonoGame;
using Vector2 = System.Numerics.Vector2;

namespace Parme.Frb
{
    public class ParmeFrbEmitter : IStaticPositionable
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

        public ParmeFrbEmitter(ParticlePool particlePool, IEmitterLogic logic, Random random)
        {
            Emitter = new MonoGameEmitter(logic,
                particlePool,
                FlatRedBallServices.GraphicsDevice,
                new FrbTextureFileLoader(),
                random)
            {
                IsEmittingNewParticles = true
            };
        }

        public void UpdatePosition()
        {
            if (Parent != null)
            {
                var parentPosition = Parent.Position;
                var parentRotationMatrix = Parent.RotationMatrix;

                var offset = parentRotationMatrix.Right * XOffset + parentRotationMatrix.Up * YOffset;
                var absoluteParticlePosition = parentPosition + offset;

                Emitter.WorldCoordinates = new Vector2(
                    absoluteParticlePosition.X,
                    absoluteParticlePosition.Y);
                
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

        private void SetRotatedOffsetFromAbsoluteValues(float x, float y)
        {
            if (Parent == null)
            {
                XOffset = x;
                YOffset = y;
            }
            else
            {
                var absoluteCoordinates = new Vector3(x, y, 0);
                var tempVector = absoluteCoordinates - Parent.Position;
                var invertedMatrix = Matrix.Invert(Parent.RotationMatrix);

                XOffset =
                    invertedMatrix.M11 * tempVector.X +
                    invertedMatrix.M21 * tempVector.Y;

                YOffset =
                    invertedMatrix.M12 * tempVector.X +
                    invertedMatrix.M22 * tempVector.Y;

                // The x and y that were passed in were absolute values, so set the world coordinates to the passed
                // in values.  We have to do this immediately, otherwise a call to `X`'s setter then `Y`'s setter will
                // reset the X value, as the setters use world coordinates for values to this method.
                Emitter.WorldCoordinates = new Vector2(x, y);
            }
        }
        
        float IStaticPositionable.X
        {
            get => Emitter.WorldCoordinates.X;
            set => SetRotatedOffsetFromAbsoluteValues(value, Emitter.WorldCoordinates.Y);
        }

        float IStaticPositionable.Y
        {
            get => Emitter.WorldCoordinates.Y;
            set => SetRotatedOffsetFromAbsoluteValues(Emitter.WorldCoordinates.X, value);
        }

        float IStaticPositionable.Z
        {
            get => Parent?.Z ?? 0;
            set { }
        }
    }
}