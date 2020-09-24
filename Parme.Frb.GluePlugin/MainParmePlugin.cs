using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.Errors;
using FlatRedBall.Glue.Plugins;
using FlatRedBall.Glue.Plugins.ExportedImplementations;
using FlatRedBall.Glue.Plugins.Interfaces;
using FlatRedBall.IO;
using Parme.Core;
using Parme.CSharp;
using Parme.CSharp.CodeGen;

namespace Parme.Frb.GluePlugin
{
    [Export(typeof(PluginBase))]
    public class MainParmePlugin : PluginBase
    {
        public override string FriendlyName => "ParME GluePlugin";
        public override Version Version => new Version(0, 0, 0, 1);
        
        public override void StartUp()
        {
            ReactToLoadedGlux += GluxLoaded;
            FillWithReferencedFiles += CustomFillWithReferencedFiles;
            
            foreach (var assetTypeInfo in new AssetTypeManager().GetDrawableBatchAssetTypes())
            {
                AvailableAssetTypes.Self.AddAssetType(assetTypeInfo);
            }
        }

        private GeneralResponse CustomFillWithReferencedFiles(FilePath arg1, List<FilePath> arg2)
        {
            // see if arg1 is a .emitter, then load that emitter, find pngs and add them to arg2
            // arg1 has full path, get directory of arg1 for emitter
            return GeneralResponse.SuccessfulResponse;
        }

        public override bool ShutDown(PluginShutDownReason shutDownReason)
        {
            return true;
        }

        private static string GenerateEmitterLogic(EmitterSettings settings, string className)
        {
            return EmitterLogicClassGenerator.Generate(settings,
                GlueState.Self.ProjectNamespace,
                className,
                false);
        }

        private void GluxLoaded()
        {
            // Search through all files for *.emitter
            // Generate code for each one for IEmitterLogic class
            // Create new ATI each one that (without emitter extension) and CanBeObject = true
            //     + runtime qualified type
            //     + custom load function

            var emitterFiles = ObjectFinder.Self
                .GetAllReferencedFiles()
                .Where(x => Path.GetExtension(x.Name).Equals(".emitter", StringComparison.OrdinalIgnoreCase))
                .ToArray();
            
            foreach (var emitterFile in emitterFiles)
            {
                var name = Path.GetFileNameWithoutExtension(emitterFile.Name) + "EmitterLogic";
                name = name.Replace("-", "")
                    .Replace(" ", "");
                
                var json = File.ReadAllText(GlueCommands.Self.GetAbsoluteFileName(emitterFile));
                
                var emitter = EmitterSettings.FromJson(json);
                if (!string.IsNullOrWhiteSpace(emitter.TextureFileName))
                {
                    var projectPath = GlueState.Self.CurrentGlueProjectDirectory;
                    var absolutePath = GlueCommands.Self.GetAbsoluteFileName(emitterFile);
                    var relativePath = FileManager.MakeRelative(absolutePath, projectPath);
                    var directory = Path.GetDirectoryName(relativePath);
                    emitter.TextureFileName = Path.Combine(directory, emitter.TextureFileName);
                }

                var code = GenerateEmitterLogic(emitter, name);
                var codeGenFilePath = new FilePath(Path.Combine(GlueState.Self.CurrentGlueProjectDirectory, "Particles", $"{name}.generated.cs"));
                GlueCommands.Self.ProjectCommands.CreateAndAddCodeFile(codeGenFilePath);
                File.WriteAllText(codeGenFilePath.FullPath, code);
                
                var ati = new AssetTypeInfo
                {
                    CanBeObject = true,
                    QualifiedRuntimeTypeName = new PlatformSpecificType
                    {
                        QualifiedType = "Parme.Frb.EmitterDrawableBatch",
                    },
                    FriendlyName = $"EmitterDrawableBatch ({name})",
                    ConstructorFunc = (element, save, arg3) => $"{save.FieldName} = new EmitterDrawableBatch(new {name}());",
                    AddToManagersFunc = (element, save, arg3, arg4) => $"FlatRedBall.SpriteManager.AddDrawableBatch({save.FieldName});",
                    VariableDefinitions =
                    {
                        new VariableDefinition
                        {
                            Name = "IsEmitting",
                            Type = "bool",
                            DefaultValue = "true",
                        }
                    }
                };
                
                AvailableAssetTypes.Self.AddAssetType(ati);
            }
        }
    }
}