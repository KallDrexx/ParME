﻿using System;
using System.IO;
using Parme.Core;

namespace Parme.Editor.AppOperations
{
    public class SaveEmitterRequested : IAppOperation
    {
        public string FileName { get; }
        public EmitterSettings EmitterSettings { get; }

        public SaveEmitterRequested(string fileName, EmitterSettings emitterSettings)
        {
            FileName = fileName;
            EmitterSettings = emitterSettings;
        }

        public AppOperationResult Run()
        {
            if (string.IsNullOrWhiteSpace(FileName))
            {
                throw new InvalidOperationException("No filename given for save");
            }

            if (EmitterSettings == null)
            {
                // Nothing to save
                return new AppOperationResult();
            }

            var backupFileName = FileName + ".bak";
            var tempFile = Path.GetTempFileName();
            try
            {
                var json = EmitterSettings.ToJson();
                File.WriteAllText(tempFile, json);
                
                if (File.Exists(FileName))
                {
                    File.Replace(tempFile, FileName, backupFileName);
                    File.Delete(backupFileName);
                }
                else
                {
                    File.Move(tempFile, FileName);
                }
            }
            catch (Exception exception)
            {
                return new AppOperationResult
                {
                    NewErrorMessage = $"Failed to save emitter: {exception.Message}",
                };
            }

            return new AppOperationResult
            {
                ResetUnsavedChangesMarker = true,
                UpdatedFileName = FileName,
            };
        }
    }
}