using System.Numerics;
using Testbed.Abstractions;

namespace ET.Client
{
    [FriendOf(typeof(ComboOffsetAbility))]
    public static class ComboOffsetAbilitySystem
    {
        public class ComboOffsetAbilityAwakeSystem : AwakeSystem<ComboOffsetAbility>
        {
            protected override void Awake(ComboOffsetAbility self)
            {
                self.bufferQueue.Clear();
                self.token = new ETCancellationToken();
                self.CheckCor().Coroutine();
            }
        }

        public class ComboOffsetAbilityDestroySystem : DestroySystem<ComboOffsetAbility>
        {
            protected override void Destroy(ComboOffsetAbility self)
            {
                self.bufferQueue.Clear();
                self.token.Cancel();
            }
        }

        public class ComboOffsetAbilityGizmosUpdateSystem : GizmosUpdateSystem<ComboOffsetAbility>
        {
            protected override void GizmosUpdate(ComboOffsetAbility self)
            {
                if (!Global.Settings.ShowComboOffset) return;
                
                // title
                Vector2 startPosition = new (5, 200f);
                float offset = 20f;
                b2WorldManager.Instance.DrawText(startPosition, "Combo Offset:");
                
                // 逐个打印
                int count = self.bufferQueue.Count;
                while (count -- > 0)
                {
                    ComboOffsetBuffer buffer = self.bufferQueue.Dequeue();
                    self.bufferQueue.Enqueue(buffer);
                    b2WorldManager.Instance.DrawText(startPosition + new Vector2(0, (self.bufferQueue.Count - count) * offset), $"{buffer.behaviorName}  {buffer.cnt}");
                }
            }
        }
        
        private static async ETTask CheckCor(this ComboOffsetAbility self)
        {
            BBTimerComponent bbTimer = self.GetParent<BuffManager>().GetParent<Unit>().GetComponent<BBTimerComponent>();

            while (true)
            {
                await bbTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) return;
                
                // 更新buffer计时器，过期了出列
                int count = self.bufferQueue.Count;
                while (count -- > 0)
                {
                    ComboOffsetBuffer buffer = self.bufferQueue.Dequeue();
                    if (buffer.cnt-- <= 0) continue;
                    self.bufferQueue.Enqueue(buffer);
                }
            }
        }

        public static void BuffComboOffset(this ComboOffsetAbility self, string behaviorName, int buffFrame)
        {
            ComboOffsetBuffer buffer = new() { behaviorName = behaviorName, buffFrame = buffFrame, cnt = buffFrame };
            self.bufferQueue.Enqueue(buffer);
        }
    }
}