using System;

namespace Parme.CSharp.CodeGen
{
    public interface IGenerateCode
    {
        Type ParmeObjectType { get; }
        FormattableString GenerateProperties(object obj);
        FormattableString GenerateFields(object obj);
        FormattableString GenerateExecutionCode(object obj);
        FormattableString GenerateCapacityEstimationCode(object obj);
    }
}