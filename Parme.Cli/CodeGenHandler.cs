using System;
using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using Parme.Core;
using Parme.Core.Serialization;
using Parme.CSharp.CodeGen;

namespace Parme.Cli
{
    public static class CodeGenHandler
    {
        public static void Handle(string inputFile, 
            string outputFile, 
            string language, 
            string className, 
            string @namespace, 
            IConsole console)
        {
            if (!File.Exists(inputFile))
            {
                console.Error.WriteLine($"The file '{inputFile}' does not exist");
                return;
            }

            if (string.IsNullOrWhiteSpace(outputFile))
            {
                console.Error.WriteLine("An output file is required");
                return;
            }

            switch (language?.ToLower().Trim())
            {
                case "csharp":
                    GenerateCSharpCode(inputFile, outputFile, className, @namespace, console);
                    break;
                
                default:
                    console.Error.WriteLine("Invalid language option.  Valid options: \"csharp\"");
                    return;
            }
        }

        private static void GenerateCSharpCode(string inputFile, string outputFile, string className, string @namespace, IConsole console)
        {
            console.Out.WriteLine($"Generating C# code for emitter logic file '{inputFile}' into '{outputFile}'");
            
            string json;
            try
            {
                json = File.ReadAllText(inputFile);
            }
            catch (Exception exception)
            {
                console.Error.WriteLine($"Failed to read input file '{inputFile}': {exception.Message}");
                return;
            }

            EmitterSettings emitterSettings;
            try
            {
                emitterSettings = EmitterSettings.FromJson(json);
            }
            catch (MissingParmeTypeException exception)
            {
                var message = $"The emitter logic defined in '{inputFile}' could not be read, as it is expecting a " +
                              $"ParME type of '{exception.TypeName}', but this type is not known.";

                console.Error.WriteLine(message);

                return;
            }
            catch (Exception exception)
            {
                var message = $"Failed to parse emitter logic defined in `{inputFile}`: {exception.Message}" +
                              $"{Environment.NewLine}{Environment.NewLine}{exception}";

                console.Error.WriteLine(message);
                return;
            }

            @namespace ??= "Parme.Generated";
            className ??= Path.GetFileNameWithoutExtension(inputFile);
            var code = EmitterLogicClassGenerator.Generate(emitterSettings, @namespace, className, false);

            try
            {
                File.WriteAllText(outputFile, code);
            }
            catch (Exception exception)
            {
                console.Error.WriteLine($"Failed to write generated code to '{outputFile}': {exception.Message}");
                return;
            }
            
            console.Out.WriteLine("Parme emitter logic code successfully generated");
        }
    }
}