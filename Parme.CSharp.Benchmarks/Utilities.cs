using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Parme.Core;
using Parme.CSharp.CodeGen;

namespace Parme.CSharp.Benchmarks
{
    public static class Utilities
    {
        public static IEmitterLogic GetInstance(EmitterSettings settings)
        {
            var code = EmitterLogicClassGenerator.Generate(settings, "Parme.Editor", "Test", true);
            
            var scriptOptions = ScriptOptions.Default
                .WithReferences(typeof(IEmitterLogic).Assembly);
                
            return CSharpScript.EvaluateAsync<IEmitterLogic>(code, scriptOptions).GetAwaiter().GetResult();
        }
    }
}