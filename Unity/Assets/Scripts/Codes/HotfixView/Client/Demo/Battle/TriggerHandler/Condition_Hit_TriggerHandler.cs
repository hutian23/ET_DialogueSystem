using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class Condition_Hit_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Hit";
        }

        //该trigger只能在 PostStep生命周期中使用
        //RegistCallback: (Hit: xxx), 'HitCheck'
        public override bool Check(BBParser parser, BBScriptData data)
        {
            b2Body b2Body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);

            int count = b2Body.triggerStayBuffers.Count;
            while (count-- > 0)
            {
                CollisionInfo info = b2Body.triggerStayBuffers.Dequeue();
                b2Body.triggerStayBuffers.Enqueue(info);

                BoxInfo boxInfoA = info.dataA.UserData as BoxInfo;
                BoxInfo boxInfoB = info.dataB.UserData as BoxInfo;
                if (boxInfoA.hitboxType is HitboxType.Hit && boxInfoB.hitboxType is HitboxType.Hurt)
                {
                    return true;
                }
            }
            return false;
        }
    }
}