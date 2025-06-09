using System.Numerics;
using Testbed.Abstractions;
using Timeline;

namespace ET.Client
{
    public static class PlayerManagerSystem
    {
        public class PlayerManagerAwakeSystem : AwakeSystem<PlayerManager>
        {
            protected override void Awake(PlayerManager self)
            {
                PlayerManager.Instance = self;
            }
        }
        
        public class PlayerManagerDestroySystem : DestroySystem<PlayerManager>
        {
            protected override void Destroy(PlayerManager self)
            {
                PlayerManager.Instance = null;
            }
        }
        
        [FriendOf(typeof(BehaviorInfo))]
        public class PlayerManagerGizmosUpdateSystem : GizmosUpdateSystem<PlayerManager>
        {
            protected override void GizmosUpdate(PlayerManager self)
            {
                if (!Global.Settings.ShowPlayerInfo) return;

                Unit unit = self.GetParent<Unit>();

                Vector2 startPosition = new(5f, 200f);
                b2WorldManager.Instance.DrawText(startPosition, "PlayerInfo: ");

                // BehaviorMachine
                BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
                int currentOrder = machine.GetCurrentOrder();
                string behaviorName = machine.GetInfoByOrder(currentOrder).behaviorName;
                b2WorldManager.Instance.DrawText(startPosition + new Vector2(0, 20), $"BehaviorMachine: ");
                b2WorldManager.Instance.DrawText(startPosition + new Vector2(0, 40), $"Order: {currentOrder}  behaviorName: {behaviorName}");
                
                // Timeline
                TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
                RuntimePlayable playable = timelineComponent.GetTimelinePlayer().RuntimePlayable;

                b2WorldManager.Instance.DrawText(startPosition + new Vector2(0, 60), "Timeline: ");
                int currentFrame = playable.GetNow();
                int totalFrame = playable.GetMaxFrame();
                string keyFrame = playable.GetKeyFrame(currentFrame);
                b2WorldManager.Instance.DrawText(startPosition + new Vector2(0, 80), $"Current Frame: {currentFrame}  Total Frame: {totalFrame}");
                b2WorldManager.Instance.DrawText(startPosition + new Vector2(0, 100), $"KeyFrame: {keyFrame}");
            }
        }
    }
}