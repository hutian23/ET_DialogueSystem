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
                    break;
                case CollisionResponses.Cross:
                    break;
                case CollisionResponses.Slide:
                    break;
                case CollisionResponses.Bounce:
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