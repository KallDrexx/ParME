using System;

namespace Parme.CSharp.CodeGen
{
    public abstract class ParticleCodeGenerator
    {
        public abstract Type ParmeObjectType { get; }
        public virtual FormattableString GenerateProperties(object obj) => $"";
        public virtual FormattableString GenerateFields(object obj) => $"";
        public virtual FormattableString GenerateExecutionCode(object obj) => $"";
        public virtual FormattableString GenerateCapacityEstimationCode(object obj) => $"";
        public virtual FormattableString GeneratePositionExecutionCode(object obj) => $"";
    }
}