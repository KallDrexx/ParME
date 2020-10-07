using System;
using System.Collections.Generic;
using System.Linq;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Triggers;
using Shouldly;
using Xunit;

namespace Parme.Core.Tests
{
    public class EmitterSettingsTests
    {
        [Fact]
        public void Can_Serialize_And_Deserialize_Simple_Emitter()
        {
            var trigger = new TimeElapsedTrigger {Frequency = 1.2f};
            var initializer = new StaticPositionInitializer {XOffset = 1.1f, YOffset = 2.2f};
            var modifier = new ConstantSizeModifier {WidthChangePerSecond = 10, HeightChangePerSecond = 11};
            var emitter = new EmitterSettings
            {
                Trigger = trigger,
                Initializers = new []{initializer},
                Modifiers = new[]{modifier},
                MaxParticleLifeTime = 5.5f
            };

            var json = emitter.ToJson();

            var deserializedEmitter = EmitterSettings.FromJson(json);
            deserializedEmitter.ShouldNotBeNull();
            deserializedEmitter.MaxParticleLifeTime.ShouldBe(5.5f);
            
            deserializedEmitter.Trigger.ShouldNotBeNull();
            deserializedEmitter.Trigger.ShouldBeOfType<TimeElapsedTrigger>();
            ((TimeElapsedTrigger) deserializedEmitter.Trigger).Frequency.ShouldBe(1.2f);
            
            deserializedEmitter.Initializers.ShouldNotBeNull();
            deserializedEmitter.Initializers.Count.ShouldBe(1);
            deserializedEmitter.Initializers[0].ShouldNotBeNull();
            deserializedEmitter.Initializers[0].ShouldBeOfType<StaticPositionInitializer>();
            ((StaticPositionInitializer) deserializedEmitter.Initializers[0]).XOffset.ShouldBe(1.1f);
            ((StaticPositionInitializer) deserializedEmitter.Initializers[0]).YOffset.ShouldBe(2.2f);
            
            deserializedEmitter.Modifiers.ShouldNotBeNull();
            deserializedEmitter.Modifiers.Count.ShouldBe(1);
            deserializedEmitter.Modifiers[0].ShouldNotBeNull();
            deserializedEmitter.Modifiers[0].ShouldBeOfType<ConstantSizeModifier>();
            ((ConstantSizeModifier) deserializedEmitter.Modifiers[0]).WidthChangePerSecond.ShouldBe(10);
            ((ConstantSizeModifier) deserializedEmitter.Modifiers[0]).HeightChangePerSecond.ShouldBe(11);
        }

        [Fact]
        public void Can_Serialize_And_Deserialize_Emitter_With_All_Initializers_And_Deserializers()
        {
            var triggerTypes = typeof(IParticleTrigger).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(IParticleTrigger).IsAssignableFrom(x))
                .ToHashSet();

            var initializerTypes = typeof(IParticleInitializer).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(IParticleInitializer).IsAssignableFrom(x))
                .ToHashSet();
            
