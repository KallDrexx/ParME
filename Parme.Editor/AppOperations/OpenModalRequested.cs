namespace Parme.Editor.AppOperations
{
    public class OpenModalRequested : IAppOperation
    {
        private readonly Modal _modal;

        public OpenModalRequested(Modal modal)
        {
            _modal = modal;
        }
        
        public AppOperationResult Run()
        {
            return new AppOperationResult
            {
                ModalToOpen = _modal,
            };
        }
    }
}