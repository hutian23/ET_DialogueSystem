using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorInfo))]
    public static class BehaviorInfoSystem
    {
        [Invoke(BBTimerInvokeType.BehaviorBuffer_TriggerTimer)]
        [FriendOf(typeof (BehaviorInfo))]
        public class BehaviorBufferCheckTimer: BBTimer<BehaviorInfo>
        {
            protected override void Run(BehaviorInfo self)
            {
                BBParser parser = self.GetParent<BehaviorBufferComponent>().GetParent<DialogueComponent>().GetComponent<BBParser>();
                bool ret = true;
                foreach (string trigger in self.triggers)
                {
                    Match match = Regex.Match(trigger, @"");
                    if (!match.Success)
                    {
                        DialogueHelper.ScripMatchError(trigger);
                    }

                    BBScriptData data = BBScriptData.Create(trigger, 0, 0);
                    bool res = DialogueDispatcherComponent.Instance.GetTrigger(match.Value).Check(parser, data);
                    if (!res)
                    {
                        ret = false;
                    }

                    data.Recycle();

                    if (!ret)
                    {
                        return;
                    }
                }

                // BehaviorBuffer buffer = BehaviorBuffer.Create(self.GetOrder());
            }
        }

        public static async ETTask InputCheckCor(this BehaviorInfo self)
        {
            await ETTask.CompletedTask;
        }

        private static long GetOrder(this BehaviorInfo self)
        {
            ulong result = 0;
            result |= self.order;
            result |= (ulong)self.skillType << 16;
            return (long)result;
        }
    }
}