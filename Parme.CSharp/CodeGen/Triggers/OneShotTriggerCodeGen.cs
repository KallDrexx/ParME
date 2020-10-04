using System;
using Parme.Core.Triggers;

namespace Parme.CSharp.CodeGen.Triggers
{
    internal class OneShotTriggerCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(OneShotTrigger);

        public string GenerateProperties(object obj)
        {
            return string.Empty;
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"if (parent.IsEmittingNewParticles)
                {
                    shouldCreateNewParticle = true;
                    stopEmittingAfterUpdate = true;
                }";
        }
    }
}