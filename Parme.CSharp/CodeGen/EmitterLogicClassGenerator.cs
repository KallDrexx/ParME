using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parme.Core.Initializers;

namespace Parme.CSharp.CodeGen
{
    public static class EmitterLogicClassGenerator
    {
        private static readonly IReadOnlyDictionary<Type, IGenerateCode> CodeGenerators =
            typeof(EmitterLogicClassGenerator).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(IGenerateCode).IsAssignableFrom(x))
                .Select(x => (IGenerateCode) Activator.CreateInstance(x))
                .ToDictionary(x => x.ParmeObjectType, x => x);
        
        private const string Template = @"
using System;
using System.Numerics;
using Parme.CSharp;

{0}
    public class {1} : IEmitterLogic
    {{
        private readonly Random _random = new Random();
        {2}

        public int MaxParticleLifeTime {{ get; set; }} = {3};
        
        {4}
        
        public void Update(ParticleBuffer particleBuffer, float timeSinceLastFrame, Emitter parent)
        {{
            var emitterCoordinates = parent.WorldCoordinates;

            // Update existing particles
            var particles = particleBuffer.Particles;
            for (var particleIndex = 0; particleIndex < particles.Length; particleIndex++)
            {{
                ref var particle = ref particles[particleIndex];
                if (!particle.IsAlive)
                {{
                    continue;
                }}
                
                particle.TimeAlive += timeSinceLastFrame;
                if (particle.TimeAlive > MaxParticleLifeTime)
                {{
                    particle.IsAlive = false;
                    continue;
                }}
                
                // modifiers
                {5}
                
                particle.Position += particle.Velocity;
            }}
            
            var shouldCreateNewParticle = false;
            {{
                {6}
            }}
            
            if (shouldCreateNewParticle && parent.IsEmittingNewParticles)
            {{
                var newParticleCount = 0;
                {{
                    {7}
                }}
                
                for (var newParticleIndex = 0; newParticleIndex < newParticleCount; newParticleIndex++)
                {{
                    var particle = new Particle
                    {{
                        IsAlive = true,
                        TimeAlive = 0,
                        RotationInRadians = 0, // TODO: add initializer
                    }};
                    
                    // Initializers
                    {8}

                    // Adjust the particle's position by the emitter's location
                    particle.Position += emitterCoordinates;
                    
                    particleBuffer.Add(particle);            
                }}
            }}
        }}
    }}
{9}
";

        public static string Generate(EmitterSettings settings, 
            string namespaceName, 
            string className,
            bool generateScriptCode)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var fieldDefinitions = GetFieldDefinitions(settings);
            var properties = GetProperties(settings);
            var modifiers = GetModifierCode(settings);
            var initializers = GetInitializerCode(settings);
            var particleCountCode = GetParticleCountCode(settings);

            var triggerGenerator = GetCodeGenerator(settings.Trigger.GetType());
            
            return string.Format(Template,
                generateScriptCode ? string.Empty : $"namespace {namespaceName}{Environment.NewLine}{{{Environment.NewLine}",
                className,
                fieldDefinitions,
                settings.MaxParticleLifeTime,
                properties,
                modifiers,
                triggerGenerator.GenerateExecutionCode(settings.Trigger),
                particleCountCode,
                initializers,
                generateScriptCode 
                    ? $"return new {className}();"
                    : "}"
            );
        }

        private static string GetFieldDefinitions(EmitterSettings settings)
        {
            var fieldDefinitions = new StringBuilder();

            var triggerCodeGenerator = GetCodeGenerator(settings.Trigger.GetType());
            fieldDefinitions.Append(triggerCodeGenerator.GenerateFields(settings.Trigger));

            foreach (var initializer in settings.Initializers)
            {
                var codeGenerator = GetCodeGenerator(initializer.GetType());
                fieldDefinitions.Append(codeGenerator.GenerateFields(initializer));
            }

            foreach (var modifier in settings.Modifiers)
            {
                var codeGenerator = GetCodeGenerator(modifier.GetType());
                fieldDefinitions.Append(codeGenerator.GenerateFields(modifier));
            }

            return fieldDefinitions.ToString();
        }
        
        private static string GetProperties(EmitterSettings settings)
        {
            var properties = new StringBuilder();

            var triggerCodeGenerator = GetCodeGenerator(settings.Trigger.GetType());
            properties.Append(triggerCodeGenerator.GenerateProperties(settings.Trigger));

            foreach (var initializer in settings.Initializers)
            {
                var codeGenerator = GetCodeGenerator(initializer.GetType());
                properties.Append(codeGenerator.GenerateProperties(initializer));
            }

            foreach (var modifier in settings.Modifiers)
            {
                var codeGenerator = GetCodeGenerator(modifier.GetType());
                properties.Append(codeGenerator.GenerateProperties(modifier));
            }

            return properties.ToString();
        }

        private static string GetModifierCode(EmitterSettings settings)
        {
            var modifierCode = new StringBuilder();
            modifierCode.AppendLine();
            
            foreach (var modifier in settings.Modifiers)
            {
                var codeGenerator = GetCodeGenerator(modifier.GetType());

                modifierCode.AppendLine("                {");
                modifierCode.Append("                        ");
                modifierCode.Append(codeGenerator.GenerateExecutionCode(modifier));
                modifierCode.AppendLine("                }");
            }

            return modifierCode.ToString();
        }

        private static string GetInitializerCode(EmitterSettings settings)
        {
            var initializerCode = new StringBuilder();
            initializerCode.AppendLine();

            foreach (var initializer in settings.Initializers
                .Where(x => x.InitializerType != InitializerType.ParticleCount))
            {
                var codeGenerator = GetCodeGenerator(initializer.GetType());

                initializerCode.AppendLine("                    {");
                initializerCode.Append("                        ");
                initializerCode.Append(codeGenerator.GenerateExecutionCode(initializer));
                initializerCode.AppendLine("                    }");
            }

            return initializerCode.ToString();
        }

        private static string GetParticleCountCode(EmitterSettings settings)
        {
            var initializerCode = new StringBuilder();
            initializerCode.AppendLine();

            foreach (var initializer in settings.Initializers
                .Where(x => x.InitializerType == InitializerType.ParticleCount))
            {
                var codeGenerator = GetCodeGenerator(initializer.GetType());

                initializerCode.AppendLine("                    {");
                initializerCode.Append("                        ");
                initializerCode.Append(codeGenerator.GenerateExecutionCode(initializer));
                initializerCode.AppendLine("                    }");
            }

            return initializerCode.ToString();
        }

        private static IGenerateCode GetCodeGenerator(Type type)
        {
            if (!CodeGenerators.TryGetValue(type, out var codeGenerator))
            {
                var message = $"No code generator exists for type {type.FullName}";
                throw new InvalidOperationException(message);
            }

            return codeGenerator;
        }
    }
}