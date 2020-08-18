using System;
using System.Linq;
using System.Reflection;
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
    }
}