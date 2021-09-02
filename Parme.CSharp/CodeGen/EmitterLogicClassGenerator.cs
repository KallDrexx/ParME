using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parme.Core;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;

namespace Parme.CSharp.CodeGen
{
    public static class EmitterLogicClassGenerator
    {
        private static readonly IReadOnlyDictionary<Type, ParticleCodeGenerator> CodeGenerators =
            typeof(EmitterLogicClassGenerator).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(ParticleCodeGenerator).IsAssignableFrom(x))
                .Select(x => (ParticleCodeGenerator) Activator.CreateInstance(x))
                .ToDictionary(x => x.ParmeObjectType, x => x);
        
        private const string Template = @"
using System;
using System.Numerics;
using Parme.Core;
using Parme.CSharp;

{0}
    public class {1} : IEmitterLogic
    {{
        {2}

        public float MaxParticleLifeTime {{ get; set; }} = {3}f;
        
        public string TextureFilePath {{ get; }} = {10};
        {11}        
        
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
                
                // position modifier
                {13}

                particle.RotationInRadians += particle.RotationalVelocityInRadians * timeSinceLastFrame;
            }}
            
            var shouldCreateNewParticle = false;
            var stopEmittingAfterUpdate = false;
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
                        RotationInRadians = 0,
                        Position = Vector2.Zero,
                        ReferencePosition = Vector2.Zero,
                        RotationalVelocityInRadians = 0f,
                        CurrentRed = 255,
                        CurrentGreen = 255,
                        CurrentBlue = 255,
                        CurrentAlpha = 255,
                        Size = Vector2.Zero,
                        InitialSize = Vector2.Zero,
                        Velocity = Vector2.Zero,
                    }};
                    
                    // Initializers
                    {8}

                    // Set the initial values to their current equivalents
                    particle.InitialRed = (byte) particle.CurrentRed;
                    particle.InitialGreen = (byte) particle.CurrentGreen;
                    particle.InitialBlue = (byte) particle.CurrentBlue;
                    particle.InitialAlpha = (byte) particle.CurrentAlpha;
                    particle.InitialSize = particle.Size;

                    // Adjust the particle's rotation, position, and velocity by the emitter's rotation
                    RotateVector(ref particle.Position, parent.RotationInRadians);
                    RotateVector(ref particle.Velocity, parent.RotationInRadians);
                    particle.RotationInRadians += parent.RotationInRadians;

                    // Adjust the particle's position by the emitter's location
                    particle.Position += emitterCoordinates;
                    particle.ReferencePosition = particle.Position;
                    
                    particleBuffer.Add(particle);            
                }}
            }}

            if (stopEmittingAfterUpdate)
            {{
                parent.IsEmittingNewParticles = false;
                stopEmittingAfterUpdate = false;
            }}
        }}

        public int GetEstimatedCapacity()
        {{
            var particlesPerTrigger = 0f;
            var triggersPerSecond = 0f;

{12}
            
            return (int) Math.Ceiling(particlesPerTrigger * triggersPerSecond * MaxParticleLifeTime);
        }}

        private static void RotateVector(ref Vector2 vector, float radians)
        {{
            if (vector == Vector2.Zero)
            {{
                return;
            }}
            
            var magnitude = vector.Length();
            var angle = Math.Atan2(vector.Y, vector.X);
            angle += radians;
            
            vector.X = magnitude * (float) Math.Cos(angle);
            vector.Y = magnitude * (float) Math.Sin(angle);
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
            
            settings.Initializers ??= new IParticleInitializer[0];
            settings.Modifiers ??= new IParticleModifier[0];
            settings.TextureSections ??= new TextureSectionCoords[0];

            var fieldDefinitions = GetFieldDefinitions(settings);
            var properties = GetProperties(settings);
            var modifiers = GetModifierCode(settings);
            var initializers = GetInitializerCode(settings);
            var particleCountCode = GetParticleCountCode(settings);
            var textureSectionCode = GetTextureCoordinateMapCode(settings);
            var estimationCode = GetEstimationCode(settings);
            var triggerGenerator = GetCodeGenerator(settings.Trigger?.GetType());
            var positionModifier = GetPositionModifierCode(settings);
            
            return string.Format(Template,
                generateScriptCode ? string.Empty : $"namespace {namespaceName}{Environment.NewLine}{{{Environment.NewLine}",
                className,
                fieldDefinitions,
                ToInvariant($"{settings.MaxParticleLifeTime}"),
                properties,
                modifiers,
                ToInvariant(triggerGenerator?.GenerateExecutionCode(settings.Trigger)),
                particleCountCode,
                initializers,
                generateScriptCode 
                    ? $"return new {className}();"
                    : "}",
                $"@\"{settings.TextureFileName}\"",
                textureSectionCode,
                estimationCode,
                positionModifier
            );
        }

        private static string GetFieldDefinitions(EmitterSettings settings)
        {
            var fieldDefinitions = new StringBuilder();

            if (settings.Trigger != null)
            {
                var triggerCodeGenerator = GetCodeGenerator(settings.Trigger.GetType());
                fieldDefinitions.Append(ToInvariant(triggerCodeGenerator.GenerateFields(settings.Trigger)));
            }

            foreach (var initializer in settings.Initializers.Where(x => x != null))
            {
                var codeGenerator = GetCodeGenerator(initializer.GetType());
                fieldDefinitions.Append(ToInvariant(codeGenerator.GenerateFields(initializer)));
            }

            foreach (var modifier in settings.Modifiers.Where(x => x != null))
            {
                var codeGenerator = GetCodeGenerator(modifier.GetType());
                fieldDefinitions.Append(ToInvariant(codeGenerator.GenerateFields(modifier)));
            }

            if (settings.PositionModifier != null)
            {
                var codeGenerator = GetCodeGenerator(settings.PositionModifier.GetType());
                fieldDefinitions.Append(ToInvariant(codeGenerator.GenerateFields(settings.PositionModifier)));
            }

            return fieldDefinitions.ToString();
        }
        
        private static string GetProperties(EmitterSettings settings)
        {
            var properties = new StringBuilder();

            if (settings.Trigger != null)
            {
                var triggerCodeGenerator = GetCodeGenerator(settings.Trigger.GetType());
                properties.Append(ToInvariant(triggerCodeGenerator.GenerateProperties(settings.Trigger)));
            }

            foreach (var initializer in settings.Initializers.Where(x => x != null))
            {
                var codeGenerator = GetCodeGenerator(initializer.GetType());
                properties.Append(ToInvariant(codeGenerator.GenerateProperties(initializer)));
            }

            foreach (var modifier in settings.Modifiers.Where(x => x != null))
            {
                var codeGenerator = GetCodeGenerator(modifier.GetType());
                properties.Append(ToInvariant(codeGenerator.GenerateProperties(modifier)));
            }
            
            if (settings.PositionModifier != null)
            {
                var codeGenerator = GetCodeGenerator(settings.PositionModifier.GetType());
                properties.Append(ToInvariant(codeGenerator.GenerateProperties(settings.PositionModifier)));
            }

            return properties.ToString();
        }

        private static string GetModifierCode(EmitterSettings settings)
        {
            var modifierCode = new StringBuilder();
            modifierCode.AppendLine();
            
            foreach (var modifier in settings.Modifiers.Where(x => x != null))
            {
                var codeGenerator = GetCodeGenerator(modifier.GetType());

                modifierCode.AppendLine("                {");
                modifierCode.Append("                        ");
                modifierCode.Append(ToInvariant(codeGenerator.GenerateExecutionCode(modifier)));
                modifierCode.AppendLine("                }");
            }
            
            if (settings.PositionModifier != null)
            {
                var codeGenerator = GetCodeGenerator(settings.PositionModifier.GetType());
                
                modifierCode.AppendLine("                {");
                modifierCode.Append("                        ");
                modifierCode.Append(ToInvariant(codeGenerator.GenerateExecutionCode(settings.PositionModifier)));
                modifierCode.AppendLine("                }");
            }

            return modifierCode.ToString();
        }

        private static string GetInitializerCode(EmitterSettings settings)
        {
            var initializerCode = new StringBuilder();
            initializerCode.AppendLine();

            // We want to ensure texture section initializer is first, in case any other initializers are
            // dependent on the selected texture (i.e. texture based sizing)
            foreach (var initializer in settings.Initializers
                .Where(x => x != null)
                .Where(x => x.InitializerType != InitializerType.ParticleCount)
                .OrderByDescending(x => x.InitializerType == InitializerType.TextureSectionIndex))
            {
                var codeGenerator = GetCodeGenerator(initializer.GetType());

                initializerCode.AppendLine("                    {");
                initializerCode.Append("                        ");
                initializerCode.Append(ToInvariant(codeGenerator.GenerateExecutionCode(initializer)));
                initializerCode.AppendLine("                    }");
            }

            return initializerCode.ToString();
        }

        private static string GetParticleCountCode(EmitterSettings settings)
        {
            var initializerCode = new StringBuilder();
            initializerCode.AppendLine();

            foreach (var initializer in settings.Initializers
                .Where(x => x != null)
                .Where(x => x.InitializerType == InitializerType.ParticleCount))
            {
                var codeGenerator = GetCodeGenerator(initializer.GetType());

                initializerCode.AppendLine("                    {");
                initializerCode.Append("                        ");
                initializerCode.Append(ToInvariant(codeGenerator.GenerateExecutionCode(initializer)));
                initializerCode.AppendLine("                    }");
            }

            return initializerCode.ToString();
        }

        private static string GetTextureCoordinateMapCode(EmitterSettings settings)
        {
            var code = new StringBuilder();
            code.AppendLine("public TextureSectionCoords[] TextureSections { get; } = new TextureSectionCoords[] {");

            foreach (var section in settings.TextureSections ?? new List<TextureSectionCoords>())
            {
                code.AppendLine($"            new TextureSectionCoords({section.LeftX}, {section.TopY}, {section.RightX}, {section.BottomY}),");
            }
            
            code.AppendLine("        };");

            return code.ToString();
        }

        private static string GetEstimationCode(EmitterSettings settings)
        {
            var code = new StringBuilder();
            
            if (settings.Trigger != null)
            {
                var triggerCode = ToInvariant(GetCodeGenerator(settings.Trigger.GetType())
                    .GenerateCapacityEstimationCode(settings.Trigger));

                if (!string.IsNullOrWhiteSpace(triggerCode))
                {
                    code.AppendLine("            {");
                    code.AppendLine($"                {triggerCode}");
                    code.AppendLine("            }");
                }
            }

            foreach (var initializer in settings.Initializers.Where(x => x != null))
            {
                var generatedCode = ToInvariant(GetCodeGenerator(initializer.GetType())
                    .GenerateCapacityEstimationCode(initializer));

                if (!string.IsNullOrWhiteSpace(generatedCode))
                {
                    code.AppendLine("            {");
                    code.AppendLine($"                {generatedCode}");
                    code.AppendLine("            }");
                }
            }

            return code.ToString();
        }

        private static string GetPositionModifierCode(EmitterSettings settings)
        {
            if (settings.PositionModifier == null)
            {
                return "particle.Position += particle.Velocity * timeSinceLastFrame;";
            }

            var codeGenerator = GetCodeGenerator(settings.PositionModifier.GetType());
            var code = codeGenerator.GeneratePositionExecutionCode(settings.PositionModifier);

            return ToInvariant(code);
        }

        private static ParticleCodeGenerator GetCodeGenerator(Type type)
        {
            if (type == null)
            {
                return null;
            }
            
            if (!CodeGenerators.TryGetValue(type, out var codeGenerator))
            {
                var message = $"No code generator exists for type {type.FullName}";
                throw new InvalidOperationException(message);
            }

            return codeGenerator;
        }

        private static string ToInvariant(FormattableString formattableString)
        {
            return FormattableString.Invariant(formattableString ?? $"");
        }
    }
}