using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(B2Unit))]
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
            B2Unit b2Unit = parser.GetParent<Unit>().GetComponent<B2Unit>();
            
            int count = b2Unit.TriggerBuffer.Count;
            while (count-- > 0)
            {
                CollisionInfo info = b2Unit.TriggerBuffer.Dequeue();
                b2Unit.TriggerBuffer.Enqueue(info);
                
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