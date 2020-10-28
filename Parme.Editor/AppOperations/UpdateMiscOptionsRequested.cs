﻿using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace Parme.Editor.AppOperations
{
    public class UpdateMiscOptionsRequested : IAppOperation
    {
        public decimal? UpdatedZoomLevel { get; set; }
        public Vector3? UpdatedBackgroundColor { get; set; }
        public SamplerState UpdatedSamplerState { get; set; }
        public int? UpdatedGridSize { get; set; }
        public bool? UpdatedAutoSave { get; set; }
        
        public AppOperationResult Run()
        {
            return new AppOperationResult
            {
                UpdatedZoomLevel = UpdatedZoomLevel,
                UpdatedBackgroundColor = UpdatedBackgroundColor,
                UpdatedSamplerState = UpdatedSamplerState,
                UpdatedGridSize = UpdatedGridSize,
                UpdatedAutoSave = UpdatedAutoSave,
            };
        }
    }
}