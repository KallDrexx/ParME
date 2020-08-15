using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatRedBall.Graphics;

namespace Parme.FlatRedBall.Example.Performance
{
    public interface IEntityFactory
    {
        object CreateNew(float x = 0, float y = 0);
        object CreateNew(Layer layer);

        void Initialize(string contentManager);
        void ClearListsToAddTo();
    }
}
