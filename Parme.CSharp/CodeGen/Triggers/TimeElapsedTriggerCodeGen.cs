using System;
using Parme.Core.Triggers;

namespace Parme.CSharp.CodeGen.Triggers
{
    internal class TimeElapsedTriggerCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(TimeElapsedTrigger);

        public override FormattableString GenerateProperties(object obj)
        {
            var trigger = (TimeElapsedTrigger) obj;
            
            return $@"public float TimeElapsedTriggerFrequency {{ get; set; }} = {trigger.Frequency}f; 
";
        }

        public override FormattableString GenerateFields(object obj)
        {
            return $@"private float _timeSinceLastTrigger;";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
            shouldCreateNewParticle = false;
            _timeSinceLastTrigger += timeSinceLastFrame;
            if (_timeSinceLastTrigger >= TimeElapsedTriggerFrequency)
            {{
                shouldCreateNewParticle = true;
                _timeSinceLastTrigger = 0;  
            }}          
";
        }

        public override FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"triggersPerSecond = 1 / TimeElapsedTriggerFrequency;";
        }
    }
}