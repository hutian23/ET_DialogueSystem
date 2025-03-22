using MongoDB.Bson;
using Timeline;
using UnityEngine;
using Camera = UnityEngine.Camera;

namespace ET.Client
{
    [FriendOf(typeof(B2Unit))]
    [FriendOf(typeof(InputWait))]
    [FriendOfAttribute(typeof(ET.Client.BehaviorInfo))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            // Vector3 half = new(HalfWidth, HalfHeight);
            //
            // Unit unit = Root.Instance.Get(parser.GetParam<long>("VC_Follow_Id")) as Unit;
            // GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            //
            // //1. 先不考虑死区的问题
            // Rect curRect = new(go.transform.position - half , new Vector2(HalfWidth, HalfHeight) * 2);
            // Rect confiner = parser.GetParam<Rect>("VC_Confiner_Rect");
            // Log.Warning(confiner.ToJson() + "   "+ curRect.ToJson());
            // TimelineComponent timelineComponent = parser.GetParent<Unit>().GetComponent<TimelineComponent>();
            // BehaviorMachine machine = parser.GetParent<Unit>().GetComponent<BehaviorMachine>();
            // Log.Warning(timelineComponent.GetTimelinePlayer().CurrentTimeline.ToJson());
            // Log.Warning(timelineComponent.GetTimelinePlayer().GetTimeline("Dummy_Idle").ToJson());

            Unit unit = parser.GetParent<Unit>();
            // TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            // BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            // BehaviorInfo behaviorInfo = machine.GetInfoByOrder(machine.GetCurrentOrder());
            //
            // BBTimeline _timeline = timelineComponent.GetTimelinePlayer().GetTimeline(behaviorInfo.behaviorName);
            //
            // timelineComponent.GetTimelinePlayer().Init(_timeline);
            //
            // Log.Warning(_timeline.ToJson());

            Log.Warning(machine.GetCurrentOrder().ToString());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}