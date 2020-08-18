using System;
using System.ComponentModel.Composition;
using System.IO;
using FlatRedBall.Glue.Elements;
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
            
            foreach (var assetTypeInfo in new AssetTypeManager().GetDrawableBatchAssetTypes())
            {
                AvailableAssetTypes.Self.AddAssetType(assetTypeInfo);
            }
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
                    new TextureSectionCoords(16, 0, 31, 15),
                    new TextureSectionCoords(48, 16, 63, 31), 
                },
            };
        }

        private void GluxLoaded()
        {
            var emitter = GetBasicFlameEmitterSettings();
            var code = GenerateEmitterLogic(emitter, "FireExample");
            var codeGenFilePath = new FilePath(Path.Combine(GlueState.Self.CurrentGlueProjectDirectory, "Particles", "FireExample.generated.cs"));
            GlueCommands.Self.ProjectCommands.CreateAndAddCodeFile(codeGenFilePath);
            
            File.WriteAllText(codeGenFilePath.FullPath, code);
        }
    }
}