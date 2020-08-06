using Parme.Core.Triggers;

namespace Parme.CSharp.CodeGen.Triggers
{
    public class TimeElapsedTriggerCodeGen : IGenerateCode<TimeElapsedTrigger>
    {
        public string GenerateProperties(TimeElapsedTrigger obj)
        {
            return $@"public float TimeElapsedTriggerFrequency {{ get; set; }} = {obj.Frequency}f; 
";
        }

        public string GenerateFields(TimeElapsedTrigger obj)
        {
            return @"private float _timeSinceLastTrigger;";
        }

        public string GenerateExecutionCode(TimeElapsedTrigger obj)
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
    }
}