using Parme.Core;

namespace Parme.Editor.AppOperations
{
    public class EmitterUpdatedNotification : IAppOperation
    {
        public EmitterSettings UpdatedEmitter { get; }

        public EmitterUpdatedNotification(EmitterSettings updatedEmitter)
        {
            UpdatedEmitter = updatedEmitter;
        }
        
        public AppOperationResult Run()
        {
            return new AppOperationResult
            {
                UpdatedSettings = UpdatedEmitter,
            };
        }
    }
}