using System;
using Parme.Core.Triggers;

namespace Parme.CSharp.CodeGen.Triggers
{
    internal class TimeElapsedTriggerCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(TimeElapsedTrigger);

        public FormattableString GenerateProperties(object obj)
        {
            var trigger = (TimeElapsedTrigger) obj;
            
            return $@"public float TimeElapsedTriggerFrequency {{ get; set; }} = {trigger.Frequency}f; 
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $@"private float _timeSinceLastTrigger;";
        }

        public FormattableString GenerateExecutionCode(object obj)
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

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"triggersPerSecond = 1 / TimeElapsedTriggerFrequency;";
        }
    }
}