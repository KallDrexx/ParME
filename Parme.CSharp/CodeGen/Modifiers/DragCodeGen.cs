using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class DragCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(DragModifier);
        
        public string GenerateProperties(object obj)
        {
            var modifier = (DragModifier) obj;

            return $@"
        public float DragFactor {{ get; set; }} = {modifier.DragFactor}f; 
";
        }

        public string GenerateFields(object obj)
        {
            return string.Empty;
        }

        public string GenerateExecutionCode(object obj)
        {
            return @"particle.Velocity -= DragFactor * particle.Velocity * timeSinceLastFrame;";
        }

        public string GenerateCapacityEstimationCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}