using System.Numerics;
using Box2DSharp.Dynamics;
using Timeline;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class AirDashToGroundComponent : Entity, IAwake, IDestroy, IPostStep, IGizmosUpdate
    {
        public bool OnGround;
    }
    
    public class AirDashToGroundRaycastCallback : IRayCastCallback
    {
        public Vector2 Point;
        public Vector2 Normal;
        public bool Hit;
        
        public float RayCastCallback(Fixture fixture, in Vector2 point, in Vector2 normal, float fraction)
        {
            Point = point;
            Normal = normal;
            Hit = false;
            
            // 夹具类型不匹配
            FixtureData fixtureData = (FixtureData)fixture.UserData;
            if (fixtureData.IsTrigger || fixtureData.LayerMask is not LayerType.Ground)
            {
                //-1 表示射线无视这个夹具，检测下一个夹具
                return -1.0f;
            }

            Hit = true;
            
            // 当回调函数返回当前的fraction值时，这会指示Box2D"裁剪"射线，即缩短射线的有效长度，然后继续检测剩余的射线部分。
            // 这样做的目的是为了确保能找到离射线起点最近的交点。
            // Warning: 不能假设fixtures会按照从近到远的顺序被检测。不保证回调接收fixtures的顺序。
            // 此处返回0，不会检测下一个夹具。
            return 0.0f; 
        }

        public static AirDashToGroundRaycastCallback Create()
        {
            return ObjectPool.Instance.Fetch<AirDashToGroundRaycastCallback>();
        }

        public void Dispose()
        {
            Point = Vector2.Zero;
            Normal = Vector2.Zero;
            Hit = false;
            ObjectPool.Instance.Recycle(this);
        }
    }
} 