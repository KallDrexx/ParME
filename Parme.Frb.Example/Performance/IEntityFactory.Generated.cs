using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parme.Frb.Example.Performance
{
    public interface IEntityFactory
    {
        object CreateNew(float x = 0, float y = 0);
        object CreateNew(global::FlatRedBall.Graphics.Layer layer);

        void Initialize(string contentManager);
        void ClearListsToAddTo();
    }
}
