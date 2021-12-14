
using System;
using System.Collections.Generic;
using Parme.Frb;

namespace Parme.Frb.Example.Particles
{
    public class ParmeEmitterLogicGenerator : IEmitterLogicMapper
    {
        private readonly Dictionary<string, Type> _nameTypeMap = new Dictionary<string, Type>()
        {
            {"Contrails", typeof(ContrailsEmitterLogic)},
            {"Directiontest", typeof(DirectiontestEmitterLogic)},
            {"Explosion", typeof(ExplosionEmitterLogic)},
            {"Test", typeof(TestEmitterLogic)},

        };

        public Type GetEmitterLogicTypeByName(string name)
        {
            if (!_nameTypeMap.TryGetValue(name, out var type))
            {
                var message = $"No emitter logic type exists with the name '{name}'";
                throw new InvalidOperationException(message);
            }            

            return type;
        }
    }
}
