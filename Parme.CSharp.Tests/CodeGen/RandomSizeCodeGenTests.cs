using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Triggers;
using Parme.CSharp.CodeGen;
using Shouldly;
using Xunit;

namespace Parme.CSharp.Tests.CodeGen
{
    public class RandomSizeCodeGenTests
    {
        [Fact]
        public async Task Generates_Valid_Script_With_No_Size_Preservation()
        {
            var settings = new EmitterSettings
            {
                Trigger = new OneShotTrigger(),
                Initializers = new[]
                {
                    new RandomSizeInitializer
                    {
                        MinWidth = 10,
                        MaxWidth = 15,
                        MinHeight = 10,
                        MaxHeight = 15,
                        PreserveAspectRatio = false,
                        RandomizedAxis = RandomSizeInitializer.Axis.Y
                    },
                }
            };

            var code = EmitterLogicClassGenerator.Generate(settings, "ParmeTest", "ParmeClass", true);
            var scriptOptions = ScriptOptions.Default.WithReferences(typeof(IEmitterLogic).Assembly);

            var emitterLogic = await CSharpScript.EvaluateAsync<IEmitterLogic>(code, scriptOptions);

            emitterLogic.ShouldNotBeNull();
        }
        
        [Fact]
        public async Task Generates_Valid_Script_With_X_Axis_Size_Preservation()
        {
            var settings = new EmitterSettings
            {
                Trigger = new OneShotTrigger(),
                Initializers = new[]
                {
                    new RandomSizeInitializer
                    {
                        MinWidth = 10,
                        MaxWidth = 15,
                        MinHeight = 10,
                        MaxHeight = 15,
                        PreserveAspectRatio = true,
                        RandomizedAxis = RandomSizeInitializer.Axis.Y
                    },
                }
            };

            var code = EmitterLogicClassGenerator.Generate(settings, "ParmeTest", "ParmeClass", true);
            var scriptOptions = ScriptOptions.Default.WithReferences(typeof(IEmitterLogic).Assembly);

            var emitterLogic = await CSharpScript.EvaluateAsync<IEmitterLogic>(code, scriptOptions);

            emitterLogic.ShouldNotBeNull();
        }
        
        [Fact]
        public async Task Generates_Valid_Script_With_Y_Axis_Size_Preservation()
        {
            var settings = new EmitterSettings
            {
                Trigger = new OneShotTrigger(),
                Initializers = new[]
                {
                    new RandomSizeInitializer
                    {
                        MinWidth = 10,
                        MaxWidth = 15,
                        MinHeight = 10,
                        MaxHeight = 15,
                        PreserveAspectRatio = true,
                        RandomizedAxis = RandomSizeInitializer.Axis.X
                    },
                }
            };

            var code = EmitterLogicClassGenerator.Generate(settings, "ParmeTest", "ParmeClass", true);
            var scriptOptions = ScriptOptions.Default.WithReferences(typeof(IEmitterLogic).Assembly);

            var emitterLogic = await CSharpScript.EvaluateAsync<IEmitterLogic>(code, scriptOptions);

            emitterLogic.ShouldNotBeNull();
        }
    }
}