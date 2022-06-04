using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using FlatRedBall.Glue;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.Parsing;
using FlatRedBall.Glue.Plugins;
using FlatRedBall.Glue.Plugins.ExportedImplementations;
using FlatRedBall.Glue.Plugins.Interfaces;
using FlatRedBall.Glue.SaveClasses;
using FlatRedBall.IO;
using Parme.Core;
using Parme.Core.Serialization;
using Parme.CSharp.CodeGen;

namespace Parme.Frb.GluePlugin
{
    [Export(typeof(PluginBase))]
    public class MainParmePlugin : PluginBase
    {
        public const string Extension = "emlogic";
        public const string EmitterLogicSuffix = "EmitterLogic";
        
        private const string ExtensionWithPeriod = "." + Extension;

        private readonly AssetTypeInfoManager _assetTypeInfoManager = new();
        private readonly EmitterLogicMapperGenerator _emitterLogicMapperGenerator = new();
        private readonly EmitterLogicMapperInitializer _emitterLogicMapperInitializer = new();

        public override string FriendlyName => "ParME GluePlugin";

        public override Version Version
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                
                // If this has a pre-release tag, remove it.  Otherwise it can't be parsed to a Version type
                var dashIndex = fileVersionInfo.ProductVersion.IndexOf('-');
                var versionString = dashIndex >= 0
                    ? fileVersionInfo.ProductVersion.Substring(0, dashIndex)
                    : fileVersionInfo.ProductVersion;
                
                return new Version(versionString);
            }
        }
        
        public override void StartUp()
        {
            ReactToLoadedGlux += GluxLoaded;
            ReactToFileRemoved += FileRemoved;
            ReactToUnloadedGlux += GluxUnloaded;
            ReactToFileChangeHandler += FileChangeHandler;
            ReactToNewFileHandler += NewFileHandler;

            AvailableAssetTypes.Self.AddAssetType(_assetTypeInfoManager.FileAssetTypeInfo);
            AvailableAssetTypes.Self.AddAssetType(_assetTypeInfoManager.LogicAssetTypeInfo);
            RegisterCodeGenerator(_emitterLogicMapperInitializer);
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
            var name = Path.GetFileNameWithoutExtension(filename) + EmitterLogicSuffix;
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
            catch (MissingParmeTypeException exception)
            {
                var message = $"The emitter logic defined in '{filename}' could not be read, as it is expecting a " +
                              $"ParME type of '{exception.TypeName}', but this type is not known.";

                MessageBox.Show(message);

                return;
            }
            catch (Exception exception)
            {
                var message = $"Failed to parse emitter logic defined in `{filename}`: {exception.Message}" +
                              $"{Environment.NewLine}{Environment.NewLine}{exception}";
                MessageBox.Show(message);

                return;
            }

            try
            {
                GenerateAndSaveEmitterCode(emitter, className, filename);
            }
            catch (Exception exception)
            {
                var message = $"Failed to generate emitter logic class for '{className}': {exception}";
                GlueCommands.Self.PrintError(message);

                return;
            }
            
            _assetTypeInfoManager.AddEmitterLogicTypeName(className);
            _emitterLogicMapperGenerator.AddEmitterLogicTypeName(className);
            GenerateAndSaveLogicMapperCode();
            GlueCommands.Self.PrintOutput($"Successfully generated code for the '{className}' emitter");
        }

        private static void GenerateAndSaveEmitterCode(EmitterSettings emitter, string logicClassName, string filename)
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
            var codeGenFilePath = new FilePath(GetGeneratedFilePath(logicClassName));
            GlueCommands.Self.ProjectCommands.CreateAndAddCodeFile(codeGenFilePath);
            File.WriteAllText(codeGenFilePath.FullPath, code);
            
            emitter.TextureFileName = prevTextureFileName;
        }

        private static string GetGeneratedFilePath(string logicClassName)
        {
            return Path.Combine(
                GlueState.Self.CurrentGlueProjectDirectory, 
                "Particles",
                $"{logicClassName}.generated.cs");
        }

        private void GenerateAndSaveLogicMapperCode()
        {
            var code = _emitterLogicMapperGenerator.GenerateMapperImplementation(GlueState.Self.ProjectNamespace);
            var codeGenPath = new FilePath(Path.Combine(
                GlueState.Self.CurrentGlueProjectDirectory,
                "Particles",
                "ParmeEmitterLogicGenerator.generated.cs"));

            GlueCommands.Self.ProjectCommands.CreateAndAddCodeFile(codeGenPath);
            File.WriteAllText(codeGenPath.FullPath, code);
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
            AvailableAssetTypes.Self.RemoveAssetType(_assetTypeInfoManager.FileAssetTypeInfo);
            AvailableAssetTypes.Self.RemoveAssetType(_assetTypeInfoManager.LogicAssetTypeInfo);
            
            UnregisterAllCodeGenerators();
            
            return true;
        }

        private void FileRemoved(IElement element, ReferencedFileSave removedFile)
        {
            if (Path.GetExtension(removedFile.Name) != $".{Extension}")
            {
                return;
            }
            
            var name = GetLogicClassName(removedFile.Name);
            _assetTypeInfoManager.RemoveEmitterLogicTypeName(name);
            _emitterLogicMapperGenerator.RemoveEmitterLogicTypeName(name);

            var generatedFilePath = new FilePath(GetGeneratedFilePath(name));
            if (generatedFilePath.Exists())
            {
                GlueCommands.Self.ProjectCommands.RemoveFromProjects(generatedFilePath);
                File.Delete(generatedFilePath.FullPath);
            }
        }

        private void GluxUnloaded()
        {
            _assetTypeInfoManager.ClearEmitterLogicTypes();
            _emitterLogicMapperGenerator.ClearEmitterLogicTypes();
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

            _emitterLogicMapperInitializer.ProjectNamespace = GlueState.Self.ProjectNamespace;
        }
    }
}