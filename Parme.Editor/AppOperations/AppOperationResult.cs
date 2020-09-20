using Parme.Core;

namespace Parme.Editor.AppOperations
{
    public class AppOperationResult
    {
        public string NewErrorMessage { get; set; }
        public bool RemoveErrorMessage { get; set; }
        public EmitterSettings UpdatedSettings { get; set; }
        public string UpdatedFileName { get; set; }
        public bool ResetUnsavedChangesMarker { get; set; }
    }
}