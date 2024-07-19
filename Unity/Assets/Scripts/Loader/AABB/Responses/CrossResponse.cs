namespace AABB
{
    public class CrossResponse: ICollisionResponse
    {
        public CrossResponse(ICollision collision)
        {
            Destination = collision.Goal;
        }

        public RectangleF Destination { get; }
    }
}