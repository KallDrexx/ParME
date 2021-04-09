using System;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen.Modifiers
{
    internal class ConstantSizeCodeGeneratorGen : ParticleCodeGenerator
    {
        public override Type ParmeObjectType => typeof(ConstantSizeModifier);

        public override FormattableString GenerateProperties(object obj)
        {
            var modifier = (ConstantSizeModifier) obj;
            
            return $@"
        public float ConstantSizeWidthChangePerSecond {{ get; set; }} = {modifier.WidthChangePerSecond}f;
        public float ConstantSizeHeightChangePerSecond {{ get; set; }} = {modifier.HeightChangePerSecond}f;
";
        }

        public override FormattableString GenerateExecutionCode(object obj)
        {
            return $@"particle.Size += timeSinceLastFrame * new Vector2(ConstantSizeWidthChangePerSecond, ConstantSizeHeightChangePerSecond);
";
        }
    }
}