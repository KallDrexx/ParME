using Parme.Core;

namespace Parme.Editor
{
    public class AppState
    {
        public string NewErrorMessage { get; set; }
        public EmitterSettings UpdatedSettings { get; set; }
        public string UpdatedFileName { get; set; }
        public bool ResetUnsavedChangesMarker { get; set; }
    }
}