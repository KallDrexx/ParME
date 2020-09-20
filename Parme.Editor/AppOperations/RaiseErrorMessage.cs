namespace Parme.Editor.AppOperations
{
    public class RaiseErrorMessage : IAppOperation
    {
        public string ErrorMessage { get; }

        public RaiseErrorMessage(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        
        public AppOperationResult Run()
        {
            return new AppOperationResult
            {
                NewErrorMessage = ErrorMessage,
            };
        }
    }
}