using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorInfo))]
    [FriendOf(typeof (DialogueComponent))]
    public static class BehaviorInfoSystem
    {
        public static long GetSkillOrder(this BehaviorInfo self)
        {
            ulong result = 0;
            result |= (uint)self.order;
            result |= (ulong)self.BehaviorType << 32;
            return (long)result;
        }

        public static async ETTask InputCheckCor(this BehaviorInfo self, Unit unit, ETCancellationToken token)
        {
            while (true)
            {
                if (token.IsCancel()) return;

                BBCheckHandler checker = DialogueDispatcherComponent.Instance.GetBBCheckHandler(self.inputChecker);
                Status ret = await checker.Handle(unit, token);
                if (token.IsCancel()) return;

                if (ret == Status.Success)
                {
                    self.GetParent<BBInputComponent>().GetComponent<BBBehaviorBufferComponent>().AddBehaviorBuffer(self);
                }

                await TimerComponent.Instance.WaitFrameAsync(token);
            }
        }
    }
}