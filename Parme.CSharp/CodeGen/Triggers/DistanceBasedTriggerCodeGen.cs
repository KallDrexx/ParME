using System;
using Parme.Core.Triggers;

namespace Parme.CSharp.CodeGen.Triggers
{
    public class DistanceBasedTriggerCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(DistanceBasedTrigger);
        
        public override FormattableString GenerateProperties(object obj)
        {
            var trigger = (DistanceBasedTrigger) obj;

            return $@"public float DistanceBasedTriggerUnitsPerEmission {{ get; set; }} = {trigger.UnitsPerEmission}f;";
        }

        public override FormattableString GenerateFields(object obj)
        {
            return $@"private Vector2 _lastEmittedPosition;";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
            shouldCreateNewParticle = false;
            var distance = Math.Abs(Vector2.Distance(_lastEmittedPosition, parent.WorldCoordinates));
            if (DistanceBasedTriggerUnitsPerEmission > 0 && distance >= DistanceBasedTriggerUnitsPerEmission)
            {{
                shouldCreateNewParticle = true;
                _lastEmittedPosition = parent.WorldCoordinates;
            }}
";
        }

        public override FormattableString GenerateCapacityEstimationCode(object obj)
        {
            // Impossible to predict, so just assume it will max at 3
            return $"triggersPerSecond = 3;";
        }
    }
}