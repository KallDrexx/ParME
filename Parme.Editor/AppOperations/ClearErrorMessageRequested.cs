namespace Parme.Editor.AppOperations
{
    public class ClearErrorMessageRequested : IAppOperation
    {
        public AppOperationResult Run()
        {
            return new AppOperationResult
            {
                RemoveErrorMessage = true,
            };
        }
    }
}