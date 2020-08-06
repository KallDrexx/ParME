using Parme.Core.Triggers;

namespace Parme.CSharp.CodeGen.Triggers
{
    public class OneShotTriggerCodeGen : IGenerateCode<OneShotTrigger>
    {
        public string GenerateProperties(OneShotTrigger obj)
        {
            return string.Empty;
        }

        public string GenerateFields(OneShotTrigger obj)
        {
            return @"private bool _hasTriggered;
";
        }

        public string GenerateExecutionCode(OneShotTrigger obj)
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