using System;
using System.Linq;
using Parme.Core.Initializers;
using Parme.Core.Modifiers;
using Parme.Core.Serialization;
using Parme.Core.Triggers;
using Shouldly;
using Xunit;

namespace Parme.Core.Tests.Serialization
{
    public class SerializedTypeNameMapTests
    {
        [Fact]
        public void Exception_When_Adding_Interface()
        {
            var map = new SerializedTypeNameMap();

            Assert.ThrowsAny<Exception>(() => map.AddType(typeof(IParticleModifier)));
        }

        [Fact]
        public void Can_Refer_To_Added_Type_By_CSharp_Short_Type_Name()
        {
            var map = new SerializedTypeNameMap();
            map.AddType(typeof(OneShotTrigger));
            map.AddType(typeof(TimeElapsedTrigger));
            
            map.Map.ContainsKey(nameof(OneShotTrigger)).ShouldBeTrue();
            map.Map[nameof(OneShotTrigger)].ShouldBe(typeof(OneShotTrigger));
            
            map.Map.ContainsKey(nameof(TimeElapsedTrigger)).ShouldBeTrue();
            map.Map[nameof(TimeElapsedTrigger)].ShouldBe(typeof(TimeElapsedTrigger));
        }

        [Fact]
        public void Map_Is_Case_Insensitive()
        {
            var map = new SerializedTypeNameMap();
            map.AddType(typeof(OneShotTrigger));
            
            map.Map.ContainsKey(nameof(OneShotTrigger).ToLower()).ShouldBeTrue();
            map.Map[nameof(OneShotTrigger).ToLower()].ShouldBe(typeof(OneShotTrigger));
        }

        [Fact]
        public void Can_Refer_To_Added_Type_By_Serialized_Type_Name_Value()
        {
            var map = new SerializedTypeNameMap();
            map.AddType(typeof(OneShotTrigger));
            map.AddType(typeof(TestType));
            map.AddType(typeof(TimeElapsedTrigger));
            
            map.Map.ContainsKey("First").ShouldBeTrue();
            map.Map["First"].ShouldBe(typeof(TestType));
            
            map.Map.ContainsKey("Second").ShouldBeTrue();
            map.Map["Second"].ShouldBe(typeof(TestType));
        }

        [Fact]
        public void Same_Type_Can_Be_Added_Multiple_Times_Without_Error()
        {
            var map = new SerializedTypeNameMap();
            map.AddType(typeof(OneShotTrigger));
            map.AddType(typeof(OneShotTrigger));
            map.AddType(typeof(OneShotTrigger));
        }

        [Fact]
        public void Exception_If_Different_Type_Added_With_Conflicting_Alias()
        {
            var map = new SerializedTypeNameMap();
            map.AddType(typeof(TestType));

            Assert.ThrowsAny<Exception>(() => map.AddType(typeof(ConflictingTestType)));
        }

        [Fact]
        public void All_Triggers_Modifiers_And_Initializers_Can_Be_Added_Without_Conflicts()
        {
            var triggers = typeof(IParticleTrigger).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(IParticleTrigger).IsAssignableFrom(x));
            
            var modifiers = typeof(IParticleModifier).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(IParticleModifier).IsAssignableFrom(x));
            
            var initializers = typeof(IParticleInitializer).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => typeof(IParticleInitializer).IsAssignableFrom(x));

            var allTypes = triggers.Union(modifiers).Union(initializers);
            
            var map = new SerializedTypeNameMap();
            foreach (var type in allTypes)
            {
                map.AddType(type);
            }
        }

        [SerializedTypeName("First")]
        [SerializedTypeName("Second")]
        private class TestType { }
        
        [SerializedTypeName("First")]
        private class ConflictingTestType { }
    }
}