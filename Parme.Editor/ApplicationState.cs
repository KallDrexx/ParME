using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Parme.Core;
using Parme.Editor.AppOperations;

namespace Parme.Editor
{
    public class ApplicationState
    {
        private float _currentTime;
        
        public Version Version { get; }
        public string ActiveFileName { get; private set; }
        public EmitterSettings ActiveEmitter { get; private set; }
        public string ErrorMessage { get; private set; }
        public float TimeLastEmitterUpdated { get; private set; }
        public bool EmitterUpdatedFromFileLoad { get; private set; }
        public bool HasUnsavedChanges { get; private set; }
        public int ParticleCount { get; set; }
        public decimal Zoom { get; set; } = 1;
        public Vector3 BackgroundColor { get; private set; }
        public SamplerState RenderSamplerState { get; private set; }

        public ApplicationState()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Version = new Version(fileVersionInfo.ProductVersion);
        }

        public void UpdateTotalTime(float totalTime)
        {
            _currentTime = totalTime;
        }

        public void Apply(AppOperationResult operationResult)
        {
            if (operationResult == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(operationResult.UpdatedFileName))
            {
                ActiveFileName = operationResult.UpdatedFileName;
            }
            
            if (operationResult.UpdatedSettings != null)
            {
                ActiveEmitter = operationResult.UpdatedSettings;
                HasUnsavedChanges = !operationResult.ResetUnsavedChangesMarker;
                TimeLastEmitterUpdated = _currentTime;
                EmitterUpdatedFromFileLoad = !string.IsNullOrWhiteSpace(operationResult.UpdatedFileName);
            }
            else if (operationResult.ResetUnsavedChangesMarker)
            {
                HasUnsavedChanges = false;
            }

            if (!string.IsNullOrWhiteSpace(operationResult.NewErrorMessage))
            {
                ErrorMessage = operationResult.NewErrorMessage;
            }
            else if (operationResult.RemoveErrorMessage)
            {
                ErrorMessage = null;
            }

            if (operationResult.UpdatedZoomLevel != null)
            {
                Zoom = operationResult.UpdatedZoomLevel.Value;
            }

            if (operationResult.UpdatedBackgroundColor != null)
            {
                BackgroundColor = operationResult.UpdatedBackgroundColor.Value;
            }

            if (operationResult.UpdatedSamplerState != null)
            {
                RenderSamplerState = operationResult.UpdatedSamplerState;
            }
        }
    }
}