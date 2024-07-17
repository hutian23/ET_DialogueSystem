using System.Drawing;

namespace AABB
{
    public interface ICollision
    {
        /// <summary>
        /// Gets the box that is moving and collides with an other one.
        /// </summary>
        protected IBox Box { get; }
        
        /// <summary>
        /// Gets the other box than being collided by the moving box.
        /// </summary>
        protected IBox Other { get; }
        
        /// <summary>
        /// Gets the goal position of the box move
        /// </summary>
        protected RectangleF Goal { get; }
        
        /// <summary>
        /// Gets information about the impact point
        /// </summary>
        protected IHit Hit { get; }
    }
}