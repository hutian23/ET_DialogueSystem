using System.Collections.Generic;
using System.Drawing;

namespace AABB
{
    public interface IMovement
    {
        protected IEnumerable<IHit> Hits { get; }

        protected bool HasCollided { get; }

        protected RectangleF Origin { get; }

        protected RectangleF Goal { get; }

        protected RectangleF Destination { get; }
    }
}