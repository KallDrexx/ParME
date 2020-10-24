using System;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen.Initializers
{
    internal class StaticPositionCodeGen : IGenerateCode
    {
        public Type ParmeObjectType => typeof(StaticPositionInitializer);

        public FormattableString GenerateProperties(object obj)
        {
            var initializer = (StaticPositionInitializer) obj;
            
            return $@"
        public float StaticPositionXOffset {{ get; set; }} = {initializer.XOffset}f;
        public float StaticPositionYOffset {{ get; set; }} = {initializer.YOffset};
";
        }

        public FormattableString GenerateFields(object obj)
        {
            return $"";
        }

        public FormattableString GenerateExecutionCode(object obj)
        {
            return $@"
                        particle.Position = new Vector2(StaticPositionXOffset, StaticPositionYOffset);
";
        }

        public FormattableString GenerateCapacityEstimationCode(object obj)
        {
            return $"";
        }
    }
}