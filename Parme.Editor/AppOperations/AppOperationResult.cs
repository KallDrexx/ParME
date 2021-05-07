using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
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
        public decimal? UpdatedZoomLevel { get; set; }
        public Vector3? UpdatedBackgroundColor { get; set; }
        public SamplerState UpdatedSamplerState { get; set; }
        public int? UpdatedGridSize { get; set; }
        public Modal? ModalToClose { get; set; }
        public Modal? ModalToOpen { get; set; }
        public bool? UpdatedAutoSave { get; set; }
        public bool? ResetCamera { get; set; }
    }
}