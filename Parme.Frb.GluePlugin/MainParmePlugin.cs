using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using FlatRedBall.Glue;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.Errors;
using FlatRedBall.Glue.Plugins;
using FlatRedBall.Glue.Plugins.ExportedImplementations;
using FlatRedBall.Glue.Plugins.Interfaces;
using FlatRedBall.Glue.SaveClasses;
using FlatRedBall.IO;
using Parme.Core;
using Parme.CSharp.CodeGen;

namespace Parme.Frb.GluePlugin
{
    [Export(typeof(PluginBase))]
    public class MainParmePlugin : PluginBase
    {
        public const string Extension = "emlogic";
        public const string ExtensionWithPeriod = "." + Extension;
        
        private readonly AssetTypeInfoManager _assetTypeInfoManager = new AssetTypeInfoManager();
        
        public override string FriendlyName => "ParME GluePlugin";

        public override Version Version
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                return new Version(fileVersionInfo.ProductVersion);
            }
        }
        
        public override void StartUp()
        {
            ReactToLoadedGlux += GluxLoaded;
            ReactToFileRemoved += FileRemoved;
            ReactToUnloadedGlux += GluxUnloaded;
            ReactToFileChangeHandler += FileChangeHandler;
            ReactToNewFileHandler += NewFileHandler;
            
            AvailableAssetTypes.Self.AddAssetType(_assetTypeInfoManager.LogicAssetTypeInfo);
            AvailableAssetTypes.Self.AddAssetType(_assetTypeInfoManager.FileAssetTypeInfo);
        }

        private static string GenerateEmitterLogic(EmitterSettings settings, string className)
        {
            return EmitterLogicClassGenerator.Generate(settings,
                GlueState.Self.ProjectNamespace,
                className,
                false);
        }

        private static string GetLogicClassName(string filename)
        {
            var name = Path.GetFileNameWithoutExtension(filename) + "EmitterLogic";
            if (char.IsLower(name[0]))
            {
                name = char.ToUpper(name[0]) + name.Substring(1);
            }
            
            return name.Replace("-", "")
                .Replace(" ", "");
        }

        private void ParseAndGenerateEmitterLogic(string filename)
        {
            var className = GetLogicClassName(filename);
            string json;
            try
            {
                json = File.ReadAllText(filename);
            }
            catch (IOException)
            {
                // File can't be read yet, so ignore it for now
                return;
            }

            EmitterSettings emitter;
            try
            {
                emitter = EmitterSettings.FromJson(json);
            }
            catch (Exception exception)
            {
                var message = $"Failed to parse emitter settings from `{filename}`: {exception}";
                GlueCommands.Self.PrintError(message);

                return;
            }

            try
            {
                GenerateAndSave(emitter, className, filename);
            }
            catch (Exception exception)
            {
                var message = $"Failed to generate emitter logic class for '{className}': {exception}";
                GlueCommands.Self.PrintError(message);

                return;
            }
            
            _assetTypeInfoManager.EmitterLogicTypes.Add(className);
        }

        private static void GenerateAndSave(EmitterSettings emitter, string logicClassName, string filename)
        {
            var prevTextureFileName = emitter.TextureFileName;
            if (!string.IsNullOrWhiteSpace(emitter.TextureFileName))
            {
                var projectPath = GlueState.Self.CurrentGlueProjectDirectory;
                var absolutePath = ProjectManager.MakeAbsolute(filename);
                var relativePath = FileManager.MakeRelative(absolutePath, projectPath);
                var directory = Path.GetDirectoryName(relativePath);
                emitter.TextureFileName = Path.Combine(directory, emitter.TextureFileName);
            }
            
            var code = GenerateEmitterLogic(emitter, logicClassName);
            var codeGenFilePath =
                new FilePath(Path.Combine(GlueState.Self.CurrentGlueProjectDirectory, "Particles", $"{logicClassName}.generated.cs"));
            GlueCommands.Self.ProjectCommands.CreateAndAddCodeFile(codeGenFilePath);
            File.WriteAllText(codeGenFilePath.FullPath, code);
            
            emitter.TextureFileName = prevTextureFileName;
        }

        private void NewFileHandler(ReferencedFileSave newFile)
        {
            if (Path.GetExtension(newFile.Name) != ExtensionWithPeriod)
            {
                return;
            }

            var filename = GlueCommands.Self.GetAbsoluteFileName(newFile);
            ParseAndGenerateEmitterLogic(filename);
        }

        private void FileChangeHandler(string filename)
        {
            if (Path.GetExtension(filename) != ExtensionWithPeriod)
            {
                return;
            }

            var referencedFile = GlueCommands.Self.GluxCommands.GetReferencedFileSaveFromFile(filename);
            if (referencedFile == null)
            {
                // Ignore since it's not part of the project
                return;
            }

            ParseAndGenerateEmitterLogic(filename);
        }

        public override bool ShutDown(PluginShutDownReason shutDownReason)
        {
            return true;
        }

        private void FileRemoved(IElement element, ReferencedFileSave removedFile)
        {
            var name = GetLogicClassName(removedFile.Name);
            _assetTypeInfoManager.EmitterLogicTypes.Remove(name);
        }

        private void GluxUnloaded()
        {
            _assetTypeInfoManager.EmitterLogicTypes.Clear();
        }

        private void GluxLoaded()
        {
            var emitterFiles = ObjectFinder.Self
                .GetAllReferencedFiles()
                .Where(x => Path.GetExtension(x.Name).Equals(ExtensionWithPeriod, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            
            foreach (var emitterFile in emitterFiles)
            {
                NewFileHandler(emitterFile);
            }
        }
    }
}