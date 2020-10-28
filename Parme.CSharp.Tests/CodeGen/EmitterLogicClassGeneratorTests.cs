using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;
using Parme.CSharp.CodeGen;
using Shouldly;
using Xunit;

namespace Parme.CSharp.Tests.CodeGen
{
    public class EmitterLogicClassGeneratorTests
    {
        [Fact]
        public async Task Can_Generate_Valid_Script_Code_That_Returns_IEmitterLogic_Class_With_All_Particle_Modifiers_And_Initializers()
        {
            var allTriggers = typeof(IParticleTrigger).Assembly
                .GetTypes()
                .Where(x => !x.IsInterface)
                .Where(x => !x.IsAbstract)
                .Where(x => typeof(IParticleTrigger).IsAssignableFrom(x))
                .Select(x => (IParticleTrigger) Activator.CreateInstance(x))
                .ToList();
            
            var allInitializers = typeof(IParticleInitializer).Assembly
                .GetTypes()
                .Where(x => !x.IsInterface)
                .Where(x => !x.IsAbstract)
                .Where(x => typeof(IParticleInitializer).IsAssignableFrom(x))
                .Select(x => (IParticleInitializer) Activator.CreateInstance(x))
                .ToList();
            
            var allModifiers = typeof(IParticleModifier).Assembly
                .GetTypes()
                .Where(x => !x.IsInterface)
                .Where(x => !x.IsAbstract)
                .Where(x => typeof(IParticleModifier).IsAssignableFrom(x))
                .Select(x => (IParticleModifier) Activator.CreateInstance(x))
                .ToList();

            var settings = new EmitterSettings
            {
                Trigger = allTriggers.First(),
                Initializers = allInitializers,
                Modifiers = allModifiers,
                MaxParticleLifeTime = 1f
            };
            
            var code = EmitterLogicClassGenerator.Generate(settings, "ParmeTest", "ParmeClass", true);
            
            var scriptOptions = ScriptOptions.Default
                .WithReferences(typeof(IEmitterLogic).Assembly);

            var emitterLogic = await CSharpScript.EvaluateAsync<IEmitterLogic>(code, scriptOptions);
            
            emitterLogic.ShouldNotBeNull();
        }

        [Fact]
        public async Task Logic_Class_Contains_Accessible_Texture_FileName()
        {
            var settings = new EmitterSettings
            {
                Trigger = new OneShotTrigger(),
                TextureFileName = @"C:\test\some.png",
            };
            
            var code = EmitterLogicClassGenerator.Generate(settings, "ParmeTest", "ParmeClass", true);
            var scriptOptions = ScriptOptions.Default.WithReferences(typeof(IEmitterLogic).Assembly);

            var emitterLogic = await CSharpScript.EvaluateAsync<IEmitterLogic>(code, scriptOptions);
            
            emitterLogic.ShouldNotBeNull();
            emitterLogic.TextureFilePath.ShouldBe(settings.TextureFileName);
        }

        [Fact]
        public async Task Logic_Class_Contains_Accessible_Texture_Sections()
        {
            var settings = new EmitterSettings
            {
                Trigger = new OneShotTrigger(),
                TextureFileName = @"C:\test\some.png",
                TextureSections = new[]
                {
                    new TextureSectionCoords(1, 2, 3, 4),
                    new TextureSectionCoords(5, 6, 7, 8),
                }
            };
            
            var code = EmitterLogicClassGenerator.Generate(settings, "ParmeTest", "ParmeClass", true);
            var scriptOptions = ScriptOptions.Default.WithReferences(typeof(IEmitterLogic).Assembly);

            var emitterLogic = await CSharpScript.EvaluateAsync<IEmitterLogic>(code, scriptOptions);
            
            emitterLogic.ShouldNotBeNull();
            emitterLogic.TextureSections.ShouldNotBeNull();
            emitterLogic.TextureSections.Length.ShouldBe(2);
            emitterLogic.TextureSections[0].ShouldBe(new TextureSectionCoords(1, 2, 3, 4));
            emitterLogic.TextureSections[1].ShouldBe(new TextureSectionCoords(5, 6, 7, 8));
        }

        [Fact]
        public async Task Class_Generation_Works_With_Comma_Separated_Decimal_Cultures()
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");

            try
            {
                var emitter = new EmitterSettings
                {
                    MaxParticleLifeTime = 1.1f,
                    Trigger = new OneShotTrigger(),
                    Initializers = new IParticleInitializer[]
                    {
                        new RadialVelocityInitializer {MinDegrees = 1, MaxDegrees = 360, Magnitude = 1.23f},
                        new StaticColorInitializer {Alpha = 0.76f, Blue = 200, Green = 199, Red = 198},
                        new RandomRangeVelocityInitializer
                            {MaxXVelocity = 123456.1f, MaxYVelocity = 1.2f, MinXVelocity = 1.3f, MinYVelocity = 1.4f},
                        new StaticRotationalVelocityInitializer {DegreesPerSecond = 256},
                        new RandomRotationalVelocityInitializer
                            {MinRotationSpeedInDegreesPerSecond = 111, MaxRotationSpeedInDegreesPerSecond = 123},
                    },
                    Modifiers = new IParticleModifier[]
                    {
                        new DragModifier {DragFactor = 1.2f},
                        new ConstantAccelerationModifier {XAcceleration = 1.1f, YAcceleration = 1.2f},
                        new EndingColorModifier {Alpha = 0.76f, Blue = 200, Green = 199, Red = 198},
                    }
                };

                var code = EmitterLogicClassGenerator.Generate(emitter, "ParmeTest", "ParmeClass", true);

                var scriptOptions = ScriptOptions.Default
                    .WithReferences(typeof(IEmitterLogic).Assembly);

                var emitterLogic = await CSharpScript.EvaluateAsync<IEmitterLogic>(code, scriptOptions);

                emitterLogic.ShouldNotBeNull();
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }
    }
}