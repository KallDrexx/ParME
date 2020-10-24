using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class DragCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(DragModifier);
        
        public FormattableString GenerateProperties(object obj)
        {
            var modifier = (DragModifier) obj;

            return $@"
        public float DragFactor {{ get; set; }} = {modifier.DragFactor}f; 
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"particle.Velocity -= DragFactor * particle.Velocity * timeSinceLastFrame;";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}