using System;

namespace ET.Client
{
    [FriendOf(typeof(OffsetAbility))]
    public static class OffsetAbilitySystem
    {
        public class OffsetAbilityAwakeSystem : AwakeSystem<OffsetAbility>
        {
            protected override void Awake(OffsetAbility self)
            {
                self.buffDict.Clear();
            }
        }

        public class OffsetAbilityDestroySystem : DestroySystem<OffsetAbility>
        {
            protected override void Destroy(OffsetAbility self)
            {
                self.buffDict.Clear();
            }
        }

        public static void BuffOption(this OffsetAbility self, string behaviorName, int buffFrame)
        {
            long maxFrame = buffFrame + BBTimerManager.Instance.SceneTimer().GetNow();
            
            // 存在该技能的缓冲，更新缓冲的最大有效帧
            if (self.buffDict.TryGetValue(behaviorName, out long _maxFrame))
            {
                self.buffDict[behaviorName] = Math.Max(maxFrame, _maxFrame);
                return;
            }
            
            // 不存在该技能缓冲，添加
            self.buffDict.Add(behaviorName, maxFrame);
        }
    }
}