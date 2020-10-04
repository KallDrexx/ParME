namespace Parme.Editor.AppOperations
{
    public class CloseModalRequested : IAppOperation
    {
        private readonly Modal _modal;

        public CloseModalRequested(Modal modal)
        {
            _modal = modal;
        }

        public AppOperationResult Run()
        {
            return new AppOperationResult
            {
                ModalToClose = _modal,
            };
        }
    }
}