            var modifierTypes = typeof(IParticleModifier).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(IParticleModifier).IsAssignableFrom(x)).ToHashSet();

            var trigger = (IParticleTrigger) Activator.CreateInstance(triggerTypes.First());
            var initializers = initializerTypes.Select(x => (IParticleInitializer) Activator.CreateInstance(x)).ToArray();
            var modifiers = modifierTypes.Select(x => (IParticleModifier) Activator.CreateInstance(x)).ToArray();
            
            var emitter = new EmitterSettings{
                Trigger = trigger, 
                Initializers = initializers, 
                Modifiers = modifiers, 
                MaxParticleLifeTime = 5
            };
            
            var json = emitter.ToJson();
            var deserializedEmitter = EmitterSettings.FromJson(json);
            
            deserializedEmitter.ShouldNotBeNull();
            deserializedEmitter.Trigger.ShouldBeOfType(trigger.GetType());
            deserializedEmitter.Initializers.ShouldNotBeNull();
            deserializedEmitter.Initializers.Count.ShouldBe(initializers.Length);
            deserializedEmitter.Modifiers.ShouldNotBeNull();
            deserializedEmitter.Modifiers.Count.ShouldBe(modifiers.Length);

            foreach (var deserializedInitializer in deserializedEmitter.Initializers)
            {
                initializerTypes.ShouldContain(deserializedInitializer.GetType());
            }
            
            foreach (var deserializedModifier in deserializedEmitter.Modifiers)
            {
                modifierTypes.ShouldContain(deserializedModifier.GetType());
            }
        }

        [Fact]
        public void Can_Deserialize_Basic_Fire_Effect_Json()
        {
            const string json = "{\n  \"MaxParticleLifeTime\": 1.0,\n  \"Trigger\": {\n    \"Frequency\": 0.01,\n    \"$ParmeType\": \"TimeElapsedTrigger\"\n  },\n  \"Initializers\": [\n    {\n      \"MinimumToSpawn\": 0,\n      \"MaximumToSpawn\": 5,\n      \"$ParmeType\": \"RandomParticleCountInitializer\"\n    },\n    {\n      \"Red\": 255,\n      \"Green\": 165,\n      \"Blue\": 0,\n      \"Alpha\": 1.0,\n      \"$ParmeType\": \"StaticColorInitializer\"\n    },\n    {\n      \"MinXVelocity\": 0.0,\n      \"MaxXVelocity\": 0.0,\n      \"MinYVelocity\": 2.0,\n      \"MaxYVelocity\": 5.0,\n      \"$ParmeType\": \"RandomRangeVelocityInitializer\"\n    },\n    {\n      \"MinXOffset\": -25.0,\n      \"MinYOffset\": -50.0,\n      \"MaxXOffset\": 25.0,\n      \"MaxYOffset\": -50.0,\n      \"$ParmeType\": \"RandomRegionPositionInitializer\"\n    },\n    {\n      \"Width\": 10,\n      \"Height\": 10,\n      \"$ParmeType\": \"StaticSizeInitializer\"\n    }\n  ],\n  \"Modifiers\": [\n    {\n      \"XAcceleration\": -5.0,\n      \"YAcceleration\": 5.0,\n      \"$ParmeType\": \"ConstantAccelerationModifier\"\n    },\n    {\n      \"WidthChangePerSecond\": -10.0,\n      \"HeightChangePerSecond\": -10.0,\n      \"$ParmeType\": \"ConstantSizeModifier\"\n    },\n    {\n      \"Red\": 3,\n      \"Green\": 2,\n      \"Blue\": 1,\n      \"Alpha\": 0.5,\n      \"$ParmeType\": \"EndingColorModifier\"\n    }\n  ]\n}";

            var emitter = EmitterSettings.FromJson(json);
            
            emitter.ShouldNotBeNull();
            emitter.MaxParticleLifeTime.ShouldBe(1.0f);
            emitter.Trigger.ShouldBeOfType<TimeElapsedTrigger>();
            ((TimeElapsedTrigger) emitter.Trigger).Frequency.ShouldBe(0.01f);

            emitter.Initializers.ShouldNotBeNull();
            emitter.Initializers.Count.ShouldBe(5);
            
            emitter.Initializers[0].ShouldBeOfType<RandomParticleCountInitializer>();
            ((RandomParticleCountInitializer) emitter.Initializers[0]).MinimumToSpawn.ShouldBe(0);
            ((RandomParticleCountInitializer) emitter.Initializers[0]).MaximumToSpawn.ShouldBe(5);
            
            emitter.Initializers[1].ShouldBeOfType<StaticColorInitializer>();
            ((StaticColorInitializer) emitter.Initializers[1]).Red.ShouldBe((byte) 255);
            ((StaticColorInitializer) emitter.Initializers[1]).Green.ShouldBe((byte) 165);
            ((StaticColorInitializer) emitter.Initializers[1]).Blue.ShouldBe((byte) 0);
            ((StaticColorInitializer) emitter.Initializers[1]).Alpha.ShouldBe(1.0f);
            
            emitter.Initializers[2].ShouldBeOfType<RandomRangeVelocityInitializer>();
            ((RandomRangeVelocityInitializer) emitter.Initializers[2]).MinXVelocity.ShouldBe(0.0f);
            ((RandomRangeVelocityInitializer) emitter.Initializers[2]).MaxXVelocity.ShouldBe(0.0f);
            ((RandomRangeVelocityInitializer) emitter.Initializers[2]).MinYVelocity.ShouldBe(2.0f);
            ((RandomRangeVelocityInitializer) emitter.Initializers[2]).MaxYVelocity.ShouldBe(5.0f);
            
            emitter.Initializers[3].ShouldBeOfType<RandomRegionPositionInitializer>();
            ((RandomRegionPositionInitializer) emitter.Initializers[3]).MinXOffset.ShouldBe(-25.0f);
            ((RandomRegionPositionInitializer) emitter.Initializers[3]).MinYOffset.ShouldBe(-50.0f);
            ((RandomRegionPositionInitializer) emitter.Initializers[3]).MaxXOffset.ShouldBe(25.0f);
            ((RandomRegionPositionInitializer) emitter.Initializers[3]).MaxYOffset.ShouldBe(-50.0f);
            
            emitter.Initializers[4].ShouldBeOfType<StaticSizeInitializer>();
            ((StaticSizeInitializer) emitter.Initializers[4]).Width.ShouldBe(10);
            ((StaticSizeInitializer) emitter.Initializers[4]).Height.ShouldBe(10);
            
            emitter.Modifiers.ShouldNotBeNull();
            emitter.Modifiers.Count.ShouldBe(3);
            
            emitter.Modifiers[0].ShouldBeOfType<ConstantAccelerationModifier>();
            ((ConstantAccelerationModifier) emitter.Modifiers[0]).XAcceleration.ShouldBe(-5);
            ((ConstantAccelerationModifier) emitter.Modifiers[0]).YAcceleration.ShouldBe(5);
            
            emitter.Modifiers[1].ShouldBeOfType<ConstantSizeModifier>();
            ((ConstantSizeModifier) emitter.Modifiers[1]).WidthChangePerSecond.ShouldBe(-10);
            ((ConstantSizeModifier) emitter.Modifiers[1]).HeightChangePerSecond.ShouldBe(-10);
            
            emitter.Modifiers[2].ShouldBeOfType<EndingColorModifier>();
            ((EndingColorModifier) emitter.Modifiers[2]).Red.ShouldBe((byte) 3);
            ((EndingColorModifier) emitter.Modifiers[2]).Green.ShouldBe((byte) 2);
            ((EndingColorModifier) emitter.Modifiers[2]).Blue.ShouldBe((byte) 1);
            ((EndingColorModifier) emitter.Modifiers[2]).Alpha.ShouldBe(0.5f);
        }

        [Fact]
        public void Texture_File_Name_Serialized_And_Deserialized()
        {
            var emitter = new EmitterSettings
            {
                Trigger = new OneShotTrigger(),
                Initializers = new IParticleInitializer[0],
                Modifiers = new IParticleModifier[0],
                MaxParticleLifeTime = 5.5f,
                TextureFileName = "..\\SomeFile.png",
            };
            
            var json = emitter.ToJson();
            var deserializedEmitter = EmitterSettings.FromJson(json);
            
            deserializedEmitter.ShouldNotBeNull();
            deserializedEmitter.TextureFileName.ShouldBe(emitter.TextureFileName);
        }

        [Fact]
        public void Texture_Sections_Serialized_And_Deserialized()
        {
            var emitter = new EmitterSettings
            {
                Trigger = new OneShotTrigger(),
                Initializers = new IParticleInitializer[0],
                Modifiers = new IParticleModifier[0],
                MaxParticleLifeTime = 5.5f,
                TextureSections = new[]
                {
                    new TextureSectionCoords(1, 2, 3, 4),
                    new TextureSectionCoords(5, 6, 7, 8),
                }
            };
            
            var json = emitter.ToJson();
            var deserializedEmitter = EmitterSettings.FromJson(json);
            
            deserializedEmitter.ShouldNotBeNull();
            deserializedEmitter.TextureSections.ShouldNotBeNull();
            deserializedEmitter.TextureSections.Count.ShouldBe(2);
            deserializedEmitter.TextureSections[0].ShouldBe(new TextureSectionCoords(1, 2, 3, 4));
            deserializedEmitter.TextureSections[1].ShouldBe(new TextureSectionCoords(5, 6, 7, 8));
        }
    }
}