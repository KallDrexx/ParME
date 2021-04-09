using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    public class DragCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(DragModifier);
        
        public override FormattableString GenerateProperties(object obj)
        {
            var modifier = (DragModifier) obj;

            return $@"
        public float DragFactor {{ get; set; }} = {modifier.DragFactor}f; 
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"particle.Velocity -= DragFactor * particle.Velocity * timeSinceLastFrame;";
        }
    }
}