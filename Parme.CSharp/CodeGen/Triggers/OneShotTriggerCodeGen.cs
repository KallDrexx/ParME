using System;
using Parme.Core.Triggers;

namespace Parme.CSharp.CodeGen.Triggers
{
    public class OneShotTriggerCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(OneShotTrigger);

        public string GenerateProperties(object obj)
        {
            return string.Empty;
        }

        public string GenerateFields(object obj)
        {
            return @"private bool _hasTriggered;
";
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"if (_hasTriggered)
            {
                shouldCreateNewParticle = false;
            }
            else
            {
                shouldCreateNewParticle = true;
                _hasTriggered = true;
            }
";
        }
    }
}