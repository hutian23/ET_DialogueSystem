namespace AABB
{
    public interface ICollision
    {
        /// <summary>
        /// Gets the box that is moving and collides with an other one.
        /// </summary>
        public IBox Box { get; }
        
        /// <summary>
        /// Gets the other box than being collided by the moving box.
        /// </summary>
        public IBox Other { get; }
        
        /// <summary>
        /// Gets the origin of the box move
        /// </summary>
        public RectangleF Origin { get; }
        
        /// <summary>
        /// Gets the goal position of the box move
        /// </summary>
        public RectangleF Goal { get; }
        
        /// <summary>
        /// Gets information about the impact point
        /// </summary>
        public IHit Hit { get; }
    }
}