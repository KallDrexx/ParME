using System.Collections.Concurrent;

namespace Parme.Editor.AppOperations
{
    public class AppOperationQueue
    {
        private readonly ConcurrentQueue<IAppOperation> _queue = new ConcurrentQueue<IAppOperation>();

        public void Enqueue(IAppOperation appOperation)
        {
            _queue.Enqueue(appOperation);
        }

        public bool TryDequeue(out IAppOperation appOperation) => _queue.TryDequeue(out appOperation);
    }
}