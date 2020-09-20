using System;
using System.IO;
using Parme.Core;

namespace Parme.Editor.AppOperations
{
    public class OpenEmitterRequested : IAppOperation
    {
        public string FileName { get; }

        public OpenEmitterRequested(string fileName)
        {
            FileName = fileName;
        }
        
        public AppOperationResult Run()
        {
            string json;
            try
            {
                json = File.ReadAllText(FileName);
            }
            catch (Exception exception)
            {
                return new AppOperationResult
                {
                    NewErrorMessage = $"Failed to load file '{FileName}': {exception.Message}",
                };
            }

            EmitterSettings emitter;
            try
            {
                emitter = EmitterSettings.FromJson(json);
            }
            catch (Exception exception)
            {
                return new AppOperationResult
                {
                    NewErrorMessage = $"File '{FileName}' did not contain valid emitter details: {exception.Message}",
                };
            }

            return new AppOperationResult
            {
                UpdatedSettings = emitter,
                UpdatedFileName = FileName,
                ResetUnsavedChangesMarker = true,
            };
        }
    }
}