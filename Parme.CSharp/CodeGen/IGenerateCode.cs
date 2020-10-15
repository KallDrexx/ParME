using System;

namespace Parme.CSharp.CodeGen
{
    public interface IGenerateCode
    {
        Type ParmeObjectType { get; }
        string GenerateProperties(object obj);
        string GenerateFields(object obj);
        string GenerateExecutionCode(object obj);
        string GenerateCapacityEstimationCode(object obj);
    }
}