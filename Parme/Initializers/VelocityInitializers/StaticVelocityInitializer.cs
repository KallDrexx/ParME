using Microsoft.Xna.Framework;

namespace Parme.Initializers.VelocityInitializers
{
    public class StaticVelocityInitializer : IVelocityInitializer
    {
        private readonly Vector2 _velocity;

        public StaticVelocityInitializer(Vector2 velocity)
        {
            _velocity = velocity;
        }

        public Vector2 GetNewParticleVelocity()
        {
            return _velocity;
        }

        public string GetCSharpExecutionCode()
        {
            return $@"
            particle.Velocity = new Vector2((float) {_velocity.X}, (float) {_velocity.Y});
";
        }
    }
}