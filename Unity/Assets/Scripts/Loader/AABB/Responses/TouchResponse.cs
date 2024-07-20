namespace AABB
{
    public class TouchResponse : ICollisionResponse
    {
        public TouchResponse(ICollision collision)
        {
            Destination = new RectangleF(collision.Hit.Position, collision.Goal.Size);
        }
        public RectangleF Destination { get; }
    }
}