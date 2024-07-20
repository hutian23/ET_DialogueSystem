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

        //current position
        public RectangleF Origin { get; set; }
        
        //target position
        public RectangleF Goal { get; set; }
        
        //after simulate collision, the actual position
        public RectangleF Destination { get; set; }
    }
}