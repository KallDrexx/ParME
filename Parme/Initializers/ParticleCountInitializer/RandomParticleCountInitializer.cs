using System;

namespace Parme.Initializers.ParticleCountInitializer
{
    public class RandomParticleCountInitializer : IParticleCountInitializer
    {
        private readonly Random _random = new Random();
        private readonly int _min, _max;

        public RandomParticleCountInitializer(int min, int max)
        {
            if (min < 0 || min > max)
            {
                var message = $"Min/max of {min}/{max} is invalid.  Values must be greater than zero and " +
                              $"max must be greater than the min";
                              
                throw new ArgumentException(message);
            }
            
            _min = min;
            _max = max;
        }

        public int GetNewParticleCount()
        {
            return _random.Next(_min, _max + 1);
        }

        public string GetCSharpExecutionCode()
        {
            return $"newParticleCount = _random.Next({_min}, {_max});";
        }
    }
}