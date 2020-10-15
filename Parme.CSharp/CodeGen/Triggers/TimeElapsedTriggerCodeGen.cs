using System;
using Parme.Core.Triggers;

namespace Parme.CSharp.CodeGen.Triggers
{
    internal class TimeElapsedTriggerCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(TimeElapsedTrigger);

        public string GenerateProperties(object obj)
        {
            var trigger = (TimeElapsedTrigger) obj;
            
            return $@"public float TimeElapsedTriggerFrequency {{ get; set; }} = {trigger.Frequency}f; 
";
        }

        public string GenerateFields(object obj)
        {
            return @"private float _timeSinceLastTrigger;";
        }

        public string GenerateExecutionCode(object obj)
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

        public string GenerateCapacityEstimationCode(object obj)
        {
            return "triggersPerSecond = 1 / TimeElapsedTriggerFrequency;";
        }
    }
}