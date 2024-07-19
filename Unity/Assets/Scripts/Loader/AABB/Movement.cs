using System;
using System.Collections.Generic;
using System.Linq;

namespace AABB
{
    public class Movement : IMovement
    {
        public Movement()
        {
            Hits = Array.Empty<IHit>();
        }
        public IEnumerable<IHit> Hits { get; set; }

        public bool HasCollided
        {
            get
            {
                return Hits.Any();
            }
        }

        public RectangleF Origin { get; set; }
        public RectangleF Goal { get; set; }
        public RectangleF Destination { get; set; }
    }
}