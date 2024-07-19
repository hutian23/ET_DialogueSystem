using System.Collections.Generic;

namespace AABB
{
    public interface IMovement
    {
        public IEnumerable<IHit> Hits { get; }

        public bool HasCollided { get; }

        public RectangleF Origin { get; }

        public RectangleF Goal { get; }

        public RectangleF Destination { get; }
    }
}