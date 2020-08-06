namespace Parme.CSharp.CodeGen
{
    public interface IGenerateCode<T>
    {
        public string GenerateProperties(T obj);
        public string GenerateFields(T obj);
        public string GenerateExecutionCode(T obj);
    }
}