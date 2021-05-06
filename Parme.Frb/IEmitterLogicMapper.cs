using System;

namespace Parme.Frb
{
    public interface IEmitterLogicMapper
    {
        Type GetEmitterLogicTypeByName(string name);
    }
}