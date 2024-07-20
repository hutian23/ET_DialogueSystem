using UnityEngine;

namespace AABB
{
    /// <summary>
    /// Represents a hit point out of a collision
    /// </summary>
    public interface IHit
    {
        //get the collided box
        public IBox Box { get; }
        
        //Gets the normal vector2 of the collided box side
        public Vector2 Normal { get; }
        
        //Gets the amount of movement needed from origin to get the impact position.
        public float Amount { get; }
        
        //Gets the impact position
        public Vector2 Position { get; }
        
        //Gets the amount of movement needed from impact position to get the requested initial goal position.
        public float Remaining { get; }

        //Indicates whether the hit point is nearer than an other from a given point.
        //Warning: this should only be used for multiple calculation of the same box movement
        //(amount is compared first, then distant)
        public bool IsNearest(IHit than, Vector2 from);
    }
}