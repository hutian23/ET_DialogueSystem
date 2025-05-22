using Box2DSharp.Dynamics;
using Timeline;

namespace ET
{
    public class B2ContactFilter: IContactFilter
    {
        public bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureA.UserData is not FixtureData dataA || fixtureB.UserData is not FixtureData dataB)
            {
                return false;
            }

            if (fixtureA.Body == fixtureB.Body)
            {
                return false;
            }

            // 在这里抛出回调
            // bool canCollide = EventSystem.Instance.Invoke<ContactFilterCallback, bool>(new ContactFilterCallback() { InstanceIdA = dataA.InstanceId, InstanceIdB = dataB.InstanceId});
            
            if (dataA.UserData is BoxInfo infoA && dataB.UserData is BoxInfo infoB)
            {
                // 编辑器阶段绘制的图形，不参与碰撞
                if (infoA.hitboxType is HitboxType.Gizmos || infoB.hitboxType is HitboxType.Gizmos)
                {
                    return false;
                }
                // Unit之间不会相互碰撞
                if (infoA.hitboxType is HitboxType.Squash && infoB.hitboxType is HitboxType.Squash && 
                    dataA.LayerType is LayerType.Unit && dataB.LayerType is LayerType.Unit)
                {
                    return false;
                }
            }

            return true;
        }
    }
}