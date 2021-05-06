using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parme.Frb.GluePlugin
{
    public class EmitterLogicMapperGenerator
    {
        private readonly List<string> _emitterLogicTypes = new();
        
        public void AddEmitterLogicTypeName(string name)
        {
            if (_emitterLogicTypes.All(x => !x.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                _emitterLogicTypes.Add(name);
            }
            
            _emitterLogicTypes.Sort();
        }

        public void RemoveEmitterLogicTypeName(string name)
        {
            _emitterLogicTypes.RemoveAll(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void ClearEmitterLogicTypes()
        {
            _emitterLogicTypes.Clear();
        }

        public string GenerateMapperImplementation(string projectNamespace)
        {
            const string template = @"
using System;
using System.Collections.Generic;
using Parme.Frb;

namespace {0}.Particles
{{
    public class ParmeEmitterLogicGenerator : IEmitterLogicMapper
    {{
        private readonly Dictionary<string, Type> _nameTypeMap = new Dictionary<string, Type>()
        {{
{1}
        }};

        public Type GetEmitterLogicTypeByName(string name)
        {{
            if (!_nameTypeMap.TryGetValue(name, out var type))
            {{
                var message = $""No emitter logic type exists with the name '{{name}}'"";
                throw new InvalidOperationException(message);
            }}            

            return type;
        }}
    }}
}}
";

            var mapping = new StringBuilder();
            foreach (var type in _emitterLogicTypes)
            {
                var stringName = type;
                if (stringName.EndsWith(MainParmePlugin.EmitterLogicSuffix))
                {
                    stringName = stringName.Substring(0, stringName.LastIndexOf(MainParmePlugin.EmitterLogicSuffix, StringComparison.Ordinal));
                }
                
                mapping.AppendLine($"            {{\"{stringName}\", typeof({type})}},");
            }

            return string.Format(template, projectNamespace, mapping);
        }
    }
}