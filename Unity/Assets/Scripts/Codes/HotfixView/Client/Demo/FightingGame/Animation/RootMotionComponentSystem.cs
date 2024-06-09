using Timeline;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (RootMotionComponent))]
    public static class RootMotionComponentSystem
    {
        public class RootMotionComponentAwakeSystem: AwakeSystem<RootMotionComponent>
        {
            protected override void Awake(RootMotionComponent self)
            {
            }
        }

        public static void Init(this RootMotionComponent self, long targetID)
        {
            GameObject go = self.GetParent<DialogueComponent>().GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            TimelinePlayer timelinePlayer = go.GetComponent<TimelinePlayer>();

            self.initPos = timelinePlayer.transform.position;
            Log.Warning(self.initPos.ToString());
        }
    }
}