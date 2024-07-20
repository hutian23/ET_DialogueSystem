using System;

namespace AABB
{
    public class CollisionResponse: ICollisionResponse
    {
        private CollisionResponse(ICollision col, CollisionResponses response)
        {
            switch (response)
            {
                case CollisionResponses.Touch:
                    child = new TouchResponse(col);
                    break;
                case CollisionResponses.Cross:
                    child = new CrossResponse(col);
                    break;
                case CollisionResponses.Slide:
                    child = new SlideResponse(col);
                    break;
                case CollisionResponses.Bounce:
                    child = new BoundsResponse(col);
                    break;
                default:
                    throw new ArgumentException("Unsupproted collision type");
            }
        }

        private ICollisionResponse child;

        public RectangleF Destination
        {
            get
            {
                return child.Destination;
            }
        }

        public static ICollisionResponse Create(ICollision col, CollisionResponses response)
        {
            if (response == CollisionResponses.None)
            {
                return null;
            }

            return new CollisionResponse(col, response);
        }
    }
}