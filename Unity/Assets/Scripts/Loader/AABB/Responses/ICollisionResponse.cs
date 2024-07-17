namespace AABB
{
    /// <summary>
    /// The result of a collision reaction onto a box position
    /// </summary>
    public interface ICollisionResponse
    {
        //equals unity.bounds
        //Gets the new destination of the box after the collision
        public RectangleF Destination { get; }
    }
}