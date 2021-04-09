using System;
using Parme.Core.Triggers;

namespace Parme.CSharp.CodeGen.Triggers
{
    internal class OneShotTriggerCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(OneShotTrigger);

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"if (parent.IsEmittingNewParticles)
                {{
                    shouldCreateNewParticle = true;
                    stopEmittingAfterUpdate = true;
                }}";
        }

        public override FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"triggersPerSecond = 1 / MaxParticleLifeTime;";
        }
    }
}