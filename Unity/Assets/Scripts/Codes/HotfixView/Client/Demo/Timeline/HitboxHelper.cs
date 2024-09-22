using Box2DSharp.Collision.Shapes;
using Box2DSharp.Testbed.Unity.Inspection;
using Timeline;

namespace ET.Client
{
    public static class HitboxHelper
    {
        public static void SetAsHitbox(this PolygonShape shape, BoxInfo boxInfo)
        {
            switch (boxInfo.hitboxType)
            {
                case HitboxType.Squash:
                    shape.SetAsBox(boxInfo.size.x / 2f, boxInfo.size.y / 2f, boxInfo.center.ToVector2(), 0);
                    break;
            }
        }
    }
}