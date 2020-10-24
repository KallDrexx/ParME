using System;
using Parme.Core.Triggers;

namespace Parme.CSharp.CodeGen.Triggers
{
    internal class OneShotTriggerCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(OneShotTrigger);

        public FormattableString GenerateProperties(object obj)
        {
            return $"";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"if (parent.IsEmittingNewParticles)
                {{
                    shouldCreateNewParticle = true;
                    stopEmittingAfterUpdate = true;
                }}";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"triggersPerSecond = 1 / MaxParticleLifeTime;";
        }
    }
}