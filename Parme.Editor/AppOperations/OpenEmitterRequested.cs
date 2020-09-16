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
        
        public void Run(App app)
        {
            string json;
            try
            {
                json = File.ReadAllText(FileName);
            }
            catch (Exception exception)
            {
                app.ErrorMessageRaised($"Failed to load file '{FileName}': {exception.Message}");
                
                return;
            }

            EmitterSettings emitter;
            try
            {
                emitter = EmitterSettings.FromJson(json);
            }
            catch (Exception exception)
            {
                var message = $"File '{FileName}' did not contain valid emitter details: {exception.Message}";
                app.ErrorMessageRaised(message);

                return;
            }
            
            app.EmitterLoadedFromFile(emitter, FileName);
        }
    }
}