using System.Numerics;

namespace Parme.Editor.AppOperations
{
    public class UpdateViewOptionsRequested : IAppOperation
    {
        public decimal? UpdatedZoomLevel { get; set; }
        public Vector3? UpdatedBackgroundColor { get; set; }
        
        public AppOperationResult Run()
        {
            return new AppOperationResult
            {
                UpdatedZoomLevel = UpdatedZoomLevel,
                UpdatedBackgroundColor = UpdatedBackgroundColor,
            };
        }
    }
}