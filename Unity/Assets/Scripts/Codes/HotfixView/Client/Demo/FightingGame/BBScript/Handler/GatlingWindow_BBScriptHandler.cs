using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (BehaviorBufferComponent))]
    public class GatlingWindow_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GatlingWindow";
        }

        //GatlingWindow: 10;
        //(GC窗口开启帧数，以战斗时间为准，hitstop不会影响这个)
        //GCWindow至少存在一帧，即当前帧缓冲的行为，下一帧才会执行
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"GatlingWindow: (?<behaviorTag>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            //窗口持续帧数
            int.TryParse(match.Groups["behaviorTag"].Value, out int lastedFrame);

            GatlingCancelCor(parser, lastedFrame).Coroutine();
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask GatlingCancelCor(BBParser parser, int lastedFrame)
        {
            DialogueComponent dialogueComponent = parser.GetParent<DialogueComponent>();
            BBTimerComponent bbTimer = dialogueComponent.GetComponent<BBTimerComponent>();
            BehaviorBufferComponent behaviorBuffer = dialogueComponent.GetComponent<BehaviorBufferComponent>();

            List<long> orderSet = behaviorBuffer.GCSet.OrderByDescending(order => order).ToList();

            int count = 0;
            long targetOrder = 0; // 取消到这个行为
            while (count++ < lastedFrame)
            {
                if (targetOrder != 0)
                {
                    parser.Cancel();
                    dialogueComponent.GetComponent<ObjectWait>().Notify(new WaitNextBehavior() { order = targetOrder });
                }
                
                //TODO 检测当前帧是否hit
                
                //找到优先级最高的可切换行为
                foreach (long order in orderSet.Where(order => behaviorBuffer.OrderSet.Contains(order)))
                {
                    targetOrder = order;
                    break;
                }

                await bbTimer.WaitFrameAsync(parser.cancellationToken);
                if(parser.cancellationToken.IsCancel()) return;
            }
        }
    }
}