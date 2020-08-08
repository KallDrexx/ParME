using System;
using System.Text;

namespace Parme.Scripting
{
    public static class CSharpEmitterLogicGenerator
    {
        private const string Template = @"
using System;
using Microsoft.Xna.Framework;
using Parme;

namespace {0}
{{
    public class {1} : IEmitterLogic
    {{
        private readonly Random _random = new Random();
        {2}
        
        public void Update(ParticleBuffer particleBuffer, float timeSinceLastFrame)
        {{
            // Update existing particles
            for (var particleIndex = 0; particleIndex < particleBuffer.Count; particleIndex++)
            {{
                ref var particle = ref particleBuffer[particleIndex];
                if (!particle.IsAlive)
                {{
                    continue;
                }}
                
                particle.TimeAlive += timeSinceLastFrame;
                if (particle.TimeAlive > {3})
                {{
                    particle.IsAlive = false;
                    continue;
                }}
                
                // modifiers
                {4}
                
                particle.Position += particle.Velocity;
            }}
            
            var shouldCreateNewParticle = false;
            {5}
            
            if (shouldCreateNewParticle)
            {{
                var newParticleCount = 0;
                {6}
                
                for (var newParticleIndex = 0; newParticleIndex < newParticleCount; newParticleIndex++)
                {{
                    var particle = new Particle
                    {{
                        IsAlive = true,
                        TimeAlive = 0,
                        RotationInRadians = 0, // TODO: add initializer
                    }};
                    
                    // position
                    {{
                        {7}
                    }}
                    
                    // velocity
                    {{
                        {8}
                    }}
                    
                    // size
                    {{
                        {9}
                    }}
                    
                    // color
                    {{
                        {10}
                    }}
                    
                    particleBuffer.Add(particle);            
                }}
            }}
        }}
    }}
}}

return new {1}();
            ";
        
        public static string Generate(EmitterSettings settings, string namespaceName, string className)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            var triggerFieldDefinitions = settings.Trigger?.GetCSharpFieldDefinitions();
            var triggerCheckCode = settings.Trigger?.GetCSharpCheckCode();
            var newParticleCountCode = settings.ParticleCountInitializer?.GetCSharpExecutionCode();
            var positionInitializerCode = settings.PositionalInitializer?.GetCSharpExecutionCode();
            var velocityInitializerCode = settings.VelocityInitializer?.GetCSharpExecutionCode();
            var sizeInitializerCode = settings.SizeInitializer?.GetCSharpExecutionCode();
            var colorInitializerCode = settings.ColorInitializer?.GetCSharpExecutionCode();
            
            var modifierCode = new StringBuilder();
            foreach (var modifier in settings.Modifiers)
            {
                modifierCode.AppendLine();
                modifierCode.AppendLine("                {");
                modifierCode.Append(modifier.GetCSharpExecutionCode());
                modifierCode.AppendLine("                }");
            }

            return string.Format(Template,
                namespaceName,
                className,
                triggerFieldDefinitions,
                settings.MaxParticleLifeTime,
                modifierCode,
                triggerCheckCode,
                newParticleCountCode,
                positionInitializerCode,
                velocityInitializerCode,
                sizeInitializerCode,
                colorInitializerCode);
        }
    }
}