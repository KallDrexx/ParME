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
            FillWithReferencedFiles += CustomFillWithReferencedFiles;
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

        private void NewFileHandler(ReferencedFileSave newfile)
        {
            if (Path.GetExtension(newfile.Name) != ".emitter")
            {
                return;
            }
            
            var name = GetLogicClassName(newfile.Name);
            var json = File.ReadAllText(GlueCommands.Self.GetAbsoluteFileName(newfile));
                
            var emitter = EmitterSettings.FromJson(json);
            GenerateAndSave(emitter, name, newfile.Name);
                
            _assetTypeInfoManager.EmitterLogicTypes.Add(name);
        }

        private void FileChangeHandler(string filename)
        {
            if (Path.GetExtension(filename) != ".emitter")
            {
                return;
            }
            
            var className = GetLogicClassName(filename);
            var json = File.ReadAllText(filename);
            var emitter = EmitterSettings.FromJson(json);
            GenerateAndSave(emitter, className, filename);
        }

        private GeneralResponse CustomFillWithReferencedFiles(FilePath currentFile, List<FilePath> referencedFiles)
        {
            if (currentFile.Extension != "emitter")
            {
                return GeneralResponse.SuccessfulResponse;
            }
            
            // Add an association of the emitter with the texture it's referencing
            var json = File.ReadAllText(currentFile.FullPath);
            var emitter = EmitterSettings.FromJson(json);
            if (!string.IsNullOrWhiteSpace(emitter.TextureFileName))
            {
                var texturePath = Path.IsPathRooted(emitter.TextureFileName)
                    ? emitter.TextureFileName
                    : Path.Combine(Path.GetDirectoryName(currentFile.FullPath), emitter.TextureFileName);
                
                referencedFiles.Add(new FilePath(texturePath));
            }
            
            // see if arg1 is a .emitter, then load that emitter, find pngs and add them to arg2
            // arg1 has full path, get directory of arg1 for emitter
            return GeneralResponse.SuccessfulResponse;
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
                .Where(x => Path.GetExtension(x.Name).Equals(".emitter", StringComparison.OrdinalIgnoreCase))
                .ToArray();
            
            foreach (var emitterFile in emitterFiles)
            {
                NewFileHandler(emitterFile);
            }
        }
    }
}