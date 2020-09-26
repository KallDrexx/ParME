using System;
using Parme.Core.Triggers;

namespace Parme.CSharp.CodeGen.Triggers
{
    public class DistanceBasedTriggerCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(DistanceBasedTrigger);
        
        public string GenerateProperties(object obj)
        {
            var trigger = (DistanceBasedTrigger) obj;

            return $@"public float DistanceBasedTriggerUnitsPerEmission {{ get; set; }} = {trigger.UnitsPerEmission}f;";
        }

        public string GenerateFields(object obj)
        {
            return @"private Vector2 _lastEmittedPosition;";
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"
            shouldCreateNewParticle = false;
            var distance = Math.Abs(Vector2.Distance(_lastEmittedPosition, parent.WorldCoordinates));
            if (DistanceBasedTriggerUnitsPerEmission > 0 && distance >= DistanceBasedTriggerUnitsPerEmission)
            {
                shouldCreateNewParticle = true;
                _lastEmittedPosition = parent.WorldCoordinates;
            }
";
        }
    }
}