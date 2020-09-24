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
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;
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
            throw new NotImplementedException();
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
        
        private static EmitterSettings GetBasicFlameEmitterSettings()
        {
            var trigger = new TimeElapsedTrigger { Frequency = 0.01f };
            var initializers = new IParticleInitializer[]
            {
                new RandomParticleCountInitializer {MinimumToSpawn = 0, MaximumToSpawn = 5},
                new StaticColorInitializer
                {
                    // Orange
                    RedMultiplier = 1.0f,
                    GreenMultiplier = 165f / 255f,
                    BlueMultiplier = 0f,
                    AlphaMultiplier = 1f
                },

                new RandomRangeVelocityInitializer
                {
                    MinXVelocity = 0,
                    MaxXVelocity = 0,
                    MinYVelocity = 2,
                    MaxYVelocity = 5,
                },

                new RandomRegionPositionInitializer
                {
                    MinXOffset = -25,
                    MaxXOffset = 25,
                    MinYOffset = -50,
                    MaxYOffset = -50,
                },

                new StaticSizeInitializer
                {
                    Width = 100,
                    Height = 100,
                },
            };

            var modifiers = new IParticleModifier[]
            {
                new ConstantRotationModifier {DegreesPerSecond = 100f},
                new ConstantAccelerationModifier
                {
                    XAcceleration = -5,
                    YAcceleration = 5,
                },

                new ConstantSizeModifier
                {
                    WidthChangePerSecond = -100,
                    HeightChangePerSecond = -100,
                },

                new ConstantColorMultiplierChangeModifier
                {
                    RedMultiplierChangePerSecond = -1,
                    GreenMultiplierChangePerSecond = -1,
                    BlueMultiplierChangePerSecond = -1,
                    AlphaMultiplierChangePerSecond = -1,
                },
                
                new AnimatingTextureModifier(), 
            };

            return new EmitterSettings
            {
                Trigger = trigger,
                Initializers = initializers,
                Modifiers = modifiers,
                MaxParticleLifeTime = 1f,
                TextureFileName = "Content\\SampleParticles.png",
                TextureSections = new []
                {
                    new TextureSectionCoords(0, 64, 31, 96),
                    new TextureSectionCoords(32, 64, 63, 96),
                    new TextureSectionCoords(64, 64, 95, 96),
                    new TextureSectionCoords(0, 96, 31, 127),
                    new TextureSectionCoords(32, 96, 63, 127),
                    new TextureSectionCoords(64, 96, 95, 127),
                },
            };
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
                
                var json = File.ReadAllText(GlueCommands.Self.GetAbsoluteFileName(emitterFile));
                var emitter = EmitterSettings.FromJson(json);
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
                };
                
                AvailableAssetTypes.Self.AddAssetType(ati);
            }

            //var emitter = GetBasicFlameEmitterSettings();
            //var code = GenerateEmitterLogic(emitter, "FireExample");
            //var codeGenFilePath = new FilePath(Path.Combine(GlueState.Self.CurrentGlueProjectDirectory, "Particles", "FireExample.generated.cs"));
            //GlueCommands.Self.ProjectCommands.CreateAndAddCodeFile(codeGenFilePath);

            //File.WriteAllText(codeGenFilePath.FullPath, code);
        }
    }
}