using Box2DSharp.Dynamics;
using ET.Event;

namespace ET
{
    public class B2ContactFilter: IContactFilter
    {
        public bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureA.Body == fixtureB.Body)
            {
                return false;
            }

            long instanceIdA = (long)fixtureA.UserData;
            long instanceIdB = (long)fixtureB.UserData;
            return EventSystem.Instance.Invoke<ContactFilterCallback, bool>(new ContactFilterCallback() { InstanceIdA = instanceIdA, InstanceIdB = instanceIdB});
        }
    }
}