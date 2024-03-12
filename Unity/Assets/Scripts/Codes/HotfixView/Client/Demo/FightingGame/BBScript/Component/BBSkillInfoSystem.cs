using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (BBSkillInfo))]
    [FriendOf(typeof (DialogueComponent))]
    public static class BBSkillInfoSystem
    {
        public static long GetSkillOrder(this BBSkillInfo self)
        {
            ulong result = 0;
            result |= (uint)self.order;
            result |= (ulong)self.skillType << 32;
            return (long)result;
        }

        public static async ETTask InputCheckCor(this BBSkillInfo self, Unit unit, ETCancellationToken token)
        {
            while (true)
            {
                if (token.IsCancel()) return;

                BBCheckHandler checker = DialogueDispatcherComponent.Instance.GetBBCheckHandler(self.inputChecker);
                Status ret = await checker.Handle(unit, token);
                if (token.IsCancel()) return;
                
                if (ret == Status.Success)
                {
                    BBTimerComponent bbTimerComponent = self.GetParent<BBInputComponent>().GetComponent<BBTimerComponent>();
                }

                await TimerComponent.Instance.WaitFrameAsync(token);
            }
        }
    }
}