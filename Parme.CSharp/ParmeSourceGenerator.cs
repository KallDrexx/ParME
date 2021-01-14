using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Parme.Core;
using Parme.CSharp.CodeGen;

namespace Parme.CSharp.SourceGenerators
{
    [Generator]
    public class ParmeSourceGenerator : ISourceGenerator
    {
        private const string EmitterNamespace = "Parme.GeneratedEmitters";
        private const string DiagnosticCategory = "ParmeSourceEmitter";
        
        private static readonly DiagnosticDescriptor DuplicateClassNames
            = new DiagnosticDescriptor(
                id: "PARME01",
                title: "Multiple emitters have conflicting class names",
                messageFormat: "Ignoring code gen for Parme emitter file '{0}', as its generated class name of '{2}' conflicts " +
                               "with the class name for the emitter file '{1}'.",
                category: DiagnosticCategory,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor EmitterParseFailure
            = new DiagnosticDescriptor(
                id: "PARME02",
                title: "Emitter logic file could not be parsed",
                messageFormat: "Could not parse emitter file '{0}': {1}",
                category: DiagnosticCategory,
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            //Debugger.Launch();

            var mappedEmitterFiles = new Dictionary<string, string>();
            
            foreach (var file in context.AdditionalFiles)
            {
                if (!Path.GetExtension(file.Path).Equals(".emlogic", StringComparison.Ordinal))
                {
                    continue;
                }

                var content = file.GetText(context.CancellationToken).ToString();
                var emitterName = FormEmitterClassName(file.Path);
                if (mappedEmitterFiles.TryGetValue(emitterName, out var prevEmitterPath))
                {
                    var diagnostic = Diagnostic.Create(DuplicateClassNames, Location.None, file.Path, prevEmitterPath, emitterName);
                    context.ReportDiagnostic(diagnostic);
                    continue;
                }
                
                mappedEmitterFiles.Add(emitterName, file.Path);

                EmitterSettings emitter;
                try
                {
                    emitter = EmitterSettings.FromJson(content);
                }
                catch (Exception exception)
                {
                    var diagnostic = Diagnostic.Create(EmitterParseFailure, Location.None, file.Path, exception.ToString());
                    context.ReportDiagnostic(diagnostic);
                    continue;
                }

                // Give the emitter a static namespace for now.  It would be nice to give the emitter a namespace
                // that resembles the file structure within the project, but it does not appear that roslyn gives
                // us that knowledge.  I can go through all additional files and predict the root by the most
                // common prefix, but that has the potential to break as files are added or removed.  This would
                // cause the namespace to be unpredictable and always changing.
                var code = EmitterLogicClassGenerator.Generate(emitter, EmitterNamespace, emitterName, false);
                
                context.AddSource(emitterName, code);
            }
        }

        private static string FormEmitterClassName(string filePath)
        {
            var name = Path.GetFileNameWithoutExtension(filePath)
                .Replace(" ", "")
                .Replace("-", "");
            
            if (char.IsDigit(name[0]))
            {
                name = "_" + name;
            }

            if (char.IsLower(name[0]))
            {
                name = char.ToUpper(name[0]) + name.Substring(1);
            }

            return name;
        }
    }
